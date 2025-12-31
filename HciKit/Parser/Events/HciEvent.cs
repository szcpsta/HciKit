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

// 7.7.1 Inquiry Complete event
public sealed class InquiryCompleteEvent : HciEvent
{
    public byte Status { get; }

    public InquiryCompleteEvent(byte status)
        : base(new(HciEventCodes.InquiryComplete))
    {
        Status = status;
    }

    public static InquiryCompleteEvent Parse(ref HciSpanReader r)
    {
        return new InquiryCompleteEvent(r.ReadU8());
    }
}

// 7.7.2 Inquiry Result event
public sealed class InquiryResultEvent : HciEvent
{

    public byte NumResponses { get; }
    public IReadOnlyList<InquiryResponse> Responses { get; }

    public readonly struct InquiryResponse(
        ulong BdAddr,
        byte PageScanRepetitionMode,
        ushort Reserved,
        uint ClassOfDevice,
        ushort ClockOffset
    );

    public InquiryResultEvent(byte numResponses, InquiryResponse[] responses)
        : base(new(HciEventCodes.InquiryResult))
    {
        NumResponses = numResponses;
        Responses = responses;
    }

    public static InquiryResultEvent Parse(ref HciSpanReader r)
    {
        byte numResponses = r.ReadU8();
        InquiryResponse[] responses = new InquiryResponse[numResponses];
        for (int i = 0; i < numResponses; i++)
        {
            responses[i] = new InquiryResponse(r.ReadU48(), r.ReadU8(), r.ReadU16(), r.ReadU24(), r.ReadU16());
        }

        return new InquiryResultEvent(numResponses, responses);
    }
}

// 7.7.3 Connection Complete event
public sealed class ConnectionCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public ulong BdAddr { get; }
    public byte LinkType { get; }
    public byte EncryptionEnabled { get; }

    public ConnectionCompleteEvent(byte status, ushort connectionHandle, ulong bdAddr, byte linkType, byte encryptionEnabled)
        : base(new(HciEventCodes.ConnectionComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        BdAddr = bdAddr;
        LinkType = linkType;
        EncryptionEnabled = encryptionEnabled;
    }

    public static ConnectionCompleteEvent Parse(ref HciSpanReader r)
    {
        return new ConnectionCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU48(), r.ReadU8(), r.ReadU8());
    }
}

// 7.7.4 Connection Request event
public sealed class ConnectionRequestEvent : HciEvent
{
    public ulong BdAddr { get; }
    public uint ClassOfDevice { get; }
    public byte LinkType { get; }

    public ConnectionRequestEvent(ulong bdAddr, uint classOfDevice, byte linkType)
        : base(new(HciEventCodes.ConnectionRequest))
    {
        BdAddr = bdAddr;
        ClassOfDevice = classOfDevice;
        LinkType = linkType;
    }

    public static ConnectionRequestEvent Parse(ref HciSpanReader r)
    {
        return new ConnectionRequestEvent(r.ReadU48(), r.ReadU24(), r.ReadU8());
    }
}

// 7.7.5 Disconnection Complete event
public sealed class DisconnectionCompleteEvent : HciEvent
{
    public byte Status { get; }
    public ushort ConnectionHandle { get; }
    public byte Reason { get; }

    public DisconnectionCompleteEvent(byte status, ushort connectionHandle, byte reason)
        : base(new(HciEventCodes.DisconnectionComplete))
    {
        Status = status;
        ConnectionHandle = connectionHandle;
        Reason = reason;
    }

    public static DisconnectionCompleteEvent Parse(ref HciSpanReader r)
    {
        return new DisconnectionCompleteEvent(r.ReadU8(), r.ReadU16(), r.ReadU8());
    }
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
