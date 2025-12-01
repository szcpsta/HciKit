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
    public static readonly HciOpcode Inquiry = new(0x0401);
    public static readonly HciOpcode CreateConnection = new(0x0405);
    public static readonly HciOpcode RemoteNameRequest = new(0x0419);
    #endregion 7.1 Link Control commands

    #region 7.2 Link Policy commands
    public static readonly HciOpcode SniffMode = new(0x0803);
    public static readonly HciOpcode ExitSniffMode = new(0x0804);
    public static readonly HciOpcode SniffSubrating = new(0x0811);
    #endregion 7.2 Link Policy commands

    #region 7.3 Controller & Baseband commands
    public static readonly HciOpcode WriteScanEnable = new(0x0C1A);
    #endregion 7.3 Controller & Baseband commands

    #region 7.8 LE Controller commands
    public static readonly HciOpcode LeSetExtendedAdvertisingParametersV1 = new(0x2036);
    public static readonly HciOpcode LeSetExtendedAdvertisingEnable = new(0x2039);
    public static readonly HciOpcode LeSetExtendedScanParameters = new(0x2041);
    public static readonly HciOpcode LeSetExtendedScanEnable = new(0x2042);
    public static readonly HciOpcode LeSetExtendedAdvertisingParametersV2 = new(0x207F);
    #endregion 7.8 LE Controller commands
}
