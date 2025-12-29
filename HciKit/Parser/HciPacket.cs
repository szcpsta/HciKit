// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser;

public abstract class HciPacket
{
    public abstract string Name { get; }

    public override string ToString() => Name;
}

public sealed class UnknownHciPacket : HciPacket
{
    public HciUnknownReason Reason { get; }
    public HciPacketType PacketType { get; }
    public ushort? Opcode { get; }
    public byte? EventCode { get; }

    public UnknownHciPacket(
        HciUnknownReason reason,
        HciPacketType packetType,
        ushort? opcode = null,
        byte? eventCode = null)
    {
        Reason = reason;
        PacketType = packetType;
        Opcode = opcode;
        EventCode = eventCode;
    }

    public override string Name => "Unknown";
}

public enum HciUnknownReason
{
    UnsupportedPacketType,
    UnsupportedOpcode,
    UnsupportedEventCode,
    InvalidLength,
    UnknownVendorSpecific,
}
