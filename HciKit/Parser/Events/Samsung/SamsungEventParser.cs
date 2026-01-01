// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events.Samsung;

public class SamsungEventParser : IVendorSpecificEventParser
{
    public string VendorName => "Samsung";

    public HciPacket Parse(ref HciSpanReader r)
    {
        return SamsungEvent.Parse(ref r);
    }
}
