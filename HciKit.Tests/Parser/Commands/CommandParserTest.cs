// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Commands;

namespace HciKit.Tests.Parser.Commands;

public class CommandParserTest
{
    [Fact]
    public void ParseCommandTest()
    {
        var parser = new HciParser();
        byte[] commandPacket = [0x1, 0x42, 0x20, 0x6, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0];

        Assert.True(parser.Parse(commandPacket) is HciCommand);
    }
}
