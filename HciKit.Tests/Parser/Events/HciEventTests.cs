// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Events;

namespace HciKit.Tests.Parser.Events;

public class HciEventTests
{
    [Fact]
    public void CommandCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0xe, 0x4, 0x1, 0x3, 0xc, 0x0];

        var evt = parser.Parse(packet) as CommandCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x0e, evt.EventCode.Value);
        Assert.Equal(1, evt.NumHciCommandPackets);
        Assert.Equal(0x0c03, evt.CommandOpcode);
        Assert.Equal([0], evt.ReturnParameters);
    }

    [Fact]
    public void CommandStatusEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0xf, 0x4, 0x0, 0x1, 0x43, 0x20];

        var evt = parser.Parse(packet) as CommandStatusEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x0f, evt.EventCode.Value);
        Assert.Equal(0, evt.Status);
        Assert.Equal(1, evt.NumHciCommandPackets);
        Assert.Equal(0x2043, evt.CommandOpcode);
    }
}
