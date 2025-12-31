// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace HciKit.Parser;

public ref struct HciSpanReader
{
    private ReadOnlySpan<byte> _span;

    public HciSpanReader(ReadOnlySpan<byte> span) => _span = span;

    public int Remaining => _span.Length;
    public bool IsEmpty => _span.IsEmpty;
    public ReadOnlySpan<byte> RemainingSpan => _span;

    // ----------------------------
    // Guards / helpers
    // ----------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Ensure(int byteCount)
    {
        if (_span.Length < byteCount)
            throw new ArgumentException($"Not enough data. Need {byteCount}, have {_span.Length}.");
    }

    // ----------------------------
    // Core consume operations
    // ----------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> ReadBytes(int length)
    {
        Ensure(length);
        var v = _span[..length];
        _span = _span[length..];
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Skip(int byteCount)
    {
        Ensure(byteCount);
        _span = _span[byteCount..];
    }

    // ----------------------------
    // Primitive reads (HCI: little-endian by default)
    // ----------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadU8()
    {
        Ensure(1);
        byte v = _span[0];
        _span = _span[1..];
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte Read8() => unchecked((sbyte)ReadU8());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadU16()
    {
        Ensure(2);
        ushort v = BinaryPrimitives.ReadUInt16LittleEndian(_span);
        _span = _span[2..];
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short Read16() => unchecked((short)ReadU16());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadU32()
    {
        Ensure(4);
        uint v = BinaryPrimitives.ReadUInt32LittleEndian(_span);
        _span = _span[4..];
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Read32() => unchecked((int)ReadU32());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadU64()
    {
        Ensure(8);
        ulong v = BinaryPrimitives.ReadUInt64LittleEndian(_span);
        _span = _span[8..];
        return v;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Read64() => unchecked((long)ReadU64());

    // ----------------------------
    // HCI convenience helpers
    // ----------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadU24()
    {
        Ensure(3);
        uint b0 = _span[0];
        uint b1 = _span[1];
        uint b2 = _span[2];
        _span = _span[3..];

        // Little-endian in buffer: b0 is LSB.
        return (b0) | (b1 << 8) | (b2 << 16);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadU48()
    {
        Ensure(6);
        ulong b0 = _span[0];
        ulong b1 = _span[1];
        ulong b2 = _span[2];
        ulong b3 = _span[3];
        ulong b4 = _span[4];
        ulong b5 = _span[5];
        _span = _span[6..];

        // Little-endian in buffer: b0 is LSB.
        return (b0) | (b1 << 8) | (b2 << 16) | (b3 << 24) | (b4 << 32) | (b5 << 40);
    }
}
