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
    public ushort MaxPeriodLength { get; }
    public ushort MinPeriodLength { get; }
    public uint Lap { get; }
    public byte InquiryLength { get; }
    public byte NumResponses { get; }

    public PeriodicInquiryModeCommand(ushort maxPeriodLength, ushort minPeriodLength, uint lap, byte inquiryLength, byte numResponses)
        : base(new(HciOpcodes.PeriodicInquiryMode))
    {
        MaxPeriodLength = maxPeriodLength;
        MinPeriodLength = minPeriodLength;
        Lap = lap;
        InquiryLength = inquiryLength;
        NumResponses = numResponses;
    }

    public static PeriodicInquiryModeCommand Parse(ref HciSpanReader r)
    {
        return new PeriodicInquiryModeCommand(r.ReadU16(), r.ReadU16(), r.ReadU24(), r.ReadU8(), r.ReadU8());
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

// 7.1.6 Disconnect command
public sealed class DisconnectCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public byte Reason { get; }

    public DisconnectCommand(ushort connectionHandle, byte reason)
        : base(new(HciOpcodes.Disconnect))
    {
        ConnectionHandle = connectionHandle;
        Reason = reason;
    }

    public static DisconnectCommand Parse(ref HciSpanReader r)
    {
        return new DisconnectCommand(r.ReadU16(), r.ReadU8());
    }
}

// 7.1.7 Create Connection Cancel command
public sealed class CreateConnectionCancelCommand : HciCommand
{
    public ulong BdAdder { get; }

    public CreateConnectionCancelCommand(ulong bdAdder)
        : base(new(HciOpcodes.CreateConnectionCancel))
    {
        BdAdder = bdAdder;
    }

    public static CreateConnectionCancelCommand Parse(ref HciSpanReader r)
    {
        return new CreateConnectionCancelCommand(r.ReadU48());
    }
}

// 7.1.8 Accept Connection Request command
public sealed class AcceptConnectionRequestCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte Role { get; }

    public AcceptConnectionRequestCommand(ulong bdAdder, byte role)
        : base(new(HciOpcodes.AcceptConnectionRequest))
    {
        BdAdder = bdAdder;
        Role = role;
    }

    public static AcceptConnectionRequestCommand Parse(ref HciSpanReader r)
    {
        return new AcceptConnectionRequestCommand(r.ReadU48(), r.ReadU8());
    }
}

// 7.1.9 Reject Connection Request command
public sealed class RejectConnectionRequestCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte Reason { get; }

    public RejectConnectionRequestCommand(ulong bdAdder, byte reason)
        : base(new(HciOpcodes.RejectConnectionRequest))
    {
        BdAdder = bdAdder;
        Reason = reason;
    }

    public static RejectConnectionRequestCommand Parse(ref HciSpanReader r)
    {
        return new RejectConnectionRequestCommand(r.ReadU48(), r.ReadU8());
    }
}

// 7.1.10 Link Key Request Reply command
public sealed class LinkKeyRequestReplyCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte[] LinkKey { get; }

    public LinkKeyRequestReplyCommand(ulong bdAdder, byte[] linkKey)
        : base(new(HciOpcodes.LinkKeyRequestReply))
    {
        BdAdder = bdAdder;
        LinkKey = linkKey;
    }

    public static LinkKeyRequestReplyCommand Parse(ref HciSpanReader r)
    {
        return new LinkKeyRequestReplyCommand(r.ReadU48(), r.ReadBytes(16).ToArray());
    }
}

// 7.1.11 Link Key Request Negative Reply command
public sealed class LinkKeyRequestNegativeReplyCommand : HciCommand
{
    public ulong BdAdder { get; }

    public LinkKeyRequestNegativeReplyCommand(ulong bdAdder)
        : base(new(HciOpcodes.LinkKeyRequestNegativeReply))
    {
        BdAdder = bdAdder;
    }

    public static LinkKeyRequestNegativeReplyCommand Parse(ref HciSpanReader r)
    {
        return new LinkKeyRequestNegativeReplyCommand(r.ReadU48());
    }
}

// 7.1.12 PIN Code Request Reply command
public sealed class PinCodeRequestReplyCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte PinCodeLength { get; }
    public byte[] PinCode { get; }

    public PinCodeRequestReplyCommand(ulong bdAdder, byte pinCodeLength, byte[] pinCode)
        : base(new(HciOpcodes.PinCodeRequestReply))
    {
        BdAdder = bdAdder;
        PinCodeLength = pinCodeLength;
        PinCode = pinCode;
    }

    public static PinCodeRequestReplyCommand Parse(ref HciSpanReader r)
    {
        return new PinCodeRequestReplyCommand(r.ReadU48(), r.ReadU8(), r.ReadBytes(16).ToArray());
    }
}

// 7.1.13 PIN Code Request Negative Reply command
public sealed class PinCodeRequestNegativeReplyCommand : HciCommand
{
    public ulong BdAdder { get; }

    public PinCodeRequestNegativeReplyCommand(ulong bdAdder)
        : base(new(HciOpcodes.PinCodeRequestNegativeReply))
    {
        BdAdder = bdAdder;
    }

    public static PinCodeRequestNegativeReplyCommand Parse(ref HciSpanReader r)
    {
        return new PinCodeRequestNegativeReplyCommand(r.ReadU48());
    }
}

// 7.1.14 Change Connection Packet Type command
public sealed class ChangeConnectionPacketTypeCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public ushort PacketType { get; }

    public ChangeConnectionPacketTypeCommand(ushort connectionHandle, ushort packetType)
        : base(new(HciOpcodes.ChangeConnectionPacketType))
    {
        ConnectionHandle = connectionHandle;
        PacketType = packetType;
    }

    public static ChangeConnectionPacketTypeCommand Parse(ref HciSpanReader r)
    {
        return new ChangeConnectionPacketTypeCommand(r.ReadU16(), r.ReadU16());
    }
}

// 7.1.15 Authentication Requested command
public sealed class AuthenticationRequestedCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public AuthenticationRequestedCommand(ushort connectionHandle)
        : base(new(HciOpcodes.AuthenticationRequested))
    {
        ConnectionHandle = connectionHandle;
    }

    public static AuthenticationRequestedCommand Parse(ref HciSpanReader r)
    {
        return new AuthenticationRequestedCommand(r.ReadU16());
    }
}

// 7.1.16 Set Connection Encryption command
public sealed class SetConnectionEncryptionCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public byte EncryptionEnable { get; }

    public SetConnectionEncryptionCommand(ushort connectionHandle, byte encryptionEnable)
        : base(new(HciOpcodes.SetConnectionEncryption))
    {
        ConnectionHandle = connectionHandle;
        EncryptionEnable = encryptionEnable;
    }

    public static SetConnectionEncryptionCommand Parse(ref HciSpanReader r)
    {
        return new SetConnectionEncryptionCommand(r.ReadU16(), r.ReadU8());
    }
}

// 7.1.17 Change Connection Link Key command
public sealed class ChangeConnectionLinkKeyCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ChangeConnectionLinkKeyCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ChangeConnectionLinkKey))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ChangeConnectionLinkKeyCommand Parse(ref HciSpanReader r)
    {
        return new ChangeConnectionLinkKeyCommand(r.ReadU16());
    }
}

// 7.1.18 Link Key Selection command
public sealed class LinkKeySelectionCommand : HciCommand
{
    public byte KeyFlag { get; }

    public LinkKeySelectionCommand(byte keyFlag)
        : base(new(HciOpcodes.LinkKeySelection))
    {
        KeyFlag = keyFlag;
    }

    public static LinkKeySelectionCommand Parse(ref HciSpanReader r)
    {
        return new LinkKeySelectionCommand(r.ReadU8());
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

// 7.1.20 Remote Name Request Cancel command
public sealed class RemoteNameRequestCancelCommand : HciCommand
{
    public ulong BdAdder { get; }

    public RemoteNameRequestCancelCommand(ulong bdAdder)
        : base(new(HciOpcodes.RemoteNameRequestCancel))
    {
        BdAdder = bdAdder;
    }

    public static RemoteNameRequestCancelCommand Parse(ref HciSpanReader r)
    {
        return new RemoteNameRequestCancelCommand(r.ReadU48());
    }
}

// 7.1.21 Read Remote Supported Features command
public sealed class ReadRemoteSupportedFeaturesCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadRemoteSupportedFeaturesCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadRemoteSupportedFeatures))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadRemoteSupportedFeaturesCommand Parse(ref HciSpanReader r)
    {
        return new ReadRemoteSupportedFeaturesCommand(r.ReadU16());
    }
}

// 7.1.22 Read Remote Extended Features command
public sealed class ReadRemoteExtendedFeaturesCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public byte PageNumber { get; }

    public ReadRemoteExtendedFeaturesCommand(ushort connectionHandle, byte pageNumber)
        : base(new(HciOpcodes.ReadRemoteExtendedFeatures))
    {
        ConnectionHandle = connectionHandle;
        PageNumber = pageNumber;
    }

    public static ReadRemoteExtendedFeaturesCommand Parse(ref HciSpanReader r)
    {
        return new ReadRemoteExtendedFeaturesCommand(r.ReadU16(), r.ReadU8());
    }
}

// 7.1.23 Read Remote Version Information command
public sealed class ReadRemoteVersionInformationCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadRemoteVersionInformationCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadRemoteVersionInformation))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadRemoteVersionInformationCommand Parse(ref HciSpanReader r)
    {
        return new ReadRemoteVersionInformationCommand(r.ReadU16());
    }
}

// 7.1.24 Read Clock Offset command
public sealed class ReadClockOffsetCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadClockOffsetCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadClockOffset))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadClockOffsetCommand Parse(ref HciSpanReader r)
    {
        return new ReadClockOffsetCommand(r.ReadU16());
    }
}

// 7.1.25 Read LMP Handle command
public sealed class ReadLmpHandleCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadLmpHandleCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadLmpHandle))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadLmpHandleCommand Parse(ref HciSpanReader r)
    {
        return new ReadLmpHandleCommand(r.ReadU16());
    }
}

// 7.1.26 Setup Synchronous Connection command
public sealed class SetupSynchronousConnectionCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public uint TransmitBandwidth { get; }
    public uint ReceiveBandwidth { get; }
    public ushort MaxLatency { get; }
    public ushort VoiceSetting { get; }
    public byte RetransmissionEffort { get; }
    public ushort PacketType { get; }

    public SetupSynchronousConnectionCommand(ushort connectionHandle, uint transmitBandwidth, uint receiveBandwidth,
                                            ushort maxLatency, ushort voiceSetting, byte retransmissionEffort, ushort packetType)
        : base(new(HciOpcodes.SetupSynchronousConnection))
    {
        ConnectionHandle = connectionHandle;
        TransmitBandwidth = transmitBandwidth;
        ReceiveBandwidth = receiveBandwidth;
        MaxLatency = maxLatency;
        VoiceSetting = voiceSetting;
        RetransmissionEffort = retransmissionEffort;
        PacketType = packetType;
    }

    public static SetupSynchronousConnectionCommand Parse(ref HciSpanReader r)
    {
        return new SetupSynchronousConnectionCommand(r.ReadU16(), r.ReadU32(), r.ReadU32(), r.ReadU16(), r.ReadU16(), r.ReadU8(), r.ReadU16());
    }
}

// 7.1.27 Accept Synchronous Connection Request command
public sealed class AcceptSynchronousConnectionRequestCommand : HciCommand
{
    public ulong BdAdder { get; }
    public uint TransmitBandwidth { get; }
    public uint ReceiveBandwidth { get; }
    public ushort MaxLatency { get; }
    public ushort VoiceSetting { get; }
    public byte RetransmissionEffort { get; }
    public ushort PacketType { get; }

    public AcceptSynchronousConnectionRequestCommand(ulong bdAdder, uint transmitBandwidth, uint receiveBandwidth,
                                                    ushort maxLatency, ushort voiceSetting, byte retransmissionEffort, ushort packetType)
        : base(new(HciOpcodes.AcceptSynchronousConnectionRequest))
    {
        BdAdder = bdAdder;
        TransmitBandwidth = transmitBandwidth;
        ReceiveBandwidth = receiveBandwidth;
        MaxLatency = maxLatency;
        VoiceSetting = voiceSetting;
        RetransmissionEffort = retransmissionEffort;
        PacketType = packetType;
    }

    public static AcceptSynchronousConnectionRequestCommand Parse(ref HciSpanReader r)
    {
        return new AcceptSynchronousConnectionRequestCommand(r.ReadU48(), r.ReadU32(), r.ReadU32(), r.ReadU16(), r.ReadU16(), r.ReadU8(), r.ReadU16());
    }
}

// 7.1.28 Reject Synchronous Connection Request command
public sealed class RejectSynchronousConnectionRequestCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte Reason { get; }

    public RejectSynchronousConnectionRequestCommand(ulong bdAdder, byte reason)
        : base(new(HciOpcodes.RejectSynchronousConnectionRequest))
    {
        BdAdder = bdAdder;
        Reason = reason;
    }

    public static RejectSynchronousConnectionRequestCommand Parse(ref HciSpanReader r)
    {
        return new RejectSynchronousConnectionRequestCommand(r.ReadU48(), r.ReadU8());
    }
}

// 7.1.29 IO Capability Request Reply command
public sealed class IoCapabilityRequestReplyCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte IoCapability { get; }
    public byte OobDataPresent { get; }
    public byte AuthenticationRequirements { get; }

    public IoCapabilityRequestReplyCommand(ulong bdAdder, byte ioCapability, byte oobDataPresent, byte authenticationRequirements)
        : base(new(HciOpcodes.IoCapabilityRequestReply))
    {
        BdAdder = bdAdder;
        IoCapability = ioCapability;
        OobDataPresent = oobDataPresent;
        AuthenticationRequirements = authenticationRequirements;
    }

    public static IoCapabilityRequestReplyCommand Parse(ref HciSpanReader r)
    {
        return new IoCapabilityRequestReplyCommand(r.ReadU48(), r.ReadU8(), r.ReadU8(), r.ReadU8());
    }
}

// 7.1.30 User Confirmation Request Reply command
public sealed class UserConfirmationRequestReplyCommand : HciCommand
{
    public ulong BdAdder { get; }

    public UserConfirmationRequestReplyCommand(ulong bdAdder)
        : base(new(HciOpcodes.UserConfirmationRequestReply))
    {
        BdAdder = bdAdder;
    }

    public static UserConfirmationRequestReplyCommand Parse(ref HciSpanReader r)
    {
        return new UserConfirmationRequestReplyCommand(r.ReadU48());
    }
}

// 7.1.31 User Confirmation Request Negative Reply command
public sealed class UserConfirmationRequestNegativeReplyCommand : HciCommand
{
    public ulong BdAdder { get; }

    public UserConfirmationRequestNegativeReplyCommand(ulong bdAdder)
        : base(new(HciOpcodes.UserConfirmationRequestNegativeReply))
    {
        BdAdder = bdAdder;
    }

    public static UserConfirmationRequestNegativeReplyCommand Parse(ref HciSpanReader r)
    {
        return new UserConfirmationRequestNegativeReplyCommand(r.ReadU48());
    }
}

// 7.1.32 User Passkey Request Reply command
public sealed class UserPasskeyRequestReplyCommand : HciCommand
{
    public ulong BdAdder { get; }
    public uint NumericValue { get; }

    public UserPasskeyRequestReplyCommand(ulong bdAdder, uint numericValue)
        : base(new(HciOpcodes.UserPasskeyRequestReply))
    {
        BdAdder = bdAdder;
        NumericValue = numericValue;
    }

    public static UserPasskeyRequestReplyCommand Parse(ref HciSpanReader r)
    {
        return new UserPasskeyRequestReplyCommand(r.ReadU48(), r.ReadU32());
    }
}

// 7.1.33 User Passkey Request Negative Reply command
public sealed class UserPasskeyRequestNegativeReplyCommand : HciCommand
{
    public ulong BdAdder { get; }

    public UserPasskeyRequestNegativeReplyCommand(ulong bdAdder)
        : base(new(HciOpcodes.UserPasskeyRequestNegativeReply))
    {
        BdAdder = bdAdder;
    }

    public static UserPasskeyRequestNegativeReplyCommand Parse(ref HciSpanReader r)
    {
        return new UserPasskeyRequestNegativeReplyCommand(r.ReadU48());
    }
}

// 7.1.34 Remote OOB Data Request Reply command
public sealed class RemoteOobDataRequestReplyCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte[] SimplePairingHashC { get; }
    public byte[] SimplePairingRandomizerR { get; }

    public RemoteOobDataRequestReplyCommand(ulong bdAdder, byte[] simplePairingHashC, byte[] simplePairingRandomizerR)
        : base(new(HciOpcodes.RemoteOobDataRequestReply))
    {
        BdAdder = bdAdder;
        SimplePairingHashC = simplePairingHashC;
        SimplePairingRandomizerR = simplePairingRandomizerR;
    }

    public static RemoteOobDataRequestReplyCommand Parse(ref HciSpanReader r)
    {
        return new RemoteOobDataRequestReplyCommand(r.ReadU48(), r.ReadBytes(16).ToArray(), r.ReadBytes(16).ToArray());
    }
}

// 7.1.35 Remote OOB Data Request Negative Reply command
public sealed class RemoteOobDataRequestNegativeReplyCommand : HciCommand
{
    public ulong BdAdder { get; }

    public RemoteOobDataRequestNegativeReplyCommand(ulong bdAdder)
        : base(new(HciOpcodes.RemoteOobDataRequestNegativeReply))
    {
        BdAdder = bdAdder;
    }

    public static RemoteOobDataRequestNegativeReplyCommand Parse(ref HciSpanReader r)
    {
        return new RemoteOobDataRequestNegativeReplyCommand(r.ReadU48());
    }
}

// 7.1.36 IO Capability Request Negative Reply command
public sealed class IoCapabilityRequestNegativeReplyCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte Reason { get; }

    public IoCapabilityRequestNegativeReplyCommand(ulong bdAdder, byte reason)
        : base(new(HciOpcodes.IoCapabilityRequestNegativeReply))
    {
        BdAdder = bdAdder;
        Reason = reason;
    }

    public static IoCapabilityRequestNegativeReplyCommand Parse(ref HciSpanReader r)
    {
        return new IoCapabilityRequestNegativeReplyCommand(r.ReadU48(), r.ReadU8());
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
