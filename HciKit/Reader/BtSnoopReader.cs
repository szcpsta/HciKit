// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;
using System.Text;

namespace HciKit.Reader;

public class BtSnoopReader(Stream stream, bool leaveOpen = false) : IAsyncDisposable
{
    public readonly record struct BtSnoopRecord(long Position, long TimestampMicros, ReadOnlyMemory<byte> Payload);

    private readonly Stream _stream = stream ?? throw new ArgumentNullException(nameof(stream));

    private const string IdentificationPattern = "btsnoop\0";
    private const uint VersionNumber = 1;

    // Unix Epoch
    private const long BtSnoopEpoch1970Us = 0x00DCDDB30F2F8000;
    // Bluetooth Epoch
    private const long BtSnoopEpoch2000Us = 0x00E03AB44A676000;

    private enum FileHeaderOffset
    {
        IdentificationPattern = 0,                          // 0
        VersionNumber = IdentificationPattern + 8,          // 8
        DatalinkType = VersionNumber + 4,                   // 12
        FileHeaderLength = DatalinkType + 4,                // 16
    }

    private enum PacketRecordOffset
    {
        OriginalLength = 0,                                 // 0
        IncludedLength = OriginalLength + 4,                // 4
        PacketFlag = IncludedLength + 4,                    // 8
        CumulativeDrops = PacketFlag + 4,                   // 12
        TimestampMicroseconds = CumulativeDrops + 4,        // 16
        HeaderLength = TimestampMicroseconds + 8,           // 24
    }

    private enum DatalinkCode
    {
        H1 = 1001,
        H4 = 1002,
        Bscp = 1003,
        H5 = 1004
    }

    [Flags]
    private enum PacketFlagBit
    {
        Direction = 0,  // Direction flag 0 = Sent, 1 = Received

        /*
        Note: Some Datalink Types already encode some or all of this information
        within the Packet Data. With these Datalink Types, these flags should be
        treated as informational only, and the value in the Packet Data should take
        precedence.
        */
        Command = 1,    // Command flag 0 = Data, 1 = Command/Event
    }

    public async IAsyncEnumerable<BtSnoopRecord> ReadAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        byte[] fileHeader = new byte[(int)FileHeaderOffset.FileHeaderLength];
        await _stream.ReadExactlyAsync(fileHeader, ct).ConfigureAwait(false);

        if (!IsFileHeaderValid(fileHeader))
        {
            throw new InvalidDataException("Invalid file header");
        }

        while (_stream.Position < _stream.Length)
        {
            ct.ThrowIfCancellationRequested();

            long recordPosition = _stream.Position;
            byte[] recordHeader = new byte[(int)PacketRecordOffset.HeaderLength];
            await _stream.ReadExactlyAsync(recordHeader, ct).ConfigureAwait(false);
            int recordLength = (int)GetIncludedLength(recordHeader);
            byte[] payload = new byte[recordLength];
            await _stream.ReadExactlyAsync(payload, ct).ConfigureAwait(false);

            yield return new BtSnoopRecord(Position: recordPosition,
                                            TimestampMicros: GetTimestampMicroseconds(recordHeader),
                                            Payload: payload);
        }
    }

    public DateTime GetDateTime(long timestampMicros)
    {
        long usSince1970 = checked(timestampMicros - BtSnoopEpoch1970Us);
        // us -> ticks (1us = 10 ticks)
        return DateTime.UnixEpoch.AddTicks(usSince1970 * 10);
    }

    private uint GetOriginalLength(ReadOnlySpan<byte> packetRecordHeader)
        => BinaryPrimitives.ReadUInt32BigEndian(packetRecordHeader.Slice((int)PacketRecordOffset.OriginalLength, 4));

    private uint GetIncludedLength(ReadOnlySpan<byte> packetRecordHeader)
        => BinaryPrimitives.ReadUInt32BigEndian(packetRecordHeader.Slice((int)PacketRecordOffset.IncludedLength, 4));

    private uint GetPacketFlag(ReadOnlySpan<byte> packetRecordHeader)
        => BinaryPrimitives.ReadUInt32BigEndian(packetRecordHeader.Slice((int)PacketRecordOffset.PacketFlag, 4));

    private uint GetCumulativeDrops(ReadOnlySpan<byte> packetRecordHeader)
        => BinaryPrimitives.ReadUInt32BigEndian(packetRecordHeader.Slice((int)PacketRecordOffset.CumulativeDrops, 4));

    private long GetTimestampMicroseconds(ReadOnlySpan<byte> packetRecordHeader)
        => (long)BinaryPrimitives.ReadUInt64BigEndian(packetRecordHeader.Slice((int)PacketRecordOffset.TimestampMicroseconds, 8));

    private bool IsFileHeaderValid(ReadOnlySpan<byte> header)
    {
        string identificationPattern = Encoding.ASCII.GetString(header.ToArray(), (int)FileHeaderOffset.IdentificationPattern, 8);
        uint versionNumber = BinaryPrimitives.ReadUInt32BigEndian(header.Slice((int)FileHeaderOffset.VersionNumber, 4));
        DatalinkCode datalinkCode = (DatalinkCode)BinaryPrimitives.ReadUInt32BigEndian(header.Slice((int)FileHeaderOffset.DatalinkType, 4));

        return identificationPattern == IdentificationPattern &&
               versionNumber == VersionNumber &&
               datalinkCode == DatalinkCode.H4;
    }

    public async ValueTask DisposeAsync()
    {
        if (!leaveOpen)
            await _stream.DisposeAsync().ConfigureAwait(false);
    }
}
