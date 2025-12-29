// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser.Commands;
using HciKit.Parser.Events;

namespace HciKit.Parser;

public enum HciPacketType : byte { Unknown = 0, Command = 0x01, Acl = 0x02, Sco = 0x03, Event = 0x04, Iso = 0x05 }

public class HciParser
{
    private enum Offset : byte
    {
        PacketType = 0,
        Parameter = 1,
    }

    public HciPacket Parse(ReadOnlySpan<byte> packet)
    {
        var packetType = GetPacketType(packet);
        return packetType switch
        {
            HciPacketType.Command => CommandParser.Parse(GetParameter(packet)),
            HciPacketType.Event => EventParser.Parse(GetParameter(packet)),
            _ => new UnknownHciPacket(HciUnknownReason.UnsupportedPacketType, packetType)
        };
    }

    private HciPacketType GetPacketType(ReadOnlySpan<byte> packet)
        => (HciPacketType)packet[(byte)Offset.PacketType];

    private ReadOnlySpan<byte> GetParameter(ReadOnlySpan<byte> packet)
        => packet.Slice((byte)Offset.Parameter);
}
