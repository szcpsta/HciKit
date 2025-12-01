// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Commands;

public class HciCommand : HciPacket
{
    public HciOpcode Opcode { get; init; }
    public override string Name => "Command";
}
