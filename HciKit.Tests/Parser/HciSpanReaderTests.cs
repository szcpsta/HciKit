// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;

namespace HciKit.Tests.Parser;

public class HciSpanReaderTests
{
    [Fact]
    public void HciSpanReaderTest()
    {
        byte[] hciPacket = [0x1, 0x3, 0xc, 0x0];

        HciSpanReader r = new HciSpanReader(hciPacket);

        Assert.Equal(4, r.Remaining);

        Assert.Equal(0x01, r.ReadU8());
        Assert.Equal(0x0c03, r.ReadU16());
        Assert.Equal(0x00, r.ReadU8());

        Assert.Equal(0, r.Remaining);
        Assert.True(r.IsEmpty);
    }

    [Fact]
    public void NullHciSpanReaderTest()
    {
        HciSpanReader r = new HciSpanReader();

        Assert.Equal(0, r.Remaining);
        Assert.True(r.IsEmpty);

        try
        {
            r.ReadU8();
            Assert.Fail("Expected exception was not thrown.");
        }
        catch (ArgumentException)
        {
            // success
        }
    }
}
