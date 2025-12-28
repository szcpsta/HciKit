// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;

namespace HciKit.Parser.Events;

internal class EventParser
{
    private enum Offset : byte
    {
        EventCode = 0,
        ParameterTotalLength = 1,
        Parameter = 2,
    }

    public static HciEvent Parse(ReadOnlySpan<byte> p)
    {
        if (p.Length != (byte)Offset.Parameter + GetParameterTotalLength(p))
        {
            throw new InvalidDataException();
        }

        HciSpanReader r = new HciSpanReader(p);
        r.Skip((int)Offset.Parameter);

        return GetEventCode(p) switch
        {
            HciEventCodes.CommandComplete => CommandCompleteEvent.Parse(ref r),
            HciEventCodes.CommandStatus => CommandStatusEvent.Parse(ref r),
            _ => throw new InvalidDataException()
        };
    }

    private static byte GetEventCode(ReadOnlySpan<byte> p)
        => p[(byte)Offset.EventCode];

    private static byte GetParameterTotalLength(ReadOnlySpan<byte> p)
        => p[(byte)Offset.ParameterTotalLength];

    private static ReadOnlySpan<byte> GetParameter(ReadOnlySpan<byte> p)
        => p.Slice((byte)Offset.Parameter);
}
