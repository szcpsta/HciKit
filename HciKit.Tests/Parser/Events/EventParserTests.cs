// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Events;

namespace HciKit.Tests.Parser.Events;

public class EventParserTests
{
    [Fact]
    public void ParseEventTest()
    {
        var parser = new HciParser();

        byte[] eventPacket = [0x4, 0xe, 0x4, 0x1, 0x3, 0xc, 0x0];

        Assert.True(parser.Parse(eventPacket) is HciEvent);
    }
}
