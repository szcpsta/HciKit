// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public readonly record struct HciEventCode(byte Value)
{
    public bool IsVendorSpecific => Value == HciEventCodes.VendorSpecific;

    public override string ToString() => $"0x{Value:X4}";
}

internal static class HciEventCodes
{
    public const byte InquiryComplete = 0x01;
    public const byte InquiryResult = 0x02;
    public const byte ConnectionComplete = 0x03;
    public const byte ConnectionRequest = 0x04;
    public const byte DisconnectionComplete = 0x05;
    public const byte AuthenticationComplete = 0x06;
    public const byte RemoteNameRequestComplete = 0x07;
    public const byte EncryptionChangeV1 = 0x08;
    public const byte ChangeConnectionLinkKeyComplete = 0x09;
    public const byte LinkKeyTypeChanged = 0x0A;
    public const byte ReadRemoteSupportedFeaturesComplete = 0x0B;
    public const byte ReadRemoteVersionInformationComplete = 0x0C;
    public const byte QoSSetupComplete = 0x0D;
    public const byte CommandComplete = 0x0E;
    public const byte CommandStatus = 0x0F;
    public const byte HardwareError = 0x10;
    public const byte FlushOccurred = 0x11;
    public const byte RoleChange = 0x12;
    public const byte NumberOfCompletedPackets = 0x13;
    public const byte ModeChange = 0x14;
    public const byte ReturnLinkKeys = 0x15;
    public const byte PinCodeRequest = 0x16;
    public const byte LinkKeyRequest = 0x17;
    public const byte LinkKeyNotification = 0x18;
    public const byte LoopbackCommand = 0x19;
    public const byte DataBufferOverflow = 0x1A;
    public const byte MaxSlotsChange = 0x1B;
    public const byte ReadClockOffsetComplete = 0x1C;
    public const byte ConnectionPacketTypeChanged = 0x1D;
    public const byte QoSViolation = 0x1E;
    public const byte PageScanRepetitionModeChange = 0x20;
    public const byte FlowSpecificationComplete = 0x21;
    public const byte InquiryResultWithRssi = 0x22;
    public const byte ReadRemoteExtendedFeaturesComplete = 0x23;
    public const byte SynchronousConnectionComplete = 0x2C;
    public const byte SynchronousConnectionChanged = 0x2D;
    public const byte SniffSubrating = 0x2E;
    public const byte ExtendedInquiryResult = 0x2F;
    public const byte EncryptionKeyRefreshComplete = 0x30;
    public const byte IoCapabilityRequest = 0x31;
    public const byte IoCapabilityResponse = 0x32;
    public const byte UserConfirmationRequest = 0x33;
    public const byte UserPasskeyRequest = 0x34;
    public const byte RemoteOobDataRequest = 0x35;
    public const byte SimplePairingComplete = 0x36;
    public const byte LinkSupervisionTimeoutChanged = 0x38;
    public const byte EnhancedFlushComplete = 0x39;
    public const byte UserPasskeyNotification = 0x3B;
    public const byte KeypressNotification = 0x3C;
    public const byte RemoteHostSupportedFeaturesNotification = 0x3D;
    public const byte LeMeta = 0x3E;
    public const byte NumberOfCompletedDataBlocks = 0x48;
    public const byte TriggeredClockCapture = 0x4E;
    public const byte SynchronizationTrainComplete = 0x4F;
    public const byte SynchronizationTrainReceived = 0x50;
    public const byte ConnectionlessPeripheralBroadcastReceive = 0x51;
    public const byte ConnectionlessPeripheralBroadcastTimeout = 0x52;
    public const byte TruncatedPageComplete = 0x53;
    public const byte PeripheralPageResponseTimeout = 0x54;
    public const byte ConnectionlessPeripheralBroadcastChannelMapChange = 0x55;
    public const byte InquiryResponseNotification = 0x56;
    public const byte AuthenticatedPayloadTimeoutExpired = 0x57;
    public const byte SamStatusChange = 0x58;
    public const byte EncryptionChangeV2 = 0x59;

    public const byte VendorSpecific = 0xFF;
}
