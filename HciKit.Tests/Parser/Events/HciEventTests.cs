// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Events;

namespace HciKit.Tests.Parser.Events;

public class HciEventTests
{
    [Fact]
    public void InquiryCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x1, 0x1, 0x0];

        var evt = parser.Parse(packet) as InquiryCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x01, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
    }

    [Fact(Skip = "TODO: EventCode=0x02")]
    public void InquiryResultEventTest()
    {
    }

    [Fact]
    public void ConnectionCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x3, 0xb, 0x0, 0x3, 0x0, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x1, 0x0];

        var evt = parser.Parse(packet) as ConnectionCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x03, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal((ulong)0x10e4c2975044, evt.BdAddr);
        Assert.Equal(0x01, evt.LinkType);
        Assert.Equal(0x00, evt.EncryptionEnabled);
    }

    [Fact]
    public void ConnectionRequestEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x4, 0xa, 0xcf, 0xf0, 0xa, 0xce, 0x9e, 0xf4, 0x4, 0x4, 0x24, 0x1];

        var evt = parser.Parse(packet) as ConnectionRequestEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x04, evt.EventCode.Value);
        Assert.Equal((ulong)0xf49ece0af0cf, evt.BdAddr);
        Assert.Equal((uint)0x240404, evt.ClassOfDevice);
        Assert.Equal(0x01, evt.LinkType);
    }

    [Fact]
    public void DisconnectionCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x5, 0x4, 0x0, 0x4, 0x0, 0x13];

        var evt = parser.Parse(packet) as DisconnectionCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x05, evt.EventCode.Value);
        Assert.Equal(0x0004, evt.ConnectionHandle);
        Assert.Equal(0x013, evt.Reason);
    }

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
