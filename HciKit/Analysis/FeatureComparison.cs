// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Commands;
using HciKit.Parser.Events;
using HciKit.Reader;
using System.Collections.ObjectModel;

namespace HciKit.Analysis;

public static class FeatureComparison
{
    public static async Task<FeatureComparisonResult> CompareAsync(string snoopFilePath, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(snoopFilePath))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(snoopFilePath));

        var parser = new HciParser();
        var state = new ComparisonState();

        await using var stream = File.OpenRead(snoopFilePath);
        await using var reader = new BtSnoopReader(stream);

        await foreach (var record in reader.ReadAsync(ct))
        {
            ct.ThrowIfCancellationRequested();
            var packet = parser.Parse(record.Payload.Span);

            switch (packet)
            {
                case ConnectionCompleteEvent connection when connection.Status == 0:
                    state.UpdateConnection(connection.ConnectionHandle, connection.BdAddr);
                    break;
                case ReadRemoteSupportedFeaturesCompleteEvent remoteFeatures when remoteFeatures.Status == 0:
                    state.AddRemoteFeatures(remoteFeatures.ConnectionHandle, pageNumber: 0, remoteFeatures.LmpFeatures);
                    break;
                case ReadRemoteExtendedFeaturesCompleteEvent remoteExtended when remoteExtended.Status == 0:
                    state.AddRemoteFeatures(remoteExtended.ConnectionHandle, remoteExtended.PageNumber, remoteExtended.ExtendedLmpFeatures);
                    break;
                case CommandCompleteEvent commandComplete:
                    if (TryParseLocalFeatures(commandComplete, out var localPage))
                        state.AddLocalFeatures(localPage.PageNumber, localPage.Features);
                    break;
            }
        }

        return state.ToResult();
    }

    // HCI stores BD_ADDR little-endian; format as big-endian hex with colons.
    public static string FormatBdAddr(ulong bdAddr)
    {
        Span<char> chars = stackalloc char[17];
        int index = 0;

        for (int i = 5; i >= 0; i--)
        {
            byte value = (byte)(bdAddr >> (i * 8));
            chars[index++] = ToHex(value >> 4);
            chars[index++] = ToHex(value & 0x0F);
            if (i != 0)
                chars[index++] = ':';
        }

        return new string(chars);
    }

    private static char ToHex(int value)
        => (char)(value < 10 ? '0' + value : 'A' + (value - 10));

    private static bool TryParseLocalFeatures(CommandCompleteEvent commandComplete, out LocalFeaturePage page)
    {
        page = default;

        if (commandComplete.CommandOpcode == HciOpcodes.ReadLocalSupportedFeatures)
        {
            if (commandComplete.ReturnParameters.Length < 9)
                return false;

            HciSpanReader r = new HciSpanReader(commandComplete.ReturnParameters);
            byte status = r.ReadU8();
            if (status != 0)
                return false;

            page = new LocalFeaturePage(pageNumber: 0, features: r.ReadU64());
            return true;
        }

        if (commandComplete.CommandOpcode == HciOpcodes.ReadLocalExtendedFeatures)
        {
            if (commandComplete.ReturnParameters.Length < 11)
                return false;

            HciSpanReader r = new HciSpanReader(commandComplete.ReturnParameters);
            byte status = r.ReadU8();
            if (status != 0)
                return false;

            byte pageNumber = r.ReadU8();
            _ = r.ReadU8(); // Max page number
            page = new LocalFeaturePage(pageNumber, r.ReadU64());
            return true;
        }

        return false;
    }

    private static List<FeaturePageDiff> BuildDiffs(
        IReadOnlyDictionary<byte, ulong> local,
        IReadOnlyDictionary<byte, ulong> remote)
    {
        var diffs = new List<FeaturePageDiff>();
        if (local.Count == 0)
            return diffs;

        SortedSet<byte> pages = new SortedSet<byte>();
        foreach (var page in local.Keys)
            pages.Add(page);
        foreach (var page in remote.Keys)
            pages.Add(page);

        foreach (byte page in pages)
        {
            local.TryGetValue(page, out ulong localFeatures);
            remote.TryGetValue(page, out ulong remoteFeatures);
            ulong missing = localFeatures & ~remoteFeatures;
            ulong extra = remoteFeatures & ~localFeatures;
            diffs.Add(new FeaturePageDiff(page, localFeatures, remoteFeatures, missing, extra));
        }

        return diffs;
    }

    private readonly record struct LocalFeaturePage(byte PageNumber, ulong Features);

    private sealed class ComparisonState
    {
        private readonly Dictionary<byte, ulong> _localFeaturesByPage = new();
        private readonly Dictionary<ushort, RemoteFeatureCollector> _remoteByHandle = new();

        public void AddLocalFeatures(byte pageNumber, ulong features)
        {
            _localFeaturesByPage[pageNumber] = features;
        }

        public void UpdateConnection(ushort connectionHandle, ulong bdAddr)
        {
            var remote = GetOrCreateRemote(connectionHandle);
            remote.BdAddr = bdAddr;
        }

        public void AddRemoteFeatures(ushort connectionHandle, byte pageNumber, ulong features)
        {
            var remote = GetOrCreateRemote(connectionHandle);
            remote.FeaturesByPage[pageNumber] = features;
        }

        public FeatureComparisonResult ToResult()
        {
            var locals = new Dictionary<byte, ulong>(_localFeaturesByPage);
            var remoteList = new List<RemoteFeatureCollector>(_remoteByHandle.Values);
            remoteList.Sort((left, right) => left.ConnectionHandle.CompareTo(right.ConnectionHandle));

            var comparisons = new List<RemoteFeatureComparison>(remoteList.Count);
            foreach (var remote in remoteList)
            {
                if (remote.FeaturesByPage.Count == 0)
                    continue;

                var remoteFeatures = new Dictionary<byte, ulong>(remote.FeaturesByPage);
                var diffs = BuildDiffs(locals, remoteFeatures);
                comparisons.Add(new RemoteFeatureComparison(remote.ConnectionHandle, remote.BdAddr, remoteFeatures, diffs));
            }

            return new FeatureComparisonResult(locals, comparisons);
        }

        private RemoteFeatureCollector GetOrCreateRemote(ushort connectionHandle)
        {
            if (!_remoteByHandle.TryGetValue(connectionHandle, out var remote))
            {
                remote = new RemoteFeatureCollector(connectionHandle);
                _remoteByHandle.Add(connectionHandle, remote);
            }

            return remote;
        }
    }

    private sealed class RemoteFeatureCollector
    {
        public ushort ConnectionHandle { get; }
        public ulong? BdAddr { get; set; }
        public Dictionary<byte, ulong> FeaturesByPage { get; } = new();
        public RemoteFeatureCollector(ushort connectionHandle)
        {
            ConnectionHandle = connectionHandle;
        }
    }
}

public sealed class FeatureComparisonResult
{
    public IReadOnlyDictionary<byte, ulong> LocalFeaturesByPage { get; }
    public IReadOnlyList<RemoteFeatureComparison> RemoteComparisons { get; }
    public bool HasLocalFeatures => LocalFeaturesByPage.Count > 0;

    internal FeatureComparisonResult(Dictionary<byte, ulong> localFeaturesByPage, List<RemoteFeatureComparison> remoteComparisons)
    {
        LocalFeaturesByPage = new Dictionary<byte, ulong>(localFeaturesByPage);
        RemoteComparisons = new ReadOnlyCollection<RemoteFeatureComparison>(remoteComparisons);
    }
}

public sealed class RemoteFeatureComparison
{
    public ushort ConnectionHandle { get; }
    public ulong? BdAddr { get; }
    public IReadOnlyDictionary<byte, ulong> RemoteFeaturesByPage { get; }
    public IReadOnlyList<FeaturePageDiff> PageDiffs { get; }
    public bool HasRemoteFeatures => RemoteFeaturesByPage.Count > 0;

    internal RemoteFeatureComparison(
        ushort connectionHandle,
        ulong? bdAddr,
        Dictionary<byte, ulong> remoteFeaturesByPage,
        List<FeaturePageDiff> pageDiffs)
    {
        ConnectionHandle = connectionHandle;
        BdAddr = bdAddr;
        RemoteFeaturesByPage = new Dictionary<byte, ulong>(remoteFeaturesByPage);
        PageDiffs = new ReadOnlyCollection<FeaturePageDiff>(pageDiffs);
    }
}

public readonly record struct FeaturePageDiff(
    byte PageNumber,
    ulong LocalFeatures,
    ulong RemoteFeatures,
    ulong MissingInRemote,
    ulong ExtraInRemote)
{
    public bool IsMatch => MissingInRemote == 0 && ExtraInRemote == 0;
}
