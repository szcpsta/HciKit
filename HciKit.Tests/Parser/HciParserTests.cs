// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Events;

namespace HciKit.Tests.Parser;

public class HciParserTests
{
    [Fact]
    public void VendorSpecificEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0xff, 0x14, 0x56, 0x4, 0x0, 0x0, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x2, 0x80, 0xc0, 0x0, 0x0, 0x3, 0x2, 0x1, 0x2, 0x0];

        var vse = parser.Parse(packet) as HciEvent;

        Assert.NotNull(vse);
        Assert.True(vse.EventCode.IsVendorSpecific);
    }
}
