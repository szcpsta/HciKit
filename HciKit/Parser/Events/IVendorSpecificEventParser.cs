// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser.Events;

public interface IVendorSpecificEventParser
{
    string VendorName { get; }

    HciPacket Parse(ref HciSpanReader r);
}
