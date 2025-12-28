// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public abstract class HciEvent : HciPacket
{
    public abstract HciEventCode EventCode { get; }
    public override string Name => "Event";
}

// 7.7.14 Command Complete event
public sealed class CommandCompleteEvent : HciEvent
{
    public override HciEventCode EventCode => new(HciEventCodes.CommandComplete);

    public byte NumHciCommandPackets;
    public ushort CommandOpcode;
    public byte[] ReturnParameters;

    public CommandCompleteEvent(byte numHciCommandPackets, ushort commandOpcode, byte[] returnParameters)
    {
        NumHciCommandPackets = numHciCommandPackets;
        CommandOpcode = commandOpcode;
        ReturnParameters = returnParameters;
    }

    public static CommandCompleteEvent Parse(ref HciSpanReader r)
    {
        return new CommandCompleteEvent(r.ReadU8(), r.ReadU16(), r.RemainingSpan.ToArray());
    }
}

// 7.7.15 Command Status event
public sealed class CommandStatusEvent : HciEvent
{
    public override HciEventCode EventCode => new(HciEventCodes.CommandStatus);

    public byte Status;
    public byte NumHciCommandPackets;
    public ushort CommandOpcode;

    public CommandStatusEvent(byte status, byte numHciCommandPackets, ushort commandOpcode)
    {
        Status = status;
        NumHciCommandPackets = numHciCommandPackets;
        CommandOpcode = commandOpcode;
    }

    public static CommandStatusEvent Parse(ref HciSpanReader r)
    {
        return new CommandStatusEvent(r.ReadU8(), r.ReadU8(), r.ReadU16());
    }
}
