// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public abstract class HciEvent : HciPacket
{
    public HciEventCode EventCode { get; }

    protected HciEvent(HciEventCode eventCode) : base(HciPacketType.Event)
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

// 7.7.16 Hardware Error event
public sealed class HardwareErrorEvent : HciEvent
{
    public byte HardwareCode { get; }

    public HardwareErrorEvent(byte hardwareCode)
        : base(new(HciEventCodes.HardwareError))
    {
        HardwareCode = hardwareCode;
    }

    public static HardwareErrorEvent Parse(ref HciSpanReader r)
    {
        return new HardwareErrorEvent(r.ReadU8());
    }
}

// 7.7.17 Flush Occurred event
public sealed class FlushOccurredEvent : HciEvent
{
    public ushort ConnectionHandle { get; }

    public FlushOccurredEvent(ushort connectionHandle)
        : base(new(HciEventCodes.FlushOccurred))
    {
        ConnectionHandle = connectionHandle;
    }

    public static FlushOccurredEvent Parse(ref HciSpanReader r)
    {
        return new FlushOccurredEvent(r.ReadU16());
    }
}

// 7.7.18 Role Change event
public sealed class RoleChangeEvent : HciEvent
{
    public byte Status { get; }
    public ulong BdAddr { get; }
    public byte NewRole { get; }

    public RoleChangeEvent(byte status, ulong bdAddr, byte newRole)
        : base(new(HciEventCodes.RoleChange))
    {
        Status = status;
        BdAddr = bdAddr;
        NewRole = newRole;
    }

    public static RoleChangeEvent Parse(ref HciSpanReader r)
    {
        return new RoleChangeEvent(r.ReadU8(), r.ReadU48(), r.ReadU8());
    }
}

// 7.7.19 Number Of Completed Packets event
public sealed class NumberOfCompletedPacketsEvent : HciEvent
{
    public byte NumHandles { get; }
    public IReadOnlyList<CompletedPacket> Handles { get; }

    public readonly struct CompletedPacket
    {
        public ushort ConnectionHandle { get; }
        public ushort NumCompletedPackets { get; }

        public CompletedPacket(ushort connectionHandle, ushort numCompletedPackets)
        {
            ConnectionHandle = connectionHandle;
            NumCompletedPackets = numCompletedPackets;
        }
    }

    public NumberOfCompletedPacketsEvent(byte numHandles, CompletedPacket[] handles)
        : base(new(HciEventCodes.NumberOfCompletedPackets))
    {
        NumHandles = numHandles;
        Handles = handles;
    }

    public static NumberOfCompletedPacketsEvent Parse(ref HciSpanReader r)
    {
        byte numHandles = r.ReadU8();
        CompletedPacket[] handles = new CompletedPacket[numHandles];
        for (int i = 0; i < numHandles; i++)
        {
            handles[i] = new CompletedPacket(r.ReadU16(), r.ReadU16());
        }

        return new NumberOfCompletedPacketsEvent(numHandles, handles);
    }
}

// 7.7.20 Mode Change event
public sealed class ModeChangeEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte CurrentMode { get; }
    public ushort Interval { get; }

    public ModeChangeEvent(byte status, ushort connectionHandle, byte currentMode, ushort interval)
        : base(new(HciEventCodes.ModeChange))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        CurrentMode = currentMode;
        Interval = interval;
    }

    public static ModeChangeEvent Parse(ref HciSpanReader r)
    {
        return new ModeChangeEvent(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU16());
    }
}

// 7.7.21 Return Link Keys event
public sealed class ReturnLinkKeysEvent : HciEvent
{
    public byte NumKeys { get; }
    public IReadOnlyList<LinkKeyEntry> Keys { get; }

    public readonly struct LinkKeyEntry
    {
        public ulong BdAddr { get; }
        public byte[] LinkKey { get; }

        public LinkKeyEntry(ulong bdAddr, byte[] linkKey)
        {
            BdAddr = bdAddr;
            LinkKey = linkKey;
        }
    }

    public ReturnLinkKeysEvent(byte numKeys, LinkKeyEntry[] keys)
        : base(new(HciEventCodes.ReturnLinkKeys))
    {
        NumKeys = numKeys;
        Keys = keys;
    }

    public static ReturnLinkKeysEvent Parse(ref HciSpanReader r)
    {
        byte numKeys = r.ReadU8();
        LinkKeyEntry[] keys = new LinkKeyEntry[numKeys];
        for (int i = 0; i < numKeys; i++)
        {
            keys[i] = new LinkKeyEntry(r.ReadU48(), r.ReadBytes(16).ToArray());
        }

        return new ReturnLinkKeysEvent(numKeys, keys);
    }
}

// 7.7.22 PIN Code Request event
public sealed class PinCodeRequestEvent : HciEvent
{
    public ulong BdAddr { get; }

    public PinCodeRequestEvent(ulong bdAddr)
        : base(new(HciEventCodes.PinCodeRequest))
    {
        BdAddr = bdAddr;
    }

    public static PinCodeRequestEvent Parse(ref HciSpanReader r)
    {
        return new PinCodeRequestEvent(r.ReadU48());
    }
}

// 7.7.23 Link Key Request event
public sealed class LinkKeyRequestEvent : HciEvent
{
    public ulong BdAddr { get; }

    public LinkKeyRequestEvent(ulong bdAddr)
        : base(new(HciEventCodes.LinkKeyRequest))
    {
        BdAddr = bdAddr;
    }

    public static LinkKeyRequestEvent Parse(ref HciSpanReader r)
    {
        return new LinkKeyRequestEvent(r.ReadU48());
    }
}

// 7.7.24 Link Key Notification event
public sealed class LinkKeyNotificationEvent : HciEvent
{
    public ulong BdAddr { get; }
    public byte[] LinkKey { get; }
    public byte KeyType { get; }

    public LinkKeyNotificationEvent(ulong bdAddr, byte[] linkKey, byte keyType)
        : base(new(HciEventCodes.LinkKeyNotification))
    {
        BdAddr = bdAddr;
        LinkKey = linkKey;
        KeyType = keyType;
    }

    public static LinkKeyNotificationEvent Parse(ref HciSpanReader r)
    {
        return new LinkKeyNotificationEvent(r.ReadU48(), r.ReadBytes(16).ToArray(), r.ReadU8());
    }
}

// 7.7.25 Loopback Command event
public sealed class LoopbackCommandEvent : HciEvent
{
    public byte[] HciCommandPacket { get; }

    public LoopbackCommandEvent(byte[] hciCommandPacket)
        : base(new(HciEventCodes.LoopbackCommand))
    {
        HciCommandPacket = hciCommandPacket;
    }

    public static LoopbackCommandEvent Parse(ref HciSpanReader r)
    {
        return new LoopbackCommandEvent(r.RemainingSpan.ToArray());
    }
}

// 7.7.26 Data Buffer Overflow event
public sealed class DataBufferOverflowEvent : HciEvent
{
    public byte LinkType { get; }

    public DataBufferOverflowEvent(byte linkType)
        : base(new(HciEventCodes.DataBufferOverflow))
    {
        LinkType = linkType;
    }

    public static DataBufferOverflowEvent Parse(ref HciSpanReader r)
    {
        return new DataBufferOverflowEvent(r.ReadU8());
    }
}

// 7.7.27 Max Slots Change event
public sealed class MaxSlotsChangeEvent : HciEvent
{
    public ushort ConnectionHandle { get; }
    public byte LmpMaxSlots { get; }

    public MaxSlotsChangeEvent(ushort connectionHandle, byte lmpMaxSlots)
        : base(new(HciEventCodes.MaxSlotsChange))
    {
        ConnectionHandle = connectionHandle;
        LmpMaxSlots = lmpMaxSlots;
    }

    public static MaxSlotsChangeEvent Parse(ref HciSpanReader r)
    {
        return new MaxSlotsChangeEvent(r.ReadU16(), r.ReadU8());
    }
}

// 7.7.28 Read Clock Offset Complete event
public sealed class ReadClockOffsetCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public ushort ClockOffset { get; }

    public ReadClockOffsetCompleteEvent(byte status, ushort connectionHandle, ushort clockOffset)
        : base(new(HciEventCodes.ReadClockOffsetComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        ClockOffset = clockOffset;
    }

    public static ReadClockOffsetCompleteEvent Parse(ref HciSpanReader r)
    {
        return new ReadClockOffsetCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU16());
    }
}

// 7.7.29 Connection Packet Type Changed event
public sealed class ConnectionPacketTypeChangedEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public ushort PacketType { get; }

    public ConnectionPacketTypeChangedEvent(byte status, ushort connectionHandle, ushort packetType)
        : base(new(HciEventCodes.ConnectionPacketTypeChanged))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        PacketType = packetType;
    }

    public static ConnectionPacketTypeChangedEvent Parse(ref HciSpanReader r)
    {
        return new ConnectionPacketTypeChangedEvent(r.ReadU8(), r.ReadU16(), r.ReadU16());
    }
}

// 7.7.30 QoS Violation event
public sealed class QoSViolationEvent : HciEvent
{
    public ushort ConnectionHandle { get; }

    public QoSViolationEvent(ushort connectionHandle)
        : base(new(HciEventCodes.QoSViolation))
    {
        ConnectionHandle = connectionHandle;
    }

    public static QoSViolationEvent Parse(ref HciSpanReader r)
    {
        return new QoSViolationEvent(r.ReadU16());
    }
}

// 7.7.31 Page Scan Repetition Mode Change event
public sealed class PageScanRepetitionModeChangeEvent : HciEvent
{
    public ulong BdAddr { get; }
    public byte PageScanRepetitionMode { get; }

    public PageScanRepetitionModeChangeEvent(ulong bdAddr, byte pageScanRepetitionMode)
        : base(new(HciEventCodes.PageScanRepetitionModeChange))
    {
        BdAddr = bdAddr;
        PageScanRepetitionMode = pageScanRepetitionMode;
    }

    public static PageScanRepetitionModeChangeEvent Parse(ref HciSpanReader r)
    {
        return new PageScanRepetitionModeChangeEvent(r.ReadU48(), r.ReadU8());
    }
}

// 7.7.32 Flow Specification Complete event
public sealed class FlowSpecificationCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte Unused { get; }
    public byte FlowDirection { get; }
    public byte ServiceType { get; }
    public uint TokenRate { get; }
    public uint TokenBucketSize { get; }
    public uint PeakBandwidth { get; }
    public uint AccessLatency { get; }

    public FlowSpecificationCompleteEvent(byte status, ushort connectionHandle, byte unused, byte flowDirection, byte serviceType,
                                            uint tokenRate, uint tokenBucketSize, uint peakBandwidth, uint accessLatency)
        : base(new(HciEventCodes.FlowSpecificationComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        Unused = unused;
        FlowDirection = flowDirection;
        ServiceType = serviceType;
        TokenRate = tokenRate;
        TokenBucketSize = tokenBucketSize;
        PeakBandwidth = peakBandwidth;
        AccessLatency = accessLatency;
    }

    public static FlowSpecificationCompleteEvent Parse(ref HciSpanReader r)
    {
        return new FlowSpecificationCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU8(),
                                                    r.ReadU32(), r.ReadU32(), r.ReadU32(), r.ReadU32());
    }
}

// 7.7.33 Inquiry Result with RSSI event
public sealed class InquiryResultWithRssiEvent : HciEvent
{
    public byte NumResponses { get; }
    public IReadOnlyList<InquiryResponseWithRssi> Responses { get; }

    public readonly struct InquiryResponseWithRssi
    {
        public ulong BdAddr { get; }
        public byte PageScanRepetitionMode { get; }
        public byte Reserved { get; }
        public uint ClassOfDevice { get; }
        public ushort ClockOffset { get; }
        public sbyte Rssi { get; }

        public InquiryResponseWithRssi(ulong bdAddr, byte pageScanRepetitionMode, byte reserved, uint classOfDevice,
                                        ushort clockOffset, sbyte rssi)
        {
            BdAddr = bdAddr;
            PageScanRepetitionMode = pageScanRepetitionMode;
            Reserved = reserved;
            ClassOfDevice = classOfDevice;
            ClockOffset = clockOffset;
            Rssi = rssi;
        }
    }

    public InquiryResultWithRssiEvent(byte numResponses, InquiryResponseWithRssi[] responses)
        : base(new(HciEventCodes.InquiryResultWithRssi))
    {
        NumResponses = numResponses;
        Responses = responses;
    }

    public static InquiryResultWithRssiEvent Parse(ref HciSpanReader r)
    {
        byte numResponses = r.ReadU8();
        InquiryResponseWithRssi[] responses = new InquiryResponseWithRssi[numResponses];
        for (int i = 0; i < numResponses; i++)
        {
            responses[i] = new InquiryResponseWithRssi(r.ReadU48(), r.ReadU8(), r.ReadU8(), r.ReadU24(), r.ReadU16(), r.Read8());
        }

        return new InquiryResultWithRssiEvent(numResponses, responses);
    }
}

// 7.7.34 Read Remote Extended Features Complete event
public sealed class ReadRemoteExtendedFeaturesCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte PageNumber { get; }
    public byte MaxPageNumber { get; }
    public ulong ExtendedLmpFeatures { get; }

    public ReadRemoteExtendedFeaturesCompleteEvent(byte status, ushort connectionHandle, byte pageNumber, byte maxPageNumber,
                                                    ulong extendedLmpFeatures)
        : base(new(HciEventCodes.ReadRemoteExtendedFeaturesComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        PageNumber = pageNumber;
        MaxPageNumber = maxPageNumber;
        ExtendedLmpFeatures = extendedLmpFeatures;
    }

    public static ReadRemoteExtendedFeaturesCompleteEvent Parse(ref HciSpanReader r)
    {
        return new ReadRemoteExtendedFeaturesCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU64());
    }
}

// 7.7.35 Synchronous Connection Complete event
public sealed class SynchronousConnectionCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public ulong BdAddr { get; }
    public byte LinkType { get; }
    public byte TransmissionInterval { get; }
    public byte RetransmissionWindow { get; }
    public ushort RxPacketLength { get; }
    public ushort TxPacketLength { get; }
    public byte AirMode { get; }

    public SynchronousConnectionCompleteEvent(byte status, ushort connectionHandle, ulong bdAddr, byte linkType,
                                                byte transmissionInterval, byte retransmissionWindow,
                                                ushort rxPacketLength, ushort txPacketLength, byte airMode)
        : base(new(HciEventCodes.SynchronousConnectionComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        BdAddr = bdAddr;
        LinkType = linkType;
        TransmissionInterval = transmissionInterval;
        RetransmissionWindow = retransmissionWindow;
        RxPacketLength = rxPacketLength;
        TxPacketLength = txPacketLength;
        AirMode = airMode;
    }

    public static SynchronousConnectionCompleteEvent Parse(ref HciSpanReader r)
    {
        return new SynchronousConnectionCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU48(), r.ReadU8(),
                                                        r.ReadU8(), r.ReadU8(), r.ReadU16(), r.ReadU16(), r.ReadU8());
    }
}

// 7.7.36 Synchronous Connection Changed event
public sealed class SynchronousConnectionChangedEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte TransmissionInterval { get; }
    public byte RetransmissionWindow { get; }
    public ushort RxPacketLength { get; }
    public ushort TxPacketLength { get; }

    public SynchronousConnectionChangedEvent(byte status, ushort connectionHandle, byte transmissionInterval, byte retransmissionWindow,
                                            ushort rxPacketLength, ushort txPacketLength)
        : base(new(HciEventCodes.SynchronousConnectionChanged))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        TransmissionInterval = transmissionInterval;
        RetransmissionWindow = retransmissionWindow;
        RxPacketLength = rxPacketLength;
        TxPacketLength = txPacketLength;
    }

    public static SynchronousConnectionChangedEvent Parse(ref HciSpanReader r)
    {
        return new SynchronousConnectionChangedEvent(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU16(), r.ReadU16());
    }
}

// 7.7.37 Sniff Subrating event
public sealed class SniffSubratingEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public ushort MaxTxLatency { get; }
    public ushort MaxRxLatency { get; }
    public ushort MinRemoteTimeout { get; }
    public ushort MinLocalTimeout { get; }

    public SniffSubratingEvent(byte status, ushort connectionHandle, ushort maxTxLatency, ushort maxRxLatency,
                                ushort minRemoteTimeout, ushort minLocalTimeout)
        : base(new(HciEventCodes.SniffSubrating))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        MaxTxLatency = maxTxLatency;
        MaxRxLatency = maxRxLatency;
        MinRemoteTimeout = minRemoteTimeout;
        MinLocalTimeout = minLocalTimeout;
    }

    public static SniffSubratingEvent Parse(ref HciSpanReader r)
    {
        return new SniffSubratingEvent(r.ReadU8(), r.ReadU16(), r.ReadU16(), r.ReadU16(), r.ReadU16(), r.ReadU16());
    }
}

// 7.7.38 Extended Inquiry Result event
public sealed class ExtendedInquiryResultEvent : HciEvent
{
    public byte NumResponses { get; }
    public ulong BdAddr { get; }
    public byte PageScanRepetitionMode { get; }
    public byte Reserved { get; }
    public uint ClassOfDevice { get; }
    public ushort ClockOffset { get; }
    public sbyte Rssi { get; }
    public byte[] ExtendedInquiryResponse { get; }

    public ExtendedInquiryResultEvent(byte numResponses, ulong bdAddr, byte pageScanRepetitionMode, byte reserved,
                                        uint classOfDevice, ushort clockOffset, sbyte rssi, byte[] extendedInquiryResponse)
        : base(new(HciEventCodes.ExtendedInquiryResult))
    {
        NumResponses = numResponses;
        BdAddr = bdAddr;
        PageScanRepetitionMode = pageScanRepetitionMode;
        Reserved = reserved;
        ClassOfDevice = classOfDevice;
        ClockOffset = clockOffset;
        Rssi = rssi;
        ExtendedInquiryResponse = extendedInquiryResponse;
    }

    public static ExtendedInquiryResultEvent Parse(ref HciSpanReader r)
    {
        return new ExtendedInquiryResultEvent(r.ReadU8(), r.ReadU48(), r.ReadU8(), r.ReadU8(),
                                                r.ReadU24(), r.ReadU16(), r.Read8(), r.ReadBytes(240).ToArray());
    }
}

// 7.7.39 Encryption Key Refresh Complete event
public sealed class EncryptionKeyRefreshCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }

    public EncryptionKeyRefreshCompleteEvent(byte status, ushort connectionHandle)
        : base(new(HciEventCodes.EncryptionKeyRefreshComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
    }

    public static EncryptionKeyRefreshCompleteEvent Parse(ref HciSpanReader r)
    {
        return new EncryptionKeyRefreshCompleteEvent(r.ReadU8(), r.ReadU16());
    }
}

// 7.7.40 IO Capability Request event
public sealed class IoCapabilityRequestEvent : HciEvent
{
    public ulong BdAddr { get; }

    public IoCapabilityRequestEvent(ulong bdAddr)
        : base(new(HciEventCodes.IoCapabilityRequest))
    {
        BdAddr = bdAddr;
    }

    public static IoCapabilityRequestEvent Parse(ref HciSpanReader r)
    {
        return new IoCapabilityRequestEvent(r.ReadU48());
    }
}

// 7.7.41 IO Capability Response event
public sealed class IoCapabilityResponseEvent : HciEvent
{
    public ulong BdAddr { get; }
    public byte IoCapability { get; }
    public byte OobDataPresent { get; }
    public byte AuthenticationRequirements { get; }

    public IoCapabilityResponseEvent(ulong bdAddr, byte ioCapability, byte oobDataPresent, byte authenticationRequirements)
        : base(new(HciEventCodes.IoCapabilityResponse))
    {
        BdAddr = bdAddr;
        IoCapability = ioCapability;
        OobDataPresent = oobDataPresent;
        AuthenticationRequirements = authenticationRequirements;
    }

    public static IoCapabilityResponseEvent Parse(ref HciSpanReader r)
    {
        return new IoCapabilityResponseEvent(r.ReadU48(), r.ReadU8(), r.ReadU8(), r.ReadU8());
    }
}

// 7.7.42 User Confirmation Request event
public sealed class UserConfirmationRequestEvent : HciEvent
{
    public ulong BdAddr { get; }
    public uint NumericValue { get; }

    public UserConfirmationRequestEvent(ulong bdAddr, uint numericValue)
        : base(new(HciEventCodes.UserConfirmationRequest))
    {
        BdAddr = bdAddr;
        NumericValue = numericValue;
    }

    public static UserConfirmationRequestEvent Parse(ref HciSpanReader r)
    {
        return new UserConfirmationRequestEvent(r.ReadU48(), r.ReadU32());
    }
}

// 7.7.43 User Passkey Request event
public sealed class UserPasskeyRequestEvent : HciEvent
{
    public ulong BdAddr { get; }

    public UserPasskeyRequestEvent(ulong bdAddr)
        : base(new(HciEventCodes.UserPasskeyRequest))
    {
        BdAddr = bdAddr;
    }

    public static UserPasskeyRequestEvent Parse(ref HciSpanReader r)
    {
        return new UserPasskeyRequestEvent(r.ReadU48());
    }
}

// 7.7.44 Remote OOB Data Request event
public sealed class RemoteOobDataRequestEvent : HciEvent
{
    public ulong BdAddr { get; }

    public RemoteOobDataRequestEvent(ulong bdAddr)
        : base(new(HciEventCodes.RemoteOobDataRequest))
    {
        BdAddr = bdAddr;
    }

    public static RemoteOobDataRequestEvent Parse(ref HciSpanReader r)
    {
        return new RemoteOobDataRequestEvent(r.ReadU48());
    }
}

// 7.7.45 Simple Pairing Complete event
public sealed class SimplePairingCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ulong BdAddr { get; }

    public SimplePairingCompleteEvent(byte status, ulong bdAddr)
        : base(new(HciEventCodes.SimplePairingComplete))
    {
        Status = status;
        BdAddr = bdAddr;
    }

    public static SimplePairingCompleteEvent Parse(ref HciSpanReader r)
    {
        return new SimplePairingCompleteEvent(r.ReadU8(), r.ReadU48());
    }
}

// 7.7.46 Link Supervision Timeout Changed event
public sealed class LinkSupervisionTimeoutChangedEvent : HciEvent
{
    public ushort ConnectionHandle { get; }
    public ushort LinkSupervisionTimeout { get; }

    public LinkSupervisionTimeoutChangedEvent(ushort connectionHandle, ushort linkSupervisionTimeout)
        : base(new(HciEventCodes.LinkSupervisionTimeoutChanged))
    {
        ConnectionHandle = connectionHandle;
        LinkSupervisionTimeout = linkSupervisionTimeout;
    }

    public static LinkSupervisionTimeoutChangedEvent Parse(ref HciSpanReader r)
    {
        return new LinkSupervisionTimeoutChangedEvent(r.ReadU16(), r.ReadU16());
    }
}

// 7.7.47 Enhanced Flush Complete event
public sealed class EnhancedFlushCompleteEvent : HciEvent
{
    public ushort ConnectionHandle { get; }

    public EnhancedFlushCompleteEvent(ushort connectionHandle)
        : base(new(HciEventCodes.EnhancedFlushComplete))
    {
        ConnectionHandle = connectionHandle;
    }

    public static EnhancedFlushCompleteEvent Parse(ref HciSpanReader r)
    {
        return new EnhancedFlushCompleteEvent(r.ReadU16());
    }
}

// 7.7.48 User Passkey Notification event
public sealed class UserPasskeyNotificationEvent : HciEvent
{
    public ulong BdAddr { get; }
    public uint Passkey { get; }

    public UserPasskeyNotificationEvent(ulong bdAddr, uint passkey)
        : base(new(HciEventCodes.UserPasskeyNotification))
    {
        BdAddr = bdAddr;
        Passkey = passkey;
    }

    public static UserPasskeyNotificationEvent Parse(ref HciSpanReader r)
    {
        return new UserPasskeyNotificationEvent(r.ReadU48(), r.ReadU32());
    }
}

// 7.7.49 Keypress Notification event
public sealed class KeypressNotificationEvent : HciEvent
{
    public ulong BdAddr { get; }
    public byte NotificationType { get; }

    public KeypressNotificationEvent(ulong bdAddr, byte notificationType)
        : base(new(HciEventCodes.KeypressNotification))
    {
        BdAddr = bdAddr;
        NotificationType = notificationType;
    }

    public static KeypressNotificationEvent Parse(ref HciSpanReader r)
    {
        return new KeypressNotificationEvent(r.ReadU48(), r.ReadU8());
    }
}

// 7.7.50 Remote Host Supported Features Notification event
public sealed class RemoteHostSupportedFeaturesNotificationEvent : HciEvent
{
    public ulong BdAddr { get; }
    public ulong HostSupportedFeatures { get; }

    public RemoteHostSupportedFeaturesNotificationEvent(ulong bdAddr, ulong hostSupportedFeatures)
        : base(new(HciEventCodes.RemoteHostSupportedFeaturesNotification))
    {
        BdAddr = bdAddr;
        HostSupportedFeatures = hostSupportedFeatures;
    }

    public static RemoteHostSupportedFeaturesNotificationEvent Parse(ref HciSpanReader r)
    {
        return new RemoteHostSupportedFeaturesNotificationEvent(r.ReadU48(), r.ReadU64());
    }
}

// 7.7.59 Number Of Completed Data Blocks event
public sealed class NumberOfCompletedDataBlocksEvent : HciEvent
{
    public ushort TotalNumDataBlocks { get; }
    public byte NumHandles { get; }
    public IReadOnlyList<CompletedDataBlock> Handles { get; }

    public readonly struct CompletedDataBlock
    {
        public ushort ConnectionHandle { get; }
        public ushort NumCompletedPackets { get; }
        public ushort NumCompletedBlocks { get; }

        public CompletedDataBlock(ushort connectionHandle, ushort numCompletedPackets, ushort numCompletedBlocks)
        {
            ConnectionHandle = connectionHandle;
            NumCompletedPackets = numCompletedPackets;
            NumCompletedBlocks = numCompletedBlocks;
        }
    }

    public NumberOfCompletedDataBlocksEvent(ushort totalNumDataBlocks, byte numHandles, CompletedDataBlock[] handles)
        : base(new(HciEventCodes.NumberOfCompletedDataBlocks))
    {
        TotalNumDataBlocks = totalNumDataBlocks;
        NumHandles = numHandles;
        Handles = handles;
    }

    public static NumberOfCompletedDataBlocksEvent Parse(ref HciSpanReader r)
    {
        ushort totalNumDataBlocks = r.ReadU16();
        byte numHandles = r.ReadU8();
        CompletedDataBlock[] handles = new CompletedDataBlock[numHandles];
        for (int i = 0; i < numHandles; i++)
        {
            handles[i] = new CompletedDataBlock(r.ReadU16(), r.ReadU16(), r.ReadU16());
        }

        return new NumberOfCompletedDataBlocksEvent(totalNumDataBlocks, numHandles, handles);
    }
}

// 7.7.65 LE Meta event
public sealed class LeMetaEvent : HciEvent
{
    public byte SubeventCode { get; }
    public byte[] Parameters { get; }

    public LeMetaEvent(byte subeventCode, byte[] parameters)
        : base(new(HciEventCodes.LeMeta))
    {
        SubeventCode = subeventCode;
        Parameters = parameters;
    }

    public static LeMetaEvent Parse(ref HciSpanReader r)
    {
        byte subeventCode = r.ReadU8();
        return new LeMetaEvent(subeventCode, r.RemainingSpan.ToArray());
    }
}

// 7.7.66 Triggered Clock Capture event
public sealed class TriggeredClockCaptureEvent : HciEvent
{
    public ushort ConnectionHandle { get; }
    public byte WhichClock { get; }
    public uint Clock { get; }
    public ushort SlotOffset { get; }

    public TriggeredClockCaptureEvent(ushort connectionHandle, byte whichClock, uint clock, ushort slotOffset)
        : base(new(HciEventCodes.TriggeredClockCapture))
    {
        ConnectionHandle = connectionHandle;
        WhichClock = whichClock;
        Clock = clock;
        SlotOffset = slotOffset;
    }

    public static TriggeredClockCaptureEvent Parse(ref HciSpanReader r)
    {
        return new TriggeredClockCaptureEvent(r.ReadU16(), r.ReadU8(), r.ReadU32(), r.ReadU16());
    }
}

// 7.7.67 Synchronization Train Complete event
public sealed class SynchronizationTrainCompleteEvent : HciEvent
{
    public byte Status { get; }

    public SynchronizationTrainCompleteEvent(byte status)
        : base(new(HciEventCodes.SynchronizationTrainComplete))
    {
        Status = status;
    }

    public static SynchronizationTrainCompleteEvent Parse(ref HciSpanReader r)
    {
        return new SynchronizationTrainCompleteEvent(r.ReadU8());
    }
}

// 7.7.68 Synchronization Train Received event
public sealed class SynchronizationTrainReceivedEvent : HciEvent
{
    public byte Status { get; }
    public ulong BdAddr { get; }
    public uint ClockOffset { get; }
    public byte[] AfhChannelMap { get; }
    public byte LtAddr { get; }
    public uint NextBroadcastInstant { get; }
    public ushort ConnectionlessPeripheralBroadcastInterval { get; }
    public byte ServiceData { get; }

    public SynchronizationTrainReceivedEvent(byte status, ulong bdAddr, uint clockOffset, byte[] afhChannelMap, byte ltAddr,
                                            uint nextBroadcastInstant, ushort connectionlessPeripheralBroadcastInterval, byte serviceData)
        : base(new(HciEventCodes.SynchronizationTrainReceived))
    {
        Status = status;
        BdAddr = bdAddr;
        ClockOffset = clockOffset;
        AfhChannelMap = afhChannelMap;
        LtAddr = ltAddr;
        NextBroadcastInstant = nextBroadcastInstant;
        ConnectionlessPeripheralBroadcastInterval = connectionlessPeripheralBroadcastInterval;
        ServiceData = serviceData;
    }

    public static SynchronizationTrainReceivedEvent Parse(ref HciSpanReader r)
    {
        return new SynchronizationTrainReceivedEvent(r.ReadU8(), r.ReadU48(), r.ReadU32(), r.ReadBytes(10).ToArray(), r.ReadU8(),
                                                    r.ReadU32(), r.ReadU16(), r.ReadU8());
    }
}

// 7.7.69 Connectionless Peripheral Broadcast Receive event
public sealed class ConnectionlessPeripheralBroadcastReceiveEvent : HciEvent
{
    public ulong BdAddr { get; }
    public byte LtAddr { get; }
    public uint Clock { get; }
    public uint Offset { get; }
    public byte RxStatus { get; }
    public byte Fragment { get; }
    public byte DataLength { get; }
    public byte[] Data { get; }

    public ConnectionlessPeripheralBroadcastReceiveEvent(ulong bdAddr, byte ltAddr, uint clock, uint offset, byte rxStatus,
                                                        byte fragment, byte dataLength, byte[] data)
        : base(new(HciEventCodes.ConnectionlessPeripheralBroadcastReceive))
    {
        BdAddr = bdAddr;
        LtAddr = ltAddr;
        Clock = clock;
        Offset = offset;
        RxStatus = rxStatus;
        Fragment = fragment;
        DataLength = dataLength;
        Data = data;
    }

    public static ConnectionlessPeripheralBroadcastReceiveEvent Parse(ref HciSpanReader r)
    {
        ulong bdAddr = r.ReadU48();
        byte ltAddr = r.ReadU8();
        uint clock = r.ReadU32();
        uint offset = r.ReadU32();
        byte rxStatus = r.ReadU8();
        byte fragment = r.ReadU8();
        byte dataLength = r.ReadU8();
        return new ConnectionlessPeripheralBroadcastReceiveEvent(bdAddr, ltAddr, clock, offset, rxStatus, fragment,
                                                                dataLength, r.ReadBytes(dataLength).ToArray());
    }
}

// 7.7.70 Connectionless Peripheral Broadcast Timeout event
public sealed class ConnectionlessPeripheralBroadcastTimeoutEvent : HciEvent
{
    public ulong BdAddr { get; }
    public byte LtAddr { get; }

    public ConnectionlessPeripheralBroadcastTimeoutEvent(ulong bdAddr, byte ltAddr)
        : base(new(HciEventCodes.ConnectionlessPeripheralBroadcastTimeout))
    {
        BdAddr = bdAddr;
        LtAddr = ltAddr;
    }

    public static ConnectionlessPeripheralBroadcastTimeoutEvent Parse(ref HciSpanReader r)
    {
        return new ConnectionlessPeripheralBroadcastTimeoutEvent(r.ReadU48(), r.ReadU8());
    }
}

// 7.7.71 Truncated Page Complete event
public sealed class TruncatedPageCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ulong BdAddr { get; }

    public TruncatedPageCompleteEvent(byte status, ulong bdAddr)
        : base(new(HciEventCodes.TruncatedPageComplete))
    {
        Status = status;
        BdAddr = bdAddr;
    }

    public static TruncatedPageCompleteEvent Parse(ref HciSpanReader r)
    {
        return new TruncatedPageCompleteEvent(r.ReadU8(), r.ReadU48());
    }
}

// 7.7.72 Peripheral Page Response Timeout event
public sealed class PeripheralPageResponseTimeoutEvent : HciEvent
{
    public PeripheralPageResponseTimeoutEvent()
        : base(new(HciEventCodes.PeripheralPageResponseTimeout))
    {
    }

    public static PeripheralPageResponseTimeoutEvent Parse(ref HciSpanReader r)
    {
        return new PeripheralPageResponseTimeoutEvent();
    }
}

// 7.7.73 Connectionless Peripheral Broadcast Channel Map Change event
public sealed class ConnectionlessPeripheralBroadcastChannelMapChangeEvent : HciEvent
{
    public byte[] ChannelMap { get; }

    public ConnectionlessPeripheralBroadcastChannelMapChangeEvent(byte[] channelMap)
        : base(new(HciEventCodes.ConnectionlessPeripheralBroadcastChannelMapChange))
    {
        ChannelMap = channelMap;
    }

    public static ConnectionlessPeripheralBroadcastChannelMapChangeEvent Parse(ref HciSpanReader r)
    {
        return new ConnectionlessPeripheralBroadcastChannelMapChangeEvent(r.ReadBytes(10).ToArray());
    }
}

// 7.7.74 Inquiry Response Notification event
public sealed class InquiryResponseNotificationEvent : HciEvent
{
    public uint Lap { get; }
    public sbyte Rssi { get; }

    public InquiryResponseNotificationEvent(uint lap, sbyte rssi)
        : base(new(HciEventCodes.InquiryResponseNotification))
    {
        Lap = lap;
        Rssi = rssi;
    }

    public static InquiryResponseNotificationEvent Parse(ref HciSpanReader r)
    {
        return new InquiryResponseNotificationEvent(r.ReadU24(), r.Read8());
    }
}

// 7.7.75 Authenticated Payload Timeout Expired event
public sealed class AuthenticatedPayloadTimeoutExpiredEvent : HciEvent
{
    public ushort ConnectionHandle { get; }

    public AuthenticatedPayloadTimeoutExpiredEvent(ushort connectionHandle)
        : base(new(HciEventCodes.AuthenticatedPayloadTimeoutExpired))
    {
        ConnectionHandle = connectionHandle;
    }

    public static AuthenticatedPayloadTimeoutExpiredEvent Parse(ref HciSpanReader r)
    {
        return new AuthenticatedPayloadTimeoutExpiredEvent(r.ReadU16());
    }
}

// 7.7.76 SAM Status Change event
public sealed class SamStatusChangeEvent : HciEvent
{
    public ushort ConnectionHandle { get; }
    public byte LocalSamIndex { get; }
    public byte LocalSamTxAvailability { get; }
    public byte LocalSamRxAvailability { get; }
    public byte RemoteSamIndex { get; }
    public byte RemoteSamTxAvailability { get; }
    public byte RemoteSamRxAvailability { get; }

    public SamStatusChangeEvent(ushort connectionHandle, byte localSamIndex, byte localSamTxAvailability, byte localSamRxAvailability,
                                byte remoteSamIndex, byte remoteSamTxAvailability, byte remoteSamRxAvailability)
        : base(new(HciEventCodes.SamStatusChange))
    {
        ConnectionHandle = connectionHandle;
        LocalSamIndex = localSamIndex;
        LocalSamTxAvailability = localSamTxAvailability;
        LocalSamRxAvailability = localSamRxAvailability;
        RemoteSamIndex = remoteSamIndex;
        RemoteSamTxAvailability = remoteSamTxAvailability;
        RemoteSamRxAvailability = remoteSamRxAvailability;
    }

    public static SamStatusChangeEvent Parse(ref HciSpanReader r)
    {
        return new SamStatusChangeEvent(r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU8(), r.ReadU8(), r.ReadU8(), r.ReadU8());
    }
}
