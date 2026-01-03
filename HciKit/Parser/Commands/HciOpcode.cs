// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Commands;

public readonly record struct HciOpcode(ushort Value)
{
    public byte Ogf => (byte)((Value >> 10) & 0x3F);
    public ushort Ocf => (ushort)(Value & 0x03FF);
    public bool IsVendorSpecific => Ogf == 0x3F;

    public override string ToString() => $"0x{Value:X4} (OGF={Ogf}, OCF={Ocf})";
}

public static class HciOpcodes
{
    #region 7.1 Link Control commands
    public const ushort Inquiry = 0x0401;
    public const ushort InquiryCancel = 0x0402;
    public const ushort PeriodicInquiryMode = 0x0403;
    public const ushort ExitPeriodicInquiryMode = 0x0404;
    public const ushort CreateConnection = 0x0405;
    public const ushort Disconnect = 0x0406;
    public const ushort CreateConnectionCancel = 0x0408;
    public const ushort AcceptConnectionRequest = 0x0409;
    public const ushort RejectConnectionRequest = 0x040A;
    public const ushort LinkKeyRequestReply = 0x040B;
    public const ushort LinkKeyRequestNegativeReply = 0x040C;
    public const ushort PinCodeRequestReply = 0x040D;
    public const ushort PinCodeRequestNegativeReply = 0x040E;
    public const ushort ChangeConnectionPacketType = 0x040F;
    public const ushort AuthenticationRequested = 0x0411;
    public const ushort SetConnectionEncryption = 0x0413;
    public const ushort ChangeConnectionLinkKey = 0x0415;
    public const ushort LinkKeySelection = 0x0417;
    public const ushort RemoteNameRequest = 0x0419;
    public const ushort RemoteNameRequestCancel = 0x041A;
    public const ushort ReadRemoteSupportedFeatures = 0x041B;
    public const ushort ReadRemoteExtendedFeatures = 0x041C;
    public const ushort ReadRemoteVersionInformation = 0x041D;
    public const ushort ReadClockOffset = 0x041F;
    public const ushort ReadLmpHandle = 0x0420;
    public const ushort SetupSynchronousConnection = 0x0428;
    public const ushort AcceptSynchronousConnectionRequest = 0x0429;
    public const ushort RejectSynchronousConnectionRequest = 0x042A;
    public const ushort IoCapabilityRequestReply = 0x042B;
    public const ushort UserConfirmationRequestReply = 0x042C;
    public const ushort UserConfirmationRequestNegativeReply = 0x042D;
    public const ushort UserPasskeyRequestReply = 0x042E;
    public const ushort UserPasskeyRequestNegativeReply = 0x042F;
    public const ushort RemoteOobDataRequestReply = 0x0430;
    public const ushort RemoteOobDataRequestNegativeReply = 0x0433;
    public const ushort IoCapabilityRequestNegativeReply = 0x0434;
    public const ushort EnhancedSetupSynchronousConnection = 0x043D;
    public const ushort EnhancedAcceptSynchronousConnectionRequest = 0x043E;
    public const ushort TruncatedPage = 0x043F;
    public const ushort TruncatedPageCancel = 0x0440;
    public const ushort SetConnectionlessPeripheralBroadcast = 0x0441;
    public const ushort SetConnectionlessPeripheralBroadcastReceive = 0x0442;
    public const ushort StartSynchronizationTrain = 0x0443;
    public const ushort ReceiveSynchronizationTrain = 0x0444;
    public const ushort RemoteOobExtendedDataRequestReply = 0x0445;
    #endregion 7.1 Link Control commands

    #region 7.2 Link Policy commands
    public const ushort SniffMode = 0x0803;
    public const ushort ExitSniffMode = 0x0804;
    public const ushort SniffSubrating = 0x0811;
    #endregion 7.2 Link Policy commands

    #region 7.3 Controller & Baseband commands
    public const ushort WriteScanEnable = 0x0C1A;
    #endregion 7.3 Controller & Baseband commands

    #region 7.8 LE Controller commands
    public const ushort LeSetExtendedAdvertisingParametersV1 = 0x2036;
    public const ushort LeSetExtendedAdvertisingEnable = 0x2039;
    public const ushort LeSetExtendedScanParameters = 0x2041;
    public const ushort LeSetExtendedScanEnable = 0x2042;
    public const ushort LeSetExtendedAdvertisingParametersV2 = 0x207F;
    #endregion 7.8 LE Controller commands
}
