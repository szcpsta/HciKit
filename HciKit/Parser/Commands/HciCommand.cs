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

// 7.1.45 Enhanced Setup Synchronous Connection command
public sealed class EnhancedSetupSynchronousConnectionCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public uint TransmitBandwidth { get; }
    public uint ReceiveBandwidth { get; }
    public byte[] TransmitCodingFormat { get; }
    public byte[] ReceiveCodingFormat { get; }
    public ushort TransmitCodecFrameSize { get; }
    public ushort ReceiveCodecFrameSize { get; }
    public uint InputBandwidth { get; }
    public uint OutputBandwidth { get; }
    public byte[] InputCodingFormat { get; }
    public byte[] OutputCodingFormat { get; }
    public ushort InputCodedDataSize { get; }
    public ushort OutputCodedDataSize { get; }
    public byte InputPcmDataFormat { get; }
    public byte OutputPcmDataFormat { get; }
    public byte InputPcmSamplePayloadMsbPosition { get; }
    public byte OutputPcmSamplePayloadMsbPosition { get; }
    public byte InputDataPath { get; }
    public byte OutputDataPath { get; }
    public byte InputTransportUnitSize { get; }
    public byte OutputTransportUnitSize { get; }
    public ushort MaxLatency { get; }
    public ushort PacketType { get; }
    public byte RetransmissionEffort { get; }

    public EnhancedSetupSynchronousConnectionCommand(ushort connectionHandle, uint transmitBandwidth, uint receiveBandwidth,
                                                    byte[] transmitCodingFormat, byte[] receiveCodingFormat,
                                                    ushort transmitCodecFrameSize, ushort receiveCodecFrameSize,
                                                    uint inputBandwidth, uint outputBandwidth,
                                                    byte[] inputCodingFormat, byte[] outputCodingFormat,
                                                    ushort inputCodedDataSize, ushort outputCodedDataSize,
                                                    byte inputPcmDataFormat, byte outputPcmDataFormat,
                                                    byte inputPcmSamplePayloadMsbPosition, byte outputPcmSamplePayloadMsbPosition,
                                                    byte inputDataPath, byte outputDataPath,
                                                    byte inputTransportUnitSize, byte outputTransportUnitSize,
                                                    ushort maxLatency, ushort packetType, byte retransmissionEffort)
        : base(new(HciOpcodes.EnhancedSetupSynchronousConnection))
    {
        ConnectionHandle = connectionHandle;
        TransmitBandwidth = transmitBandwidth;
        ReceiveBandwidth = receiveBandwidth;
        TransmitCodingFormat = transmitCodingFormat;
        ReceiveCodingFormat = receiveCodingFormat;
        TransmitCodecFrameSize = transmitCodecFrameSize;
        ReceiveCodecFrameSize = receiveCodecFrameSize;
        InputBandwidth = inputBandwidth;
        OutputBandwidth = outputBandwidth;
        InputCodingFormat = inputCodingFormat;
        OutputCodingFormat = outputCodingFormat;
        InputCodedDataSize = inputCodedDataSize;
        OutputCodedDataSize = outputCodedDataSize;
        InputPcmDataFormat = inputPcmDataFormat;
        OutputPcmDataFormat = outputPcmDataFormat;
        InputPcmSamplePayloadMsbPosition = inputPcmSamplePayloadMsbPosition;
        OutputPcmSamplePayloadMsbPosition = outputPcmSamplePayloadMsbPosition;
        InputDataPath = inputDataPath;
        OutputDataPath = outputDataPath;
        InputTransportUnitSize = inputTransportUnitSize;
        OutputTransportUnitSize = outputTransportUnitSize;
        MaxLatency = maxLatency;
        PacketType = packetType;
        RetransmissionEffort = retransmissionEffort;
    }

    public static EnhancedSetupSynchronousConnectionCommand Parse(ref HciSpanReader r)
    {
        return new EnhancedSetupSynchronousConnectionCommand(r.ReadU16(), r.ReadU32(), r.ReadU32(),
                                                r.ReadBytes(5).ToArray(), r.ReadBytes(5).ToArray(),
                                                r.ReadU16(), r.ReadU16(),
                                                r.ReadU32(), r.ReadU32(),
                                                r.ReadBytes(5).ToArray(), r.ReadBytes(5).ToArray(),
                                                r.ReadU16(), r.ReadU16(),
                                                r.ReadU8(), r.ReadU8(),
                                                r.ReadU8(), r.ReadU8(),
                                                r.ReadU8(), r.ReadU8(),
                                                r.ReadU8(), r.ReadU8(),
                                                r.ReadU16(), r.ReadU16(), r.ReadU8());
    }
}

// 7.1.46 Enhanced Accept Synchronous Connection Request command
public sealed class EnhancedAcceptSynchronousConnectionRequestCommand : HciCommand
{
    public ulong BdAdder { get; }
    public uint TransmitBandwidth { get; }
    public uint ReceiveBandwidth { get; }
    public byte[] TransmitCodingFormat { get; }
    public byte[] ReceiveCodingFormat { get; }
    public ushort TransmitCodecFrameSize { get; }
    public ushort ReceiveCodecFrameSize { get; }
    public uint InputBandwidth { get; }
    public uint OutputBandwidth { get; }
    public byte[] InputCodingFormat { get; }
    public byte[] OutputCodingFormat { get; }
    public ushort InputCodedDataSize { get; }
    public ushort OutputCodedDataSize { get; }
    public byte InputPcmDataFormat { get; }
    public byte OutputPcmDataFormat { get; }
    public byte InputPcmSamplePayloadMsbPosition { get; }
    public byte OutputPcmSamplePayloadMsbPosition { get; }
    public byte InputDataPath { get; }
    public byte OutputDataPath { get; }
    public byte InputTransportUnitSize { get; }
    public byte OutputTransportUnitSize { get; }
    public ushort MaxLatency { get; }
    public ushort PacketType { get; }
    public byte RetransmissionEffort { get; }

    public EnhancedAcceptSynchronousConnectionRequestCommand(ulong bdAdder, uint transmitBandwidth, uint receiveBandwidth,
                                                            byte[] transmitCodingFormat, byte[] receiveCodingFormat,
                                                            ushort transmitCodecFrameSize, ushort receiveCodecFrameSize,
                                                            uint inputBandwidth, uint outputBandwidth,
                                                            byte[] inputCodingFormat, byte[] outputCodingFormat,
                                                            ushort inputCodedDataSize, ushort outputCodedDataSize,
                                                            byte inputPcmDataFormat, byte outputPcmDataFormat,
                                                            byte inputPcmSamplePayloadMsbPosition, byte outputPcmSamplePayloadMsbPosition,
                                                            byte inputDataPath, byte outputDataPath,
                                                            byte inputTransportUnitSize, byte outputTransportUnitSize,
                                                            ushort maxLatency, ushort packetType, byte retransmissionEffort)
        : base(new(HciOpcodes.EnhancedAcceptSynchronousConnectionRequest))
    {
        BdAdder = bdAdder;
        TransmitBandwidth = transmitBandwidth;
        ReceiveBandwidth = receiveBandwidth;
        TransmitCodingFormat = transmitCodingFormat;
        ReceiveCodingFormat = receiveCodingFormat;
        TransmitCodecFrameSize = transmitCodecFrameSize;
        ReceiveCodecFrameSize = receiveCodecFrameSize;
        InputBandwidth = inputBandwidth;
        OutputBandwidth = outputBandwidth;
        InputCodingFormat = inputCodingFormat;
        OutputCodingFormat = outputCodingFormat;
        InputCodedDataSize = inputCodedDataSize;
        OutputCodedDataSize = outputCodedDataSize;
        InputPcmDataFormat = inputPcmDataFormat;
        OutputPcmDataFormat = outputPcmDataFormat;
        InputPcmSamplePayloadMsbPosition = inputPcmSamplePayloadMsbPosition;
        OutputPcmSamplePayloadMsbPosition = outputPcmSamplePayloadMsbPosition;
        InputDataPath = inputDataPath;
        OutputDataPath = outputDataPath;
        InputTransportUnitSize = inputTransportUnitSize;
        OutputTransportUnitSize = outputTransportUnitSize;
        MaxLatency = maxLatency;
        PacketType = packetType;
        RetransmissionEffort = retransmissionEffort;
    }

    public static EnhancedAcceptSynchronousConnectionRequestCommand Parse(ref HciSpanReader r)
    {
        return new EnhancedAcceptSynchronousConnectionRequestCommand(r.ReadU48(), r.ReadU32(), r.ReadU32(),
                                                    r.ReadBytes(5).ToArray(), r.ReadBytes(5).ToArray(),
                                                    r.ReadU16(), r.ReadU16(),
                                                    r.ReadU32(), r.ReadU32(),
                                                    r.ReadBytes(5).ToArray(), r.ReadBytes(5).ToArray(),
                                                    r.ReadU16(), r.ReadU16(),
                                                    r.ReadU8(), r.ReadU8(),
                                                    r.ReadU8(), r.ReadU8(),
                                                    r.ReadU8(), r.ReadU8(),
                                                    r.ReadU8(), r.ReadU8(),
                                                    r.ReadU16(), r.ReadU16(), r.ReadU8());
    }
}

// 7.1.47 Truncated Page command
public sealed class TruncatedPageCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte PageScanRepetitionMode { get; }
    public ushort ClockOffset { get; }

    public TruncatedPageCommand(ulong bdAdder, byte pageScanRepetitionMode, ushort clockOffset)
        : base(new(HciOpcodes.TruncatedPage))
    {
        BdAdder = bdAdder;
        PageScanRepetitionMode = pageScanRepetitionMode;
        ClockOffset = clockOffset;
    }

    public static TruncatedPageCommand Parse(ref HciSpanReader r)
    {
        return new TruncatedPageCommand(r.ReadU48(), r.ReadU8(), r.ReadU16());
    }
}

// 7.1.48 Truncated Page Cancel command
public sealed class TruncatedPageCancelCommand : HciCommand
{
    public ulong BdAdder { get; }

    public TruncatedPageCancelCommand(ulong bdAdder)
        : base(new(HciOpcodes.TruncatedPageCancel))
    {
        BdAdder = bdAdder;
    }

    public static TruncatedPageCancelCommand Parse(ref HciSpanReader r)
    {
        return new TruncatedPageCancelCommand(r.ReadU48());
    }
}

// 7.1.49 Set Connectionless Peripheral Broadcast command
public sealed class SetConnectionlessPeripheralBroadcastCommand : HciCommand
{
    public byte Enable { get; }
    public byte LtAddr { get; }
    public byte LpoAllowed { get; }
    public ushort PacketType { get; }
    public ushort IntervalMin { get; }
    public ushort IntervalMax { get; }
    public ushort SupervisionTimeout { get; }

    public SetConnectionlessPeripheralBroadcastCommand(byte enable, byte ltAddr, byte lpoAllowed, ushort packetType,
                                                        ushort intervalMin, ushort intervalMax,
                                                        ushort supervisionTimeout)
        : base(new(HciOpcodes.SetConnectionlessPeripheralBroadcast))
    {
        Enable = enable;
        LtAddr = ltAddr;
        LpoAllowed = lpoAllowed;
        PacketType = packetType;
        IntervalMin = intervalMin;
        IntervalMax = intervalMax;
        SupervisionTimeout = supervisionTimeout;
    }

    public static SetConnectionlessPeripheralBroadcastCommand Parse(ref HciSpanReader r)
    {
        return new SetConnectionlessPeripheralBroadcastCommand(r.ReadU8(), r.ReadU8(), r.ReadU8(), r.ReadU16(),
                                                            r.ReadU16(), r.ReadU16(),
                                                            r.ReadU16());
    }
}

// 7.1.50 Set Connectionless Peripheral Broadcast Receive command
public sealed class SetConnectionlessPeripheralBroadcastReceiveCommand : HciCommand
{
    public byte Enable { get; }
    public ulong BdAdder { get; }
    public byte LtAddr { get; }
    public ushort Interval { get; }
    public uint ClockOffset { get; }
    public uint NextConnectionlessPeripheralBroadcastClock { get; }
    public ushort SupervisionTimeout { get; }
    public byte RemoteTimingAccuracy { get; }
    public byte Skip { get; }
    public ushort PacketType { get; }
    public byte[] AfhChannelMap { get; }

    public SetConnectionlessPeripheralBroadcastReceiveCommand(byte enable, ulong bdAdder, byte ltAddr, ushort interval,
                                                            uint clockOffset, uint nextConnectionlessPeripheralBroadcastClock,
                                                            ushort supervisionTimeout, byte remoteTimingAccuracy, byte skip,
                                                            ushort packetType, byte[] afhChannelMap)
        : base(new(HciOpcodes.SetConnectionlessPeripheralBroadcastReceive))
    {
        Enable = enable;
        BdAdder = bdAdder;
        LtAddr = ltAddr;
        Interval = interval;
        ClockOffset = clockOffset;
        NextConnectionlessPeripheralBroadcastClock = nextConnectionlessPeripheralBroadcastClock;
        SupervisionTimeout = supervisionTimeout;
        RemoteTimingAccuracy = remoteTimingAccuracy;
        Skip = skip;
        PacketType = packetType;
        AfhChannelMap = afhChannelMap;
    }

    public static SetConnectionlessPeripheralBroadcastReceiveCommand Parse(ref HciSpanReader r)
    {
        return new SetConnectionlessPeripheralBroadcastReceiveCommand(r.ReadU8(), r.ReadU48(), r.ReadU8(), r.ReadU16(),
                                                                    r.ReadU32(), r.ReadU32(),
                                                                    r.ReadU16(), r.ReadU8(), r.ReadU8(),
                                                                    r.ReadU16(), r.ReadBytes(10).ToArray());
    }
}

// 7.1.51 Start Synchronization Train command
public sealed class StartSynchronizationTrainCommand : HciCommand
{
    public StartSynchronizationTrainCommand()
        : base(new(HciOpcodes.StartSynchronizationTrain))
    {
    }

    public static StartSynchronizationTrainCommand Parse(ref HciSpanReader r)
    {
        return new StartSynchronizationTrainCommand();
    }
}

// 7.1.52 Receive Synchronization Train command
public sealed class ReceiveSynchronizationTrainCommand : HciCommand
{
    public ulong BdAdder { get; }
    public ushort SynchronizationScanTimeout { get; }
    public ushort SynchronizationScanWindow { get; }
    public ushort SynchronizationScanInterval { get; }

    public ReceiveSynchronizationTrainCommand(ulong bdAdder, ushort synchronizationScanTimeout,
                                            ushort synchronizationScanWindow, ushort synchronizationScanInterval)
        : base(new(HciOpcodes.ReceiveSynchronizationTrain))
    {
        BdAdder = bdAdder;
        SynchronizationScanTimeout = synchronizationScanTimeout;
        SynchronizationScanWindow = synchronizationScanWindow;
        SynchronizationScanInterval = synchronizationScanInterval;
    }

    public static ReceiveSynchronizationTrainCommand Parse(ref HciSpanReader r)
    {
        return new ReceiveSynchronizationTrainCommand(r.ReadU48(), r.ReadU16(), r.ReadU16(), r.ReadU16());
    }
}

// 7.1.53 Remote OOB Extended Data Request Reply command
public sealed class RemoteOobExtendedDataRequestReplyCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte[] SimplePairingHashC192 { get; }
    public byte[] SimplePairingRandomizerR192 { get; }
    public byte[] SimplePairingHashC256 { get; }
    public byte[] SimplePairingRandomizerR256 { get; }

    public RemoteOobExtendedDataRequestReplyCommand(ulong bdAdder, byte[] simplePairingHashC192, byte[] simplePairingRandomizerR192,
                                                    byte[] simplePairingHashC256, byte[] simplePairingRandomizerR256)
        : base(new(HciOpcodes.RemoteOobExtendedDataRequestReply))
    {
        BdAdder = bdAdder;
        SimplePairingHashC192 = simplePairingHashC192;
        SimplePairingRandomizerR192 = simplePairingRandomizerR192;
        SimplePairingHashC256 = simplePairingHashC256;
        SimplePairingRandomizerR256 = simplePairingRandomizerR256;
    }

    public static RemoteOobExtendedDataRequestReplyCommand Parse(ref HciSpanReader r)
    {
        return new RemoteOobExtendedDataRequestReplyCommand(r.ReadU48(),
                                                            r.ReadBytes(16).ToArray(),
                                                            r.ReadBytes(16).ToArray(),
                                                            r.ReadBytes(16).ToArray(),
                                                            r.ReadBytes(16).ToArray());
    }
}

#endregion 7.1 Link Control commands

#region 7.2 Link Policy commands
// 7.2.1 Hold Mode command
public sealed class HoldModeCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public ushort HoldModeMaxInterval { get; }
    public ushort HoldModeMinInterval { get; }

    public HoldModeCommand(ushort connectionHandle, ushort holdModeMaxInterval, ushort holdModeMinInterval)
        : base(new(HciOpcodes.HoldMode))
    {
        ConnectionHandle = connectionHandle;
        HoldModeMaxInterval = holdModeMaxInterval;
        HoldModeMinInterval = holdModeMinInterval;
    }

    public static HoldModeCommand Parse(ref HciSpanReader r)
    {
        return new HoldModeCommand(r.ReadU16(), r.ReadU16(), r.ReadU16());
    }
}

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

// 7.2.6 QoS Setup command
public sealed class QoSSetupCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public byte Unused { get; }
    public byte ServiceType { get; }
    public uint TokenRate { get; }
    public uint PeakBandwidth { get; }
    public uint Latency { get; }
    public uint DelayVariation { get; }

    public QoSSetupCommand(ushort connectionHandle, byte unused, byte serviceType,
                            uint tokenRate, uint peakBandwidth, uint latency, uint delayVariation)
        : base(new(HciOpcodes.QoSSetup))
    {
        ConnectionHandle = connectionHandle;
        Unused = unused;
        ServiceType = serviceType;
        TokenRate = tokenRate;
        PeakBandwidth = peakBandwidth;
        Latency = latency;
        DelayVariation = delayVariation;
    }

    public static QoSSetupCommand Parse(ref HciSpanReader r)
    {
        return new QoSSetupCommand(r.ReadU16(), r.ReadU8(), r.ReadU8(),
                                    r.ReadU32(), r.ReadU32(), r.ReadU32(), r.ReadU32());
    }
}

// 7.2.7 Role Discovery command
public sealed class RoleDiscoveryCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public RoleDiscoveryCommand(ushort connectionHandle)
        : base(new(HciOpcodes.RoleDiscovery))
    {
        ConnectionHandle = connectionHandle;
    }

    public static RoleDiscoveryCommand Parse(ref HciSpanReader r)
    {
        return new RoleDiscoveryCommand(r.ReadU16());
    }
}

// 7.2.8 Switch Role command
public sealed class SwitchRoleCommand : HciCommand
{
    public ulong BdAdder { get; }
    public byte Role { get; }

    public SwitchRoleCommand(ulong bdAdder, byte role)
        : base(new(HciOpcodes.SwitchRole))
    {
        BdAdder = bdAdder;
        Role = role;
    }

    public static SwitchRoleCommand Parse(ref HciSpanReader r)
    {
        return new SwitchRoleCommand(r.ReadU48(), r.ReadU8());
    }
}

// 7.2.9 Read Link Policy Settings command
public sealed class ReadLinkPolicySettingsCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadLinkPolicySettingsCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadLinkPolicySettings))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadLinkPolicySettingsCommand Parse(ref HciSpanReader r)
    {
        return new ReadLinkPolicySettingsCommand(r.ReadU16());
    }
}

// 7.2.10 Write Link Policy Settings command
public sealed class WriteLinkPolicySettingsCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public ushort LinkPolicySettings { get; }

    public WriteLinkPolicySettingsCommand(ushort connectionHandle, ushort linkPolicySettings)
        : base(new(HciOpcodes.WriteLinkPolicySettings))
    {
        ConnectionHandle = connectionHandle;
        LinkPolicySettings = linkPolicySettings;
    }

    public static WriteLinkPolicySettingsCommand Parse(ref HciSpanReader r)
    {
        return new WriteLinkPolicySettingsCommand(r.ReadU16(), r.ReadU16());
    }
}

// 7.2.11 Read Default Link Policy Settings command
public sealed class ReadDefaultLinkPolicySettingsCommand : HciCommand
{
    public ReadDefaultLinkPolicySettingsCommand()
        : base(new(HciOpcodes.ReadDefaultLinkPolicySettings))
    {
    }

    public static ReadDefaultLinkPolicySettingsCommand Parse(ref HciSpanReader r)
    {
        return new ReadDefaultLinkPolicySettingsCommand();
    }
}

// 7.2.12 Write Default Link Policy Settings command
public sealed class WriteDefaultLinkPolicySettingsCommand : HciCommand
{
    public ushort DefaultLinkPolicySettings { get; }

    public WriteDefaultLinkPolicySettingsCommand(ushort defaultLinkPolicySettings)
        : base(new(HciOpcodes.WriteDefaultLinkPolicySettings))
    {
        DefaultLinkPolicySettings = defaultLinkPolicySettings;
    }

    public static WriteDefaultLinkPolicySettingsCommand Parse(ref HciSpanReader r)
    {
        return new WriteDefaultLinkPolicySettingsCommand(r.ReadU16());
    }
}

// 7.2.13 Flow Specification command
public sealed class FlowSpecificationCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public byte Unused { get; }
    public byte FlowDirection { get; }
    public byte ServiceType { get; }
    public uint TokenRate { get; }
    public uint TokenBucketSize { get; }
    public uint PeakBandwidth { get; }
    public uint AccessLatency { get; }

    public FlowSpecificationCommand(ushort connectionHandle, byte unused, byte flowDirection, byte serviceType,
                                    uint tokenRate, uint tokenBucketSize, uint peakBandwidth, uint accessLatency)
        : base(new(HciOpcodes.FlowSpecification))
    {
        ConnectionHandle = connectionHandle;
        Unused = unused;
        FlowDirection = flowDirection;
        ServiceType = serviceType;
        TokenRate = tokenRate;
        TokenBucketSize = tokenBucketSize;
        PeakBandwidth = peakBandwidth;
        AccessLatency = accessLatency;
    }

    public static FlowSpecificationCommand Parse(ref HciSpanReader r)
    {
        return new FlowSpecificationCommand(r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU8(),
                                            r.ReadU32(), r.ReadU32(), r.ReadU32(), r.ReadU32());
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

#region 7.4 Informational Parameters
// 7.4.1 Read Local Version Information command
public sealed class ReadLocalVersionInformationCommand : HciCommand
{
    public ReadLocalVersionInformationCommand()
        : base(new(HciOpcodes.ReadLocalVersionInformation))
    {
    }

    public static ReadLocalVersionInformationCommand Parse(ref HciSpanReader r)
    {
        return new ReadLocalVersionInformationCommand();
    }
}

// 7.4.2 Read Local Supported Commands command
public sealed class ReadLocalSupportedCommandsCommand : HciCommand
{
    public ReadLocalSupportedCommandsCommand()
        : base(new(HciOpcodes.ReadLocalSupportedCommands))
    {
    }

    public static ReadLocalSupportedCommandsCommand Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedCommandsCommand();
    }
}

// 7.4.3 Read Local Supported Features command
public sealed class ReadLocalSupportedFeaturesCommand : HciCommand
{
    public ReadLocalSupportedFeaturesCommand()
        : base(new(HciOpcodes.ReadLocalSupportedFeatures))
    {
    }

    public static ReadLocalSupportedFeaturesCommand Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedFeaturesCommand();
    }
}

// 7.4.4 Read Local Extended Features command
public sealed class ReadLocalExtendedFeaturesCommand : HciCommand
{
    public byte PageNumber { get; }

    public ReadLocalExtendedFeaturesCommand(byte pageNumber)
        : base(new(HciOpcodes.ReadLocalExtendedFeatures))
    {
        PageNumber = pageNumber;
    }

    public static ReadLocalExtendedFeaturesCommand Parse(ref HciSpanReader r)
    {
        return new ReadLocalExtendedFeaturesCommand(r.ReadU8());
    }
}

// 7.4.5 Read Buffer Size command
public sealed class ReadBufferSizeCommand : HciCommand
{
    public ReadBufferSizeCommand()
        : base(new(HciOpcodes.ReadBufferSize))
    {
    }

    public static ReadBufferSizeCommand Parse(ref HciSpanReader r)
    {
        return new ReadBufferSizeCommand();
    }
}

// 7.4.6 Read BD_ADDR command
public sealed class ReadBdAddrCommand : HciCommand
{
    public ReadBdAddrCommand()
        : base(new(HciOpcodes.ReadBdAddr))
    {
    }

    public static ReadBdAddrCommand Parse(ref HciSpanReader r)
    {
        return new ReadBdAddrCommand();
    }
}

// 7.4.7 Read Data Block Size command
public sealed class ReadDataBlockSizeCommand : HciCommand
{
    public ReadDataBlockSizeCommand()
        : base(new(HciOpcodes.ReadDataBlockSize))
    {
    }

    public static ReadDataBlockSizeCommand Parse(ref HciSpanReader r)
    {
        return new ReadDataBlockSizeCommand();
    }
}

// 7.4.8 Read Local Supported Codecs command [v2]
public sealed class ReadLocalSupportedCodecsV2Command : HciCommand
{
    public ReadLocalSupportedCodecsV2Command()
        : base(new(HciOpcodes.ReadLocalSupportedCodecsV2))
    {
    }

    public static ReadLocalSupportedCodecsV2Command Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedCodecsV2Command();
    }
}

// 7.4.8 Read Local Supported Codecs command [v1]
public sealed class ReadLocalSupportedCodecsV1Command : HciCommand
{
    public ReadLocalSupportedCodecsV1Command()
        : base(new(HciOpcodes.ReadLocalSupportedCodecsV1))
    {
    }

    public static ReadLocalSupportedCodecsV1Command Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedCodecsV1Command();
    }
}

// 7.4.9 Read Local Simple Pairing Options command
public sealed class ReadLocalSimplePairingOptionsCommand : HciCommand
{
    public ReadLocalSimplePairingOptionsCommand()
        : base(new(HciOpcodes.ReadLocalSimplePairingOptions))
    {
    }

    public static ReadLocalSimplePairingOptionsCommand Parse(ref HciSpanReader r)
    {
        return new ReadLocalSimplePairingOptionsCommand();
    }
}

// 7.4.10 Read Local Supported Codec Capabilities command
public sealed class ReadLocalSupportedCodecCapabilitiesCommand : HciCommand
{
    public byte[] CodecId { get; }
    public byte LogicalTransportType { get; }
    public byte Direction { get; }

    public ReadLocalSupportedCodecCapabilitiesCommand(byte[] codecId, byte logicalTransportType, byte direction)
        : base(new(HciOpcodes.ReadLocalSupportedCodecCapabilities))
    {
        CodecId = codecId;
        LogicalTransportType = logicalTransportType;
        Direction = direction;
    }

    public static ReadLocalSupportedCodecCapabilitiesCommand Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedCodecCapabilitiesCommand(r.ReadBytes(5).ToArray(), r.ReadU8(), r.ReadU8());
    }
}

// 7.4.11 Read Local Supported Controller Delay command
public sealed class ReadLocalSupportedControllerDelayCommand : HciCommand
{
    public byte[] CodecId { get; }
    public byte LogicalTransportType { get; }
    public byte Direction { get; }
    public byte CodecConfigurationLength { get; }
    public byte[] CodecConfiguration { get; }

    public ReadLocalSupportedControllerDelayCommand(byte[] codecId, byte logicalTransportType, byte direction,
                                                    byte codecConfigurationLength, byte[] codecConfiguration)
        : base(new(HciOpcodes.ReadLocalSupportedControllerDelay))
    {
        CodecId = codecId;
        LogicalTransportType = logicalTransportType;
        Direction = direction;
        CodecConfigurationLength = codecConfigurationLength;
        CodecConfiguration = codecConfiguration;
    }

    public static ReadLocalSupportedControllerDelayCommand Parse(ref HciSpanReader r)
    {
        byte[] codecId = r.ReadBytes(5).ToArray();
        byte logicalTransportType = r.ReadU8();
        byte direction = r.ReadU8();
        byte codecConfigurationLength = r.ReadU8();
        return new ReadLocalSupportedControllerDelayCommand(codecId, logicalTransportType, direction,
                                                            codecConfigurationLength, r.ReadBytes(codecConfigurationLength).ToArray());
    }
}

#endregion 7.4 Informational Parameters

#region 7.5 Status Parameters
// 7.5.1 Read Failed Contact Counter command
public sealed class ReadFailedContactCounterCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadFailedContactCounterCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadFailedContactCounter))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadFailedContactCounterCommand Parse(ref HciSpanReader r)
    {
        return new ReadFailedContactCounterCommand(r.ReadU16());
    }
}

// 7.5.2 Reset Failed Contact Counter command
public sealed class ResetFailedContactCounterCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ResetFailedContactCounterCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ResetFailedContactCounter))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ResetFailedContactCounterCommand Parse(ref HciSpanReader r)
    {
        return new ResetFailedContactCounterCommand(r.ReadU16());
    }
}

// 7.5.3 Read Link Quality command
public sealed class ReadLinkQualityCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadLinkQualityCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadLinkQuality))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadLinkQualityCommand Parse(ref HciSpanReader r)
    {
        return new ReadLinkQualityCommand(r.ReadU16());
    }
}

// 7.5.4 Read RSSI command
public sealed class ReadRssiCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadRssiCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadRssi))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadRssiCommand Parse(ref HciSpanReader r)
    {
        return new ReadRssiCommand(r.ReadU16());
    }
}

// 7.5.5 Read AFH Channel Map command
public sealed class ReadAfhChannelMapCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadAfhChannelMapCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadAfhChannelMap))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadAfhChannelMapCommand Parse(ref HciSpanReader r)
    {
        return new ReadAfhChannelMapCommand(r.ReadU16());
    }
}

// 7.5.6 Read Clock command
public sealed class ReadClockCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public byte WhichClock { get; }

    public ReadClockCommand(ushort connectionHandle, byte whichClock)
        : base(new(HciOpcodes.ReadClock))
    {
        ConnectionHandle = connectionHandle;
        WhichClock = whichClock;
    }

    public static ReadClockCommand Parse(ref HciSpanReader r)
    {
        return new ReadClockCommand(r.ReadU16(), r.ReadU8());
    }
}

// 7.5.7 Read Encryption Key Size command
public sealed class ReadEncryptionKeySizeCommand : HciCommand
{
    public ushort ConnectionHandle { get; }

    public ReadEncryptionKeySizeCommand(ushort connectionHandle)
        : base(new(HciOpcodes.ReadEncryptionKeySize))
    {
        ConnectionHandle = connectionHandle;
    }

    public static ReadEncryptionKeySizeCommand Parse(ref HciSpanReader r)
    {
        return new ReadEncryptionKeySizeCommand(r.ReadU16());
    }
}

// 7.5.11 Get MWS Transport Layer Configuration command
public sealed class GetMwsTransportLayerConfigurationCommand : HciCommand
{
    public GetMwsTransportLayerConfigurationCommand()
        : base(new(HciOpcodes.GetMwsTransportLayerConfiguration))
    {
    }

    public static GetMwsTransportLayerConfigurationCommand Parse(ref HciSpanReader r)
    {
        return new GetMwsTransportLayerConfigurationCommand();
    }
}

// 7.5.12 Set Triggered Clock Capture command
public sealed class SetTriggeredClockCaptureCommand : HciCommand
{
    public ushort ConnectionHandle { get; }
    public byte Enable { get; }
    public byte WhichClock { get; }
    public byte LpoAllowed { get; }
    public byte NumClockCapturesToFilter { get; }

    public SetTriggeredClockCaptureCommand(ushort connectionHandle, byte enable, byte whichClock,
                                            byte lpoAllowed, byte numClockCapturesToFilter)
        : base(new(HciOpcodes.SetTriggeredClockCapture))
    {
        ConnectionHandle = connectionHandle;
        Enable = enable;
        WhichClock = whichClock;
        LpoAllowed = lpoAllowed;
        NumClockCapturesToFilter = numClockCapturesToFilter;
    }

    public static SetTriggeredClockCaptureCommand Parse(ref HciSpanReader r)
    {
        return new SetTriggeredClockCaptureCommand(r.ReadU16(), r.ReadU8(), r.ReadU8(), r.ReadU8(), r.ReadU8());
    }
}

#endregion 7.5 Status Parameters
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
