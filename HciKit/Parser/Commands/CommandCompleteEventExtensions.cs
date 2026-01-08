// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser.Events;

namespace HciKit.Parser.Commands;

public static class CommandCompleteEventExtensions
{
    public static bool TryParseReturnParameters(this CommandCompleteEvent evt,
                                                out HciCommandReturnParameters? returnParameters)
    {
        return CommandReturnParametersParser.TryParse(evt.CommandOpcode, evt.ReturnParameters, out returnParameters);
    }
}
