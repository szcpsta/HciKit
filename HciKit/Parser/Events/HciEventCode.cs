// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public readonly record struct HciEventCode(byte Value)
{
    public bool IsVendorSpecific => Value == 0xFF;

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

    public const byte EncryptionChangeV2 = 0x59;
}
