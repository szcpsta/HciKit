// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Commands;

internal static class CommandReturnParametersParser
{
    public static bool TryParse(ushort commandOpcode, ReadOnlySpan<byte> returnParameters,
                                out HciCommandReturnParameters? parsed)
    {
        HciSpanReader r = new HciSpanReader(returnParameters);
        parsed = commandOpcode switch
        {
            HciOpcodes.ReadLocalVersionInformation => ReadLocalVersionInformationReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCommands => ReadLocalSupportedCommandsReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalSupportedFeatures => ReadLocalSupportedFeaturesReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalExtendedFeatures => ReadLocalExtendedFeaturesReturnParameters.Parse(ref r),
            HciOpcodes.ReadBufferSize => ReadBufferSizeReturnParameters.Parse(ref r),
            HciOpcodes.ReadBdAddr => ReadBdAddrReturnParameters.Parse(ref r),
            HciOpcodes.ReadDataBlockSize => ReadDataBlockSizeReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCodecsV2 => ReadLocalSupportedCodecsV2ReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCodecsV1 => ReadLocalSupportedCodecsV1ReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalSimplePairingOptions => ReadLocalSimplePairingOptionsReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCodecCapabilities => ReadLocalSupportedCodecCapabilitiesReturnParameters.Parse(ref r),
            HciOpcodes.ReadLocalSupportedControllerDelay => ReadLocalSupportedControllerDelayReturnParameters.Parse(ref r),
            _ => null
        };

        return parsed is not null;
    }
}
