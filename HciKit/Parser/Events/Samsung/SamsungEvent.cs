// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events.Samsung;

public class SamsungEvent : HciEvent
{
    public SamsungEvent() : base(new(HciEventCodes.VendorSpecific))
    {
    }

    public static SamsungEvent Parse(ref HciSpanReader r)
    {
        return new SamsungEvent();
    }
}
