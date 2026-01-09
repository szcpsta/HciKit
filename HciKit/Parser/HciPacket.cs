// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser;

public abstract class HciPacket
{
    public HciPacketType PacketType { get; }

    protected HciPacket(HciPacketType packetType)
    {
        PacketType = packetType;
    }

    public abstract string Name { get; }

    public override string ToString() => Name;
}

public sealed class HciAcl : HciPacket
{
    public override string Name => "Acl";

    public byte[] Parameters { get; }

    public HciAcl(byte[] parameters) : base(HciPacketType.Acl)
    {
        Parameters = parameters;
    }

    public static HciAcl Parse(ReadOnlySpan<byte> packet)
    {
        return new HciAcl(packet.ToArray());
    }
}

public sealed class HciSco : HciPacket
{
    public override string Name => "Sco";

    public byte[] Parameters { get; }

    public HciSco(byte[] parameters) : base(HciPacketType.Sco)
    {
        Parameters = parameters;
    }

    public static HciSco Parse(ReadOnlySpan<byte> packet)
    {
        return new HciSco(packet.ToArray());
    }
}

public sealed class HciIso : HciPacket
{
    public override string Name => "Iso";

    public byte[] Parameters { get; }

    public HciIso(byte[] parameters) : base(HciPacketType.Iso)
    {
        Parameters = parameters;
    }

    public static HciIso Parse(ReadOnlySpan<byte> packet)
    {
        return new HciIso(packet.ToArray());
    }
}

public sealed class UnknownHciPacket : HciPacket
{
    public HciUnknownReason Reason { get; }

    public ushort? Opcode { get; }
    public byte? EventCode { get; }

    public UnknownHciPacket(
        HciUnknownReason reason,
        HciPacketType packetType,
        ushort? opcode = null,
        byte? eventCode = null) : base(packetType)
    {
        Reason = reason;
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
