// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Commands;

public abstract class HciCommand : HciPacket
{
    public HciOpcode Opcode { get; }

    protected HciCommand(HciOpcode opcode)
    {
        Opcode = opcode;
    }

    public override string Name => "Command";
}

#region 7.1 Link Control commands
// 7.1.1 Inquiry command
public sealed class InquiryCommand : HciCommand
{
    public uint Lap { get; }
    public byte InquiryLength { get; }
    public byte NumResponses { get; }

    public InquiryCommand(uint lap, byte inquiryLength, byte numResponses)
        : base(new(HciOpcodes.Inquiry))
    {
        Lap = lap;
        InquiryLength = inquiryLength;
        NumResponses = numResponses;
    }

    public static InquiryCommand Parse(ref HciSpanReader r)
    {
        return new InquiryCommand(r.ReadU24(), r.ReadU8(), r.ReadU8());
    }
}

// 7.1.2 Inquiry Cancel command
public sealed class InquiryCancelCommand : HciCommand
{
    public InquiryCancelCommand()
        : base(new(HciOpcodes.InquiryCancel))
    {
    }

    public static InquiryCancelCommand Parse(ref HciSpanReader r)
    {
        return new InquiryCancelCommand();
    }
}

// 7.1.3 Periodic Inquiry Mode command
public sealed class PeriodicInquiryModeCommand : HciCommand
{
    public uint Lap { get; }
    public ushort MaxPeriodLength { get; }
    public ushort MinPeriodLength { get; }
    public byte InquiryLength { get; }
    public byte NumResponses { get; }

    public PeriodicInquiryModeCommand(uint lap, ushort maxPeriodLength, ushort minPeriodLength, byte inquiryLength, byte numResponses)
        : base(new(HciOpcodes.PeriodicInquiryMode))
    {
        Lap = lap;
        MaxPeriodLength = maxPeriodLength;
        MinPeriodLength = minPeriodLength;
        InquiryLength = inquiryLength;
        NumResponses = numResponses;
    }

    public static PeriodicInquiryModeCommand Parse(ref HciSpanReader r)
    {
        return new PeriodicInquiryModeCommand(r.ReadU24(), r.ReadU16(), r.ReadU16(), r.ReadU8(), r.ReadU8());
    }
}

// 7.1.4 Exit Periodic Inquiry Mode command
public sealed class ExitPeriodicInquiryModeCommand : HciCommand
{
    public ExitPeriodicInquiryModeCommand()
        : base(new(HciOpcodes.ExitPeriodicInquiryMode))
    {
    }

    public static ExitPeriodicInquiryModeCommand Parse(ref HciSpanReader r)
    {
        return new ExitPeriodicInquiryModeCommand();
    }
}

// 7.1.5 Create Connection command
public sealed class CreateConnectionCommand : HciCommand
{
    public ulong BdAdder { get; }
    public ushort PacketType { get; }
    public byte PageScanRepetitionMode { get; }
    public byte Reserved { get; }
    public ushort ClockOffset { get; }
    public byte AllowRoleSwitch { get; }

    public CreateConnectionCommand(ulong bdAdder, ushort paketType, byte pageScanRepetitionMode, byte reserved, ushort clockOffset, byte allowRoleSwitch)
        : base(new(HciOpcodes.CreateConnection))
    {
        BdAdder = bdAdder;
        PacketType = paketType;
        PageScanRepetitionMode = pageScanRepetitionMode;
        Reserved = reserved;
        ClockOffset = clockOffset;
        AllowRoleSwitch = allowRoleSwitch;
    }

    public static CreateConnectionCommand Parse(ref HciSpanReader r)
    {
        return new CreateConnectionCommand(r.ReadU48(), r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU16(), r.ReadU8());
    }
}

// 7.1.19 Remote Name Request command
public sealed class RemoteNameRequestCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte PageScanRepetitionMode { get; }
    public byte Reserved { get; }
    public ushort ClockOffset { get; }

    public RemoteNameRequestCommand(ulong bdAdder, byte pageScanRepetitionMode, byte reserved, ushort clockOffset)
        : base(new(HciOpcodes.RemoteNameRequest))
    {
        BdAdder = bdAdder;
        PageScanRepetitionMode = pageScanRepetitionMode;
        Reserved = reserved;
        ClockOffset = clockOffset;
    }

    public static RemoteNameRequestCommand Parse(ref HciSpanReader r)
    {
        return new RemoteNameRequestCommand(r.ReadU48(), r.ReadU8(), r.ReadU8(), r.ReadU16());
    }
}

#endregion 7.1 Link Control commands

#region 7.2 Link Policy commands
// 7.2.2 Sniff Mode command
public sealed class SniffModeCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public ushort SniffMaxInterval { get; }
    public ushort SniffMinInterval { get; }
    public ushort SniffAttempt { get; }
    public ushort SniffTimeout { get; }

    public SniffModeCommand(ushort connectionHandle, ushort sniffMaxInterval, ushort sniffMinInterval, ushort sniffAttempt, ushort sniffTimeout)
        : base(new(HciOpcodes.SniffMode))
    {
        ConnectionHandle = connectionHandle;
        SniffMaxInterval = sniffMaxInterval;
        SniffMinInterval = sniffMinInterval;
        SniffAttempt = sniffAttempt;
        SniffTimeout = sniffTimeout;
    }

    public static SniffModeCommand Parse(ref HciSpanReader r)
    {
        return new SniffModeCommand(r.ReadU16(), r.ReadU16(), r.ReadU16(), r.ReadU16(), r.ReadU16());
    }
}

// 7.2.3 Exit Sniff Mode command
public sealed class ExitSniffModeCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ExitSniffModeCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ExitSniffMode))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ExitSniffModeCommand Parse(ref HciSpanReader r)
    {
        return new ExitSniffModeCommand(r.ReadU16());
    }
}

// 7.2.14 Sniff Subrating command
public sealed class SniffSubratingCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public ushort MaxLatency { get; }
    public ushort MinRemoteTimeout { get; }
    public ushort MinLocalTimeout { get; }

    public SniffSubratingCommand(ushort connectionHandle, ushort maxLatency, ushort minRemoteTimeout, ushort minLocalTimeout)
        : base(new(HciOpcodes.SniffSubrating))
    {
        ConnectionHandle = connectionHandle;
        MaxLatency = maxLatency;
        MinRemoteTimeout = minRemoteTimeout;
        MinLocalTimeout = minLocalTimeout;
    }

    public static SniffSubratingCommand Parse(ref HciSpanReader r)
    {
        return new SniffSubratingCommand(r.ReadU16(), r.ReadU16(), r.ReadU16(), r.ReadU16());
    }
}

#endregion 7.2 Link Policy commands

#region 7.3 Controller & Baseband commands
// 7.3.18 Write Scan Enable command
public sealed class WriteScanEnableCommand : HciCommand
{
    public byte ScanEnable { get; }

    public WriteScanEnableCommand(byte scanEnable)
        : base(new(HciOpcodes.WriteScanEnable))
    {
        ScanEnable = scanEnable;
    }

    public static WriteScanEnableCommand Parse(ref HciSpanReader r)
    {
        return new WriteScanEnableCommand(r.ReadU8());
    }
}

#endregion 7.3 Controller & Baseband commands

#region 7.8 LE Controller commands
// 7.8.53 LE Set Extended Advertising Parameters command
public sealed class LeSetExtendedAdvertisingParametersV1Command : HciCommand
{
    public byte AdvertisingHandle { get; }
    public ushort AdvertisingEventProperties { get; }
    public uint PrimaryAdvertisingIntervalMin { get; }
    public uint PrimaryAdvertisingIntervalMax { get; }
    public byte PrimaryAdvertisingChannelMap { get; }
    public byte OwnAddressType { get; }
    public byte PeerAddressType { get; }
    public ulong PeerAddress { get; }
    public byte AdvertisingFilterPolicy { get; }
    public sbyte AdvertisingTxPower { get; }
    public byte PrimaryAdvertisingPhy { get; }
    public byte SecondaryAdvertisingMaxSkip { get; }
    public byte SecondaryAdvertisingPhy { get; }
    public byte AdvertisingSid { get; }
    public byte ScanRequestNotificationEnable { get; }

    public LeSetExtendedAdvertisingParametersV1Command(byte advertisingHandle, ushort advertisingEventProperties,
                                                        uint primaryAdvertisingIntervalMin,
                                                        uint primaryAdvertisingIntervalMax,
                                                        byte primaryAdvertisingChannelMap,
                                                        byte ownAddressType, byte peerAddressType, ulong peerAddress,
                                                        byte advertisingFilterPolicy, sbyte advertisingTxPower,
                                                        byte primaryAdvertisingPhy, byte secondaryAdvertisingMaxSkip,
                                                        byte secondaryAdvertisingPhy, byte advertisingSid,
                                                        byte scanRequestNotificationEnable)
        : base(new(HciOpcodes.LeSetExtendedAdvertisingParametersV1))
    {
        AdvertisingHandle = advertisingHandle;
        AdvertisingEventProperties = advertisingEventProperties;
        PrimaryAdvertisingIntervalMin = primaryAdvertisingIntervalMin;
        PrimaryAdvertisingIntervalMax = primaryAdvertisingIntervalMax;
        PrimaryAdvertisingChannelMap = primaryAdvertisingChannelMap;
        OwnAddressType = ownAddressType;
        PeerAddressType = peerAddressType;
        PeerAddress = peerAddress;
        AdvertisingFilterPolicy = advertisingFilterPolicy;
        AdvertisingTxPower = advertisingTxPower;
        PrimaryAdvertisingPhy = primaryAdvertisingPhy;
        SecondaryAdvertisingMaxSkip = secondaryAdvertisingMaxSkip;
        SecondaryAdvertisingPhy = secondaryAdvertisingPhy;
        AdvertisingSid = advertisingSid;
        ScanRequestNotificationEnable = scanRequestNotificationEnable;
    }

    public static LeSetExtendedAdvertisingParametersV1Command Parse(ref HciSpanReader r)
    {
        return new LeSetExtendedAdvertisingParametersV1Command(r.ReadU8(), r.ReadU16(),
                                        r.ReadU24(),
                                        r.ReadU24(),
                                        r.ReadU8(),
                                        r.ReadU8(), r.ReadU8(), r.ReadU48(),
                                        r.ReadU8(), r.Read8(),
                                        r.ReadU8(), r.ReadU8(),
                                        r.ReadU8(), r.ReadU8(),
                                        r.ReadU8());
    }
}

public sealed class LeSetExtendedAdvertisingParametersV2Command : HciCommand
{
    public byte AdvertisingHandle { get; }
    public ushort AdvertisingEventProperties { get; }
    public uint PrimaryAdvertisingIntervalMin { get; }
    public uint PrimaryAdvertisingIntervalMax { get; }
    public byte PrimaryAdvertisingChannelMap { get; }
    public byte OwnAddressType { get; }
    public byte PeerAddressType { get; }
    public ulong PeerAddress { get; }
    public byte AdvertisingFilterPolicy { get; }
    public sbyte AdvertisingTxPower { get; }
    public byte PrimaryAdvertisingPhy { get; }
    public byte SecondaryAdvertisingMaxSkip { get; }
    public byte SecondaryAdvertisingPhy { get; }
    public byte AdvertisingSid { get; }
    public byte ScanRequestNotificationEnable { get; }
    public byte PrimaryAdvertisingPhyOptions { get; }
    public byte SecondaryAdvertisingPhyOptions { get; }

    public LeSetExtendedAdvertisingParametersV2Command(byte advertisingHandle, ushort advertisingEventProperties,
                                                        uint primaryAdvertisingIntervalMin,
                                                        uint primaryAdvertisingIntervalMax,
                                                        byte primaryAdvertisingChannelMap,
                                                        byte ownAddressType, byte peerAddressType, ulong peerAddress,
                                                        byte advertisingFilterPolicy, sbyte advertisingTxPower,
                                                        byte primaryAdvertisingPhy, byte secondaryAdvertisingMaxSkip,
                                                        byte secondaryAdvertisingPhy, byte advertisingSid,
                                                        byte scanRequestNotificationEnable,
                                                        byte primaryAdvertisingPhyOptions,
                                                        byte secondaryAdvertisingPhyOptions)
        : base(new(HciOpcodes.LeSetExtendedAdvertisingParametersV2))
    {
        AdvertisingHandle = advertisingHandle;
        AdvertisingEventProperties = advertisingEventProperties;
        PrimaryAdvertisingIntervalMin = primaryAdvertisingIntervalMin;
        PrimaryAdvertisingIntervalMax = primaryAdvertisingIntervalMax;
        PrimaryAdvertisingChannelMap = primaryAdvertisingChannelMap;
        OwnAddressType = ownAddressType;
        PeerAddressType = peerAddressType;
        PeerAddress = peerAddress;
        AdvertisingFilterPolicy = advertisingFilterPolicy;
        AdvertisingTxPower = advertisingTxPower;
        PrimaryAdvertisingPhy = primaryAdvertisingPhy;
        SecondaryAdvertisingMaxSkip = secondaryAdvertisingMaxSkip;
        SecondaryAdvertisingPhy = secondaryAdvertisingPhy;
        AdvertisingSid = advertisingSid;
        ScanRequestNotificationEnable = scanRequestNotificationEnable;
        PrimaryAdvertisingPhyOptions = primaryAdvertisingPhyOptions;
        SecondaryAdvertisingPhyOptions = secondaryAdvertisingPhyOptions;
    }

    public static LeSetExtendedAdvertisingParametersV2Command Parse(ref HciSpanReader r)
    {
        return new LeSetExtendedAdvertisingParametersV2Command(r.ReadU8(), r.ReadU16(),
                                        r.ReadU24(),
                                        r.ReadU24(),
                                        r.ReadU8(),
                                        r.ReadU8(), r.ReadU8(), r.ReadU48(),
                                        r.ReadU8(), r.Read8(),
                                        r.ReadU8(), r.ReadU8(),
                                        r.ReadU8(), r.ReadU8(),
                                        r.ReadU8(),
                                        r.ReadU8(),
                                        r.ReadU8());
    }
}

// 7.8.56 LE Set Extended Advertising Enable command
public sealed class LeSetExtendedAdvertisingEnableCommand : HciCommand
{
    public byte Enable { get; }
    public byte NumSets { get; }
    public IReadOnlyList<AdvertisingSet> Sets { get; }

    public readonly struct AdvertisingSet
    {
        public byte AdvertisingHandle { get; }
        public ushort Duration { get; }
        public byte MaxExtendedAdvertisingEvents { get; }

        public AdvertisingSet(byte advertisingHandle, ushort duration, byte maxExtendedAdvertisingEvents)
        {
            AdvertisingHandle = advertisingHandle;
            Duration = duration;
            MaxExtendedAdvertisingEvents = maxExtendedAdvertisingEvents;
        }
    }

    public LeSetExtendedAdvertisingEnableCommand(byte enable, byte numSets, AdvertisingSet[] sets)
        : base(new(HciOpcodes.LeSetExtendedAdvertisingEnable))
    {
        Enable = enable;
        NumSets = numSets;
        Sets = sets;
    }

    public static LeSetExtendedAdvertisingEnableCommand Parse(ref HciSpanReader r)
    {
        byte enable = r.ReadU8();
        byte numSets = r.ReadU8();

        AdvertisingSet[] sets = new AdvertisingSet[numSets];
        for (int i = 0; i < numSets; i++)
        {
            sets[i] = new AdvertisingSet(r.ReadU8(), r.ReadU16(), r.ReadU8());
        }

        return new LeSetExtendedAdvertisingEnableCommand(enable, numSets, sets);
    }
}

// 7.8.64 LE Set Extended Scan Parameters command
public sealed class LeSetExtendedScanParametersCommand : HciCommand
{
    public byte OwnAddressType { get; }
    public byte ScanningFilterPolicy { get; }
    public byte ScanningPhys { get; }
    public IReadOnlyList<ScanParameter> ScanParameters { get; }

    public readonly struct ScanParameter
    {
        public byte ScanType { get; }
        public ushort ScanInterval { get; }
        public ushort ScanWindow { get; }

        public ScanParameter(byte scanType, ushort scanInterval, ushort scanWindow)
        {
            ScanType = scanType;
            ScanInterval = scanInterval;
            ScanWindow = scanWindow;
        }
    }

    public LeSetExtendedScanParametersCommand(byte ownAddressType, byte scanningFilterPolicy, byte scanningPhys, ScanParameter[] scanParameters)
        : base(new(HciOpcodes.LeSetExtendedScanParameters))
    {
        OwnAddressType = ownAddressType;
        ScanningFilterPolicy = scanningFilterPolicy;
        ScanningPhys = scanningPhys;
        ScanParameters = scanParameters;
    }

    public static LeSetExtendedScanParametersCommand Parse(ref HciSpanReader r)
    {
        byte ownAddressType = r.ReadU8();
        byte scanningFilterPolicy = r.ReadU8();
        byte scanningPhys = r.ReadU8();

        int numPhys = (scanningPhys & 0x01) + ((scanningPhys >> 2) & 0x01);
        ScanParameter[] scanParameters = new ScanParameter[numPhys];
        for (int i = 0; i < numPhys; i++)
        {
            scanParameters[i] = new ScanParameter(r.ReadU8(), r.ReadU16(), r.ReadU16());
        }

        return new LeSetExtendedScanParametersCommand(ownAddressType, scanningFilterPolicy, scanningPhys, scanParameters);
    }
}

// 7.8.65 LE Set Extended Scan Enable command
public sealed class LeSetExtendedScanEnableCommand : HciCommand
{
    public byte Enable { get; }
    public byte FilterDuplicates { get; }
    public ushort Duration { get; }
    public ushort Period { get; }

    public LeSetExtendedScanEnableCommand(byte enable, byte filterDuplicates, ushort duration, ushort period)
        : base(new(HciOpcodes.LeSetExtendedScanEnable))
    {
        Enable = enable;
        FilterDuplicates = filterDuplicates;
        Duration = duration;
        Period = period;
    }

    public static LeSetExtendedScanEnableCommand Parse(ref HciSpanReader r)
    {
        return new LeSetExtendedScanEnableCommand(r.ReadU8(), r.ReadU8(), r.ReadU16(), r.ReadU16());
    }
}

#endregion 7.8 LE Controller commands
