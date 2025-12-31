// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public abstract class HciEvent : HciPacket
{
    public HciEventCode EventCode { get; }

    protected HciEvent(HciEventCode eventCode)
    {
        EventCode = eventCode;
    }

    public override string Name => "Event";
}

// 7.7.14 Command Complete event
public sealed class CommandCompleteEvent : HciEvent
{
    public byte NumHciCommandPackets { get; }
    public ushort CommandOpcode { get; }
    public byte[] ReturnParameters { get; }

    public CommandCompleteEvent(byte numHciCommandPackets, ushort commandOpcode, byte[] returnParameters)
        : base(new(HciEventCodes.CommandComplete))
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
    public byte Status { get; }
    public byte NumHciCommandPackets { get; }
    public ushort CommandOpcode { get; }

    public CommandStatusEvent(byte status, byte numHciCommandPackets, ushort commandOpcode)
        : base(new HciEventCode(HciEventCodes.CommandStatus))
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
