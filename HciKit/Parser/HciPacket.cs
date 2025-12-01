// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser;

public abstract class HciPacket
{
    public abstract string Name { get; }

    public override string ToString() => Name;
}
