// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace HciKit.Parser.Events;

public abstract class HciCommandReturnParameters
{
}

public sealed class UnknownCommandReturnParameters : HciCommandReturnParameters
{
    public byte[] RawReturnParameters { get; }

    public UnknownCommandReturnParameters(byte[] rawReturnParameters)
    {
        RawReturnParameters = rawReturnParameters;
    }
}

public readonly struct VendorSpecificCodecId
{
    public ushort CompanyIdentifier { get; }
    public ushort CodecId { get; }

    public VendorSpecificCodecId(ushort companyIdentifier, ushort codecId)
    {
        CompanyIdentifier = companyIdentifier;
        CodecId = codecId;
    }
}

// 7.4.1 Read Local Version Information command return parameters
public sealed class ReadLocalVersionInformationReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public byte HciVersion { get; }
    public ushort HciSubversion { get; }
    public byte LmpVersion { get; }
    public ushort CompanyIdentifier { get; }
    public ushort LmpSubversion { get; }

    public ReadLocalVersionInformationReturnParameters(byte status, byte hciVersion, ushort hciSubversion, byte lmpVersion,
                                                       ushort companyIdentifier, ushort lmpSubversion)
    {
        Status = status;
        HciVersion = hciVersion;
        HciSubversion = hciSubversion;
        LmpVersion = lmpVersion;
        CompanyIdentifier = companyIdentifier;
        LmpSubversion = lmpSubversion;
    }

    public static ReadLocalVersionInformationReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadLocalVersionInformationReturnParameters(
            r.ReadU8(),
            r.ReadU8(),
            r.ReadU16(),
            r.ReadU8(),
            r.ReadU16(),
            r.ReadU16());
    }
}

// 7.4.2 Read Local Supported Commands command return parameters
public sealed class ReadLocalSupportedCommandsReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public byte[] SupportedCommands { get; }

    public ReadLocalSupportedCommandsReturnParameters(byte status, byte[] supportedCommands)
    {
        Status = status;
        SupportedCommands = supportedCommands;
    }

    public static ReadLocalSupportedCommandsReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedCommandsReturnParameters(r.ReadU8(), r.ReadBytes(64).ToArray());
    }
}

// 7.4.3 Read Local Supported Features command return parameters
public sealed class ReadLocalSupportedFeaturesReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public ulong LmpFeatures { get; }

    public ReadLocalSupportedFeaturesReturnParameters(byte status, ulong lmpFeatures)
    {
        Status = status;
        LmpFeatures = lmpFeatures;
    }

    public static ReadLocalSupportedFeaturesReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedFeaturesReturnParameters(r.ReadU8(), r.ReadU64());
    }
}

// 7.4.4 Read Local Extended Features command return parameters
public sealed class ReadLocalExtendedFeaturesReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public byte PageNumber { get; }
    public byte MaxPageNumber { get; }
    public ulong ExtendedLmpFeatures { get; }

    public ReadLocalExtendedFeaturesReturnParameters(byte status, byte pageNumber, byte maxPageNumber, ulong extendedLmpFeatures)
    {
        Status = status;
        PageNumber = pageNumber;
        MaxPageNumber = maxPageNumber;
        ExtendedLmpFeatures = extendedLmpFeatures;
    }

    public static ReadLocalExtendedFeaturesReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadLocalExtendedFeaturesReturnParameters(r.ReadU8(), r.ReadU8(), r.ReadU8(), r.ReadU64());
    }
}

// 7.4.5 Read Buffer Size command return parameters
public sealed class ReadBufferSizeReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public ushort AclDataPacketLength { get; }
    public byte SynchronousDataPacketLength { get; }
    public ushort TotalNumAclDataPackets { get; }
    public ushort TotalNumSynchronousDataPackets { get; }

    public ReadBufferSizeReturnParameters(byte status, ushort aclDataPacketLength, byte synchronousDataPacketLength,
                                          ushort totalNumAclDataPackets, ushort totalNumSynchronousDataPackets)
    {
        Status = status;
        AclDataPacketLength = aclDataPacketLength;
        SynchronousDataPacketLength = synchronousDataPacketLength;
        TotalNumAclDataPackets = totalNumAclDataPackets;
        TotalNumSynchronousDataPackets = totalNumSynchronousDataPackets;
    }

    public static ReadBufferSizeReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadBufferSizeReturnParameters(r.ReadU8(), r.ReadU16(), r.ReadU8(), r.ReadU16(), r.ReadU16());
    }
}

// 7.4.6 Read BD_ADDR command return parameters
public sealed class ReadBdAddrReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public ulong BdAddr { get; }

    public ReadBdAddrReturnParameters(byte status, ulong bdAddr)
    {
        Status = status;
        BdAddr = bdAddr;
    }

    public static ReadBdAddrReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadBdAddrReturnParameters(r.ReadU8(), r.ReadU48());
    }
}

// 7.4.7 Read Data Block Size command return parameters
public sealed class ReadDataBlockSizeReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public ushort MaxAclDataPacketLength { get; }
    public ushort DataBlockLength { get; }
    public ushort TotalNumDataBlocks { get; }

    public ReadDataBlockSizeReturnParameters(byte status, ushort maxAclDataPacketLength, ushort dataBlockLength,
                                             ushort totalNumDataBlocks)
    {
        Status = status;
        MaxAclDataPacketLength = maxAclDataPacketLength;
        DataBlockLength = dataBlockLength;
        TotalNumDataBlocks = totalNumDataBlocks;
    }

    public static ReadDataBlockSizeReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadDataBlockSizeReturnParameters(r.ReadU8(), r.ReadU16(), r.ReadU16(), r.ReadU16());
    }
}

// 7.4.8 Read Local Supported Codecs command [v2] return parameters
public sealed class ReadLocalSupportedCodecsV2ReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public byte NumSupportedStandardCodecs { get; }
    public byte[] StandardCodecIds { get; }
    public byte[] StandardCodecTransports { get; }
    public byte NumSupportedVendorSpecificCodecs { get; }
    public VendorSpecificCodecId[] VendorSpecificCodecIds { get; }
    public byte[] VendorSpecificCodecTransports { get; }

    public ReadLocalSupportedCodecsV2ReturnParameters(byte status, byte numSupportedStandardCodecs, byte[] standardCodecIds,
                                                      byte[] standardCodecTransports, byte numSupportedVendorSpecificCodecs,
                                                      VendorSpecificCodecId[] vendorSpecificCodecIds, byte[] vendorSpecificCodecTransports)
    {
        Status = status;
        NumSupportedStandardCodecs = numSupportedStandardCodecs;
        StandardCodecIds = standardCodecIds;
        StandardCodecTransports = standardCodecTransports;
        NumSupportedVendorSpecificCodecs = numSupportedVendorSpecificCodecs;
        VendorSpecificCodecIds = vendorSpecificCodecIds;
        VendorSpecificCodecTransports = vendorSpecificCodecTransports;
    }

    public static ReadLocalSupportedCodecsV2ReturnParameters Parse(ref HciSpanReader r)
    {
        byte status = r.ReadU8();
        byte numStandard = r.ReadU8();
        byte[] standardIds = r.ReadBytes(numStandard).ToArray();
        byte[] standardTransports = r.ReadBytes(numStandard).ToArray();
        byte numVendor = r.ReadU8();
        VendorSpecificCodecId[] vendorIds = new VendorSpecificCodecId[numVendor];
        for (int i = 0; i < numVendor; i++)
        {
            vendorIds[i] = new VendorSpecificCodecId(r.ReadU16(), r.ReadU16());
        }

        byte[] vendorTransports = r.ReadBytes(numVendor).ToArray();
        return new ReadLocalSupportedCodecsV2ReturnParameters(status, numStandard, standardIds, standardTransports, numVendor,
                                                              vendorIds, vendorTransports);
    }
}

// 7.4.8 Read Local Supported Codecs command [v1] return parameters
public sealed class ReadLocalSupportedCodecsV1ReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public byte NumSupportedStandardCodecs { get; }
    public byte[] StandardCodecIds { get; }
    public byte NumSupportedVendorSpecificCodecs { get; }
    public VendorSpecificCodecId[] VendorSpecificCodecIds { get; }

    public ReadLocalSupportedCodecsV1ReturnParameters(byte status, byte numSupportedStandardCodecs, byte[] standardCodecIds,
                                                      byte numSupportedVendorSpecificCodecs, VendorSpecificCodecId[] vendorSpecificCodecIds)
    {
        Status = status;
        NumSupportedStandardCodecs = numSupportedStandardCodecs;
        StandardCodecIds = standardCodecIds;
        NumSupportedVendorSpecificCodecs = numSupportedVendorSpecificCodecs;
        VendorSpecificCodecIds = vendorSpecificCodecIds;
    }

    public static ReadLocalSupportedCodecsV1ReturnParameters Parse(ref HciSpanReader r)
    {
        byte status = r.ReadU8();
        byte numStandard = r.ReadU8();
        byte[] standardIds = r.ReadBytes(numStandard).ToArray();
        byte numVendor = r.ReadU8();
        VendorSpecificCodecId[] vendorIds = new VendorSpecificCodecId[numVendor];
        for (int i = 0; i < numVendor; i++)
        {
            vendorIds[i] = new VendorSpecificCodecId(r.ReadU16(), r.ReadU16());
        }

        return new ReadLocalSupportedCodecsV1ReturnParameters(status, numStandard, standardIds, numVendor, vendorIds);
    }
}

// 7.4.9 Read Local Simple Pairing Options command return parameters
public sealed class ReadLocalSimplePairingOptionsReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public byte SimplePairingOptions { get; }
    public byte MaxEncryptionKeySize { get; }

    public ReadLocalSimplePairingOptionsReturnParameters(byte status, byte simplePairingOptions, byte maxEncryptionKeySize)
    {
        Status = status;
        SimplePairingOptions = simplePairingOptions;
        MaxEncryptionKeySize = maxEncryptionKeySize;
    }

    public static ReadLocalSimplePairingOptionsReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadLocalSimplePairingOptionsReturnParameters(r.ReadU8(), r.ReadU8(), r.ReadU8());
    }
}

// 7.4.10 Read Local Supported Codec Capabilities command return parameters
public sealed class ReadLocalSupportedCodecCapabilitiesReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public byte NumCodecCapabilities { get; }
    public byte[] CodecCapabilityLengths { get; }
    public IReadOnlyList<byte[]> CodecCapabilities { get; }

    public ReadLocalSupportedCodecCapabilitiesReturnParameters(byte status, byte numCodecCapabilities, byte[] codecCapabilityLengths,
                                                               byte[][] codecCapabilities)
    {
        Status = status;
        NumCodecCapabilities = numCodecCapabilities;
        CodecCapabilityLengths = codecCapabilityLengths;
        CodecCapabilities = codecCapabilities;
    }

    public static ReadLocalSupportedCodecCapabilitiesReturnParameters Parse(ref HciSpanReader r)
    {
        byte status = r.ReadU8();
        byte numCodecCapabilities = r.ReadU8();
        byte[] lengths = r.ReadBytes(numCodecCapabilities).ToArray();
        byte[][] capabilities = new byte[numCodecCapabilities][];
        for (int i = 0; i < numCodecCapabilities; i++)
        {
            capabilities[i] = r.ReadBytes(lengths[i]).ToArray();
        }

        return new ReadLocalSupportedCodecCapabilitiesReturnParameters(status, numCodecCapabilities, lengths, capabilities);
    }
}

// 7.4.11 Read Local Supported Controller Delay command return parameters
public sealed class ReadLocalSupportedControllerDelayReturnParameters : HciCommandReturnParameters
{
    public byte Status { get; }
    public uint MinControllerDelay { get; }
    public uint MaxControllerDelay { get; }

    public ReadLocalSupportedControllerDelayReturnParameters(byte status, uint minControllerDelay, uint maxControllerDelay)
    {
        Status = status;
        MinControllerDelay = minControllerDelay;
        MaxControllerDelay = maxControllerDelay;
    }

    public static ReadLocalSupportedControllerDelayReturnParameters Parse(ref HciSpanReader r)
    {
        return new ReadLocalSupportedControllerDelayReturnParameters(r.ReadU8(), r.ReadU24(), r.ReadU24());
    }
}
