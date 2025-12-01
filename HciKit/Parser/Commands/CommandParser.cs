// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;

namespace HciKit.Parser.Commands;

internal class CommandParser
{
    private enum Offset : byte
    {
        Opcode = 0,
        ParameterTotalLength = 2,
        Parameter = 3,
    }

    public static HciCommand Parse(ReadOnlySpan<byte> p)
    {
        if (p.Length != (byte)Offset.Parameter + GetParameterTotalLength(p))
        {
            throw new InvalidDataException();
        }

        return new HciCommand();
    }

    private static ushort GetOpcode(ReadOnlySpan<byte> p)
        => BinaryPrimitives.ReadUInt16LittleEndian(p.Slice((byte)Offset.Opcode, 2));

    private static byte GetParameterTotalLength(ReadOnlySpan<byte> p)
        => p[(byte)Offset.ParameterTotalLength];

    private static ReadOnlySpan<byte> GetParameter(ReadOnlySpan<byte> p)
        => p.Slice((byte)Offset.Parameter);
}
