// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;

namespace HciKit.Parser.Events;

internal class EventParser
{
    private enum Offset : byte
    {
        EventCode = 0,
        ParameterTotalLength = 1,
        Parameter = 2,
    }

    public static HciPacket Parse(ReadOnlySpan<byte> p)
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

            HciEventCodes.EncryptionChangeV2 => EncryptionChangeV2Event.Parse(ref r),

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
