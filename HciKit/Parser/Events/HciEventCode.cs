// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public readonly record struct HciEventCode(byte Value)
{
    public bool IsVendorSpecific => Value == 0xFF;

    public override string ToString() => $"0x{Value:X4}";
}

internal static class HciEventCodes
{
    public const byte CommandComplete = 0x0E;
    public const byte CommandStatus = 0x0F;
}
