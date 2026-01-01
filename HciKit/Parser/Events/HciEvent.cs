// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public abstract class HciEvent : HciPacket
{
    public HciEventCode EventCode { get; }

    protected HciEvent(HciEventCode eventCode)
    {
        EventCode = eventCode;
    }

    public override string Name => "Event";
}

// 7.7.1 Inquiry Complete event
public sealed class InquiryCompleteEvent : HciEvent
{
    public byte Status { get; }

    public InquiryCompleteEvent(byte status)
        : base(new(HciEventCodes.InquiryComplete))
    {
        Status = status;
    }

    public static InquiryCompleteEvent Parse(ref HciSpanReader r)
    {
        return new InquiryCompleteEvent(r.ReadU8());
    }
}

// 7.7.2 Inquiry Result event
public sealed class InquiryResultEvent : HciEvent
{
    public byte NumResponses { get; }
    public IReadOnlyList<InquiryResponse> Responses { get; }

    public readonly struct InquiryResponse
    {
        public ulong BdAddr { get; }
        public byte PageScanRepetitionMode { get; }
        public ushort Reserved { get; }
        public uint ClassOfDevice { get; }
        public ushort ClockOffset { get; }

        public InquiryResponse(ulong bdAddr, byte pageScanRepetitionMode, ushort reserved, uint classOfDevice, ushort clockOffset)
        {
            BdAddr = bdAddr;
            PageScanRepetitionMode = pageScanRepetitionMode;
            Reserved = reserved;
            ClassOfDevice = classOfDevice;
            ClockOffset = clockOffset;
        }
    }

    public InquiryResultEvent(byte numResponses, InquiryResponse[] responses)
        : base(new(HciEventCodes.InquiryResult))
    {
        NumResponses = numResponses;
        Responses = responses;
    }

    public static InquiryResultEvent Parse(ref HciSpanReader r)
    {
        byte numResponses = r.ReadU8();
        InquiryResponse[] responses = new InquiryResponse[numResponses];
        for (int i = 0; i < numResponses; i++)
        {
            responses[i] = new InquiryResponse(r.ReadU48(), r.ReadU8(), r.ReadU16(), r.ReadU24(), r.ReadU16());
        }

        return new InquiryResultEvent(numResponses, responses);
    }
}

// 7.7.3 Connection Complete event
public sealed class ConnectionCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public ulong BdAddr { get; }
    public byte LinkType { get; }
    public byte EncryptionEnabled { get; }

    public ConnectionCompleteEvent(byte status, ushort connectionHandle, ulong bdAddr, byte linkType, byte encryptionEnabled)
        : base(new(HciEventCodes.ConnectionComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        BdAddr = bdAddr;
        LinkType = linkType;
        EncryptionEnabled = encryptionEnabled;
    }

    public static ConnectionCompleteEvent Parse(ref HciSpanReader r)
    {
        return new ConnectionCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU48(), r.ReadU8(), r.ReadU8());
    }
}

// 7.7.4 Connection Request event
public sealed class ConnectionRequestEvent : HciEvent
{
    public ulong BdAddr { get; }
    public uint ClassOfDevice { get; }
    public byte LinkType { get; }

    public ConnectionRequestEvent(ulong bdAddr, uint classOfDevice, byte linkType)
        : base(new(HciEventCodes.ConnectionRequest))
    {
        BdAddr = bdAddr;
        ClassOfDevice = classOfDevice;
        LinkType = linkType;
    }

    public static ConnectionRequestEvent Parse(ref HciSpanReader r)
    {
        return new ConnectionRequestEvent(r.ReadU48(), r.ReadU24(), r.ReadU8());
    }
}

// 7.7.5 Disconnection Complete event
public sealed class DisconnectionCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte Reason { get; }

    public DisconnectionCompleteEvent(byte status, ushort connectionHandle, byte reason)
        : base(new(HciEventCodes.DisconnectionComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        Reason = reason;
    }

    public static DisconnectionCompleteEvent Parse(ref HciSpanReader r)
    {
        return new DisconnectionCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU8());
    }
}

// 7.7.6 Authentication Complete event
public sealed class AuthenticationCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }

    public AuthenticationCompleteEvent(byte status, ushort connectionHandle)
        : base(new(HciEventCodes.AuthenticationComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
    }

    public static AuthenticationCompleteEvent Parse(ref HciSpanReader r)
    {
        return new AuthenticationCompleteEvent(r.ReadU8(), r.ReadU16());
    }
}

// 7.7.7 Remote Name Request Complete event
public sealed class RemoteNameRequestCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ulong BdAddr { get; }
    public byte[] RemoteName { get; }

    public RemoteNameRequestCompleteEvent(byte status, ulong bdAddr, byte[] remoteName)
        : base(new(HciEventCodes.RemoteNameRequestComplete))
    {
        Status = status;
        BdAddr = bdAddr;
        RemoteName = remoteName;
    }

    public static RemoteNameRequestCompleteEvent Parse(ref HciSpanReader r)
    {
        return new RemoteNameRequestCompleteEvent(r.ReadU8(), r.ReadU48(), r.ReadBytes(248).ToArray());
    }
}

// 7.7.8 Encryption Change event
public sealed class EncryptionChangeV1Event : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte EncryptionEnabled { get; }

    public EncryptionChangeV1Event(byte status, ushort connectionHandle, byte encryptionEnabled)
        : base(new(HciEventCodes.EncryptionChangeV1))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        EncryptionEnabled = encryptionEnabled;
    }

    public static EncryptionChangeV1Event Parse(ref HciSpanReader r)
    {
        return new EncryptionChangeV1Event(r.ReadU8(), r.ReadU16(), r.ReadU8());
    }
}

public sealed class EncryptionChangeV2Event : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte EncryptionEnabled { get; }
    public byte EncryptionKeySize { get; }

    public EncryptionChangeV2Event(byte status, ushort connectionHandle, byte encryptionEnabled, byte encryptionKeySize)
        : base(new(HciEventCodes.EncryptionChangeV2))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        EncryptionEnabled = encryptionEnabled;
        EncryptionKeySize = encryptionKeySize;
    }

    public static EncryptionChangeV2Event Parse(ref HciSpanReader r)
    {
        return new EncryptionChangeV2Event(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU8());
    }
}

// 7.7.9 Change Connection Link Key Complete event
public sealed class ChangeConnectionLinkKeyCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }

    public ChangeConnectionLinkKeyCompleteEvent(byte status, ushort connectionHandle)
        : base(new(HciEventCodes.ChangeConnectionLinkKeyComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
    }

    public static ChangeConnectionLinkKeyCompleteEvent Parse(ref HciSpanReader r)
    {
        return new ChangeConnectionLinkKeyCompleteEvent(r.ReadU8(), r.ReadU16());
    }
}

// 7.7.10 Link Key Type Changed event
public sealed class LinkKeyTypeChangedEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte KeyFlag { get; }

    public LinkKeyTypeChangedEvent(byte status, ushort connectionHandle, byte keyFlag)
        : base(new(HciEventCodes.LinkKeyTypeChanged))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        KeyFlag = keyFlag;
    }

    public static LinkKeyTypeChangedEvent Parse(ref HciSpanReader r)
    {
        return new LinkKeyTypeChangedEvent(r.ReadU8(), r.ReadU16(), r.ReadU8());
    }
}

// 7.7.11 Read Remote Supported Features Complete event
public sealed class ReadRemoteSupportedFeaturesCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public ulong LmpFeatures { get; }

    public ReadRemoteSupportedFeaturesCompleteEvent(byte status, ushort connectionHandle, ulong lmpFeatures)
        : base(new(HciEventCodes.ReadRemoteSupportedFeaturesComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        LmpFeatures = lmpFeatures;
    }

    public static ReadRemoteSupportedFeaturesCompleteEvent Parse(ref HciSpanReader r)
    {
        return new ReadRemoteSupportedFeaturesCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU64());
    }
}

// 7.7.12 Read Remote Version Information Complete event
public sealed class ReadRemoteVersionInformationCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte Version { get; }
    public ushort CompanyIdentifier { get; }
    public ushort Subversion { get; }

    public ReadRemoteVersionInformationCompleteEvent(byte status, ushort connectionHandle, byte version, ushort companyIdentifier, ushort subversion)
        : base(new(HciEventCodes.ReadRemoteVersionInformationComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        Version = version;
        CompanyIdentifier = companyIdentifier;
        Subversion = subversion;
    }

    public static ReadRemoteVersionInformationCompleteEvent Parse(ref HciSpanReader r)
    {
        return new ReadRemoteVersionInformationCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU16(), r.ReadU16());
    }
}

// 7.7.13 QoS Setup Complete event
public sealed class QoSSetupCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte Unused { get; }
    public byte ServiceType { get; }
    public uint TokenRate { get; }
    public uint PeakBandwidth { get; }
    public uint Latency { get; }
    public uint DelayVariation { get; }

    public QoSSetupCompleteEvent(byte status, ushort connectionHandle, byte unused, byte serviceType, uint tokenRate, uint peakBandwidth, uint latency, uint delayVariation)
        : base(new(HciEventCodes.QoSSetupComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        Unused = unused;
        ServiceType = serviceType;
        TokenRate = tokenRate;
        PeakBandwidth = peakBandwidth;
        Latency = latency;
        DelayVariation = delayVariation;
    }

    public static QoSSetupCompleteEvent Parse(ref HciSpanReader r)
    {
        return new QoSSetupCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU32(), r.ReadU32(), r.ReadU32(), r.ReadU32());
    }
}

// 7.7.14 Command Complete event
public sealed class CommandCompleteEvent : HciEvent
{
    public byte NumHciCommandPackets { get; }
    public ushort CommandOpcode { get; }
    public byte[] ReturnParameters { get; }

    public CommandCompleteEvent(byte numHciCommandPackets, ushort commandOpcode, byte[] returnParameters)
        : base(new(HciEventCodes.CommandComplete))
    {
        NumHciCommandPackets = numHciCommandPackets;
        CommandOpcode = commandOpcode;
        ReturnParameters = returnParameters;
    }

    public static CommandCompleteEvent Parse(ref HciSpanReader r)
    {
        return new CommandCompleteEvent(r.ReadU8(), r.ReadU16(), r.RemainingSpan.ToArray());
    }
}

// 7.7.15 Command Status event
public sealed class CommandStatusEvent : HciEvent
{
    public byte Status { get; }
    public byte NumHciCommandPackets { get; }
    public ushort CommandOpcode { get; }

    public CommandStatusEvent(byte status, byte numHciCommandPackets, ushort commandOpcode)
        : base(new HciEventCode(HciEventCodes.CommandStatus))
    {
        Status = status;
        NumHciCommandPackets = numHciCommandPackets;
        CommandOpcode = commandOpcode;
    }

    public static CommandStatusEvent Parse(ref HciSpanReader r)
    {
        return new CommandStatusEvent(r.ReadU8(), r.ReadU8(), r.ReadU16());
    }
}
