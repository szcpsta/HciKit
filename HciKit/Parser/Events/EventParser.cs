// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser.Events.Samsung;

namespace HciKit.Parser.Events;

internal class EventParser
{
    private enum Offset : byte
    {
        EventCode = 0,
        ParameterTotalLength = 1,
        Parameter = 2,
    }

    private readonly IVendorSpecificEventParser _vseParser = new SamsungEventParser();

    public HciPacket Parse(ReadOnlySpan<byte> p)
    {
        if (p.Length != (byte)Offset.Parameter + GetParameterTotalLength(p))
        {
            return new UnknownHciPacket(HciUnknownReason.InvalidLength, HciPacketType.Event);
        }

        HciSpanReader r = new HciSpanReader(p);
        r.Skip((int)Offset.Parameter);

        var eventCode = GetEventCode(p);
        return eventCode switch
        {
            HciEventCodes.InquiryComplete => InquiryCompleteEvent.Parse(ref r),
            HciEventCodes.InquiryResult => InquiryResultEvent.Parse(ref r),
            HciEventCodes.ConnectionComplete => ConnectionCompleteEvent.Parse(ref r),
            HciEventCodes.ConnectionRequest => ConnectionRequestEvent.Parse(ref r),
            HciEventCodes.DisconnectionComplete => DisconnectionCompleteEvent.Parse(ref r),
            HciEventCodes.AuthenticationComplete => AuthenticationCompleteEvent.Parse(ref r),
            HciEventCodes.RemoteNameRequestComplete => RemoteNameRequestCompleteEvent.Parse(ref r),
            HciEventCodes.EncryptionChangeV1 => EncryptionChangeV1Event.Parse(ref r),
            HciEventCodes.ChangeConnectionLinkKeyComplete => ChangeConnectionLinkKeyCompleteEvent.Parse(ref r),
            HciEventCodes.LinkKeyTypeChanged => LinkKeyTypeChangedEvent.Parse(ref r),
            HciEventCodes.ReadRemoteSupportedFeaturesComplete => ReadRemoteSupportedFeaturesCompleteEvent.Parse(ref r),
            HciEventCodes.ReadRemoteVersionInformationComplete => ReadRemoteVersionInformationCompleteEvent.Parse(ref r),
            HciEventCodes.QoSSetupComplete => QoSSetupCompleteEvent.Parse(ref r),
            HciEventCodes.CommandComplete => CommandCompleteEvent.Parse(ref r),
            HciEventCodes.CommandStatus => CommandStatusEvent.Parse(ref r),
            HciEventCodes.HardwareError => HardwareErrorEvent.Parse(ref r),
            HciEventCodes.FlushOccurred => FlushOccurredEvent.Parse(ref r),
            HciEventCodes.RoleChange => RoleChangeEvent.Parse(ref r),
            HciEventCodes.NumberOfCompletedPackets => NumberOfCompletedPacketsEvent.Parse(ref r),
            HciEventCodes.ModeChange => ModeChangeEvent.Parse(ref r),
            HciEventCodes.ReturnLinkKeys => ReturnLinkKeysEvent.Parse(ref r),
            HciEventCodes.PinCodeRequest => PinCodeRequestEvent.Parse(ref r),
            HciEventCodes.LinkKeyRequest => LinkKeyRequestEvent.Parse(ref r),
            HciEventCodes.LinkKeyNotification => LinkKeyNotificationEvent.Parse(ref r),
            HciEventCodes.LoopbackCommand => LoopbackCommandEvent.Parse(ref r),
            HciEventCodes.DataBufferOverflow => DataBufferOverflowEvent.Parse(ref r),
            HciEventCodes.MaxSlotsChange => MaxSlotsChangeEvent.Parse(ref r),
            HciEventCodes.ReadClockOffsetComplete => ReadClockOffsetCompleteEvent.Parse(ref r),
            HciEventCodes.ConnectionPacketTypeChanged => ConnectionPacketTypeChangedEvent.Parse(ref r),
            HciEventCodes.QoSViolation => QoSViolationEvent.Parse(ref r),
            HciEventCodes.PageScanRepetitionModeChange => PageScanRepetitionModeChangeEvent.Parse(ref r),
            HciEventCodes.FlowSpecificationComplete => FlowSpecificationCompleteEvent.Parse(ref r),
            HciEventCodes.InquiryResultWithRssi => InquiryResultWithRssiEvent.Parse(ref r),
            HciEventCodes.ReadRemoteExtendedFeaturesComplete => ReadRemoteExtendedFeaturesCompleteEvent.Parse(ref r),
            HciEventCodes.SynchronousConnectionComplete => SynchronousConnectionCompleteEvent.Parse(ref r),
            HciEventCodes.SynchronousConnectionChanged => SynchronousConnectionChangedEvent.Parse(ref r),
            HciEventCodes.SniffSubrating => SniffSubratingEvent.Parse(ref r),
            HciEventCodes.ExtendedInquiryResult => ExtendedInquiryResultEvent.Parse(ref r),
            HciEventCodes.EncryptionKeyRefreshComplete => EncryptionKeyRefreshCompleteEvent.Parse(ref r),
            HciEventCodes.IoCapabilityRequest => IoCapabilityRequestEvent.Parse(ref r),
            HciEventCodes.IoCapabilityResponse => IoCapabilityResponseEvent.Parse(ref r),
            HciEventCodes.UserConfirmationRequest => UserConfirmationRequestEvent.Parse(ref r),
            HciEventCodes.UserPasskeyRequest => UserPasskeyRequestEvent.Parse(ref r),
            HciEventCodes.RemoteOobDataRequest => RemoteOobDataRequestEvent.Parse(ref r),
            HciEventCodes.SimplePairingComplete => SimplePairingCompleteEvent.Parse(ref r),
            HciEventCodes.LinkSupervisionTimeoutChanged => LinkSupervisionTimeoutChangedEvent.Parse(ref r),
            HciEventCodes.EnhancedFlushComplete => EnhancedFlushCompleteEvent.Parse(ref r),
            HciEventCodes.UserPasskeyNotification => UserPasskeyNotificationEvent.Parse(ref r),
            HciEventCodes.KeypressNotification => KeypressNotificationEvent.Parse(ref r),
            HciEventCodes.RemoteHostSupportedFeaturesNotification => RemoteHostSupportedFeaturesNotificationEvent.Parse(ref r),
            HciEventCodes.LeMeta => LeMetaEvent.Parse(ref r),
            HciEventCodes.NumberOfCompletedDataBlocks => NumberOfCompletedDataBlocksEvent.Parse(ref r),
            HciEventCodes.TriggeredClockCapture => TriggeredClockCaptureEvent.Parse(ref r),
            HciEventCodes.SynchronizationTrainComplete => SynchronizationTrainCompleteEvent.Parse(ref r),
            HciEventCodes.SynchronizationTrainReceived => SynchronizationTrainReceivedEvent.Parse(ref r),
            HciEventCodes.ConnectionlessPeripheralBroadcastReceive => ConnectionlessPeripheralBroadcastReceiveEvent.Parse(ref r),
            HciEventCodes.ConnectionlessPeripheralBroadcastTimeout => ConnectionlessPeripheralBroadcastTimeoutEvent.Parse(ref r),
            HciEventCodes.TruncatedPageComplete => TruncatedPageCompleteEvent.Parse(ref r),
            HciEventCodes.PeripheralPageResponseTimeout => PeripheralPageResponseTimeoutEvent.Parse(ref r),
            HciEventCodes.ConnectionlessPeripheralBroadcastChannelMapChange => ConnectionlessPeripheralBroadcastChannelMapChangeEvent.Parse(ref r),
            HciEventCodes.InquiryResponseNotification => InquiryResponseNotificationEvent.Parse(ref r),
            HciEventCodes.AuthenticatedPayloadTimeoutExpired => AuthenticatedPayloadTimeoutExpiredEvent.Parse(ref r),
            HciEventCodes.SamStatusChange => SamStatusChangeEvent.Parse(ref r),
            HciEventCodes.EncryptionChangeV2 => EncryptionChangeV2Event.Parse(ref r),

            HciEventCodes.VendorSpecific => _vseParser.Parse(ref r),

            _ => new UnknownHciPacket(HciUnknownReason.UnsupportedEventCode, HciPacketType.Event, eventCode)
        };
    }

    private static byte GetEventCode(ReadOnlySpan<byte> p)
        => p[(byte)Offset.EventCode];

    private static byte GetParameterTotalLength(ReadOnlySpan<byte> p)
        => p[(byte)Offset.ParameterTotalLength];

    private static ReadOnlySpan<byte> GetParameter(ReadOnlySpan<byte> p)
        => p.Slice((byte)Offset.Parameter);
}
