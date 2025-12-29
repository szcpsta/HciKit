// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace HciKit.Parser;

public static class ControllerErrorCode
{
    private static readonly IReadOnlyDictionary<byte, string> s_names =
        new Dictionary<byte, string>
        {
              {0x00, "Success"},
              {0x01, "Unknown HCI Command"},
              {0x02, "Unknown Connection Identifier"},
              {0x03, "Hardware Failure"},
              {0x04, "Page Timeout"},
              {0x05, "Authentication Failure"},
              {0x06, "PIN or Key Missing"},
              {0x07, "Memory Capacity Exceeded"},
              {0x08, "Connection Timeout"},
              {0x09, "Connection Limit Exceeded"},
              {0x0A, "Synchronous Connection Limit To A Device Exceeded"},
              {0x0B, "ACL Connection Already Exists"},
              {0x0C, "Command Disallowed"},
              {0x0D, "Connection Rejected due to Limited Resources"},
              {0x0E, "Connection Rejected due To Security Reasons"},
              {0x0F, "Connection Rejected due to Unacceptable BD_ADDR"},
              {0x10, "Connection Accept Timeout Exceeded"},
              {0x11, "Unsupported Feature or Parameter Value"},
              {0x12, "Invalid HCI Command Parameters"},
              {0x13, "Remote User Terminated Connection"},
              {0x14, "Remote Device Terminated Connection due to Low Resources"},
              {0x15, "Remote Device Terminated Connection due to Power Off"},
              {0x16, "Connection Terminated by Local Host"},
              {0x17, "Repeated Attempts"},
              {0x18, "Pairing Not Allowed"},
              {0x19, "Unknown LMP PDU"},
              {0x1A, "Unsupported Remote/LMP Feature"},
              {0x1B, "SCO Offset Rejected"},
              {0x1C, "SCO Interval Rejected"},
              {0x1D, "SCO Air Mode Rejected"},
              {0x1E, "Invalid LMP/LL Parameters"},
              {0x1F, "Unspecified Error"},
              {0x20, "Unsupported LMP/LL Parameter Value"},
              {0x21, "Role Change Not Allowed"},
              {0x22, "LMP/LL Response Timeout"},
              {0x23, "LMP Error Transaction Collision/LL Procedure Collision"},
              {0x24, "LMP PDU Not Allowed"},
              {0x25, "Encryption Mode Not Acceptable"},
              {0x26, "Link Key cannot be Changed"},
              {0x27, "Requested QoS Not Supported"},
              {0x28, "Instant Passed"},
              {0x29, "Pairing with Unit Key Not Supported"},
              {0x2A, "Different Transaction Collision"},
              {0x2C, "QoS Unacceptable Parameter"},
              {0x2D, "QoS Rejected"},
              {0x2E, "Channel Classification Not Supported"},
              {0x2F, "Insufficient Security"},
              {0x30, "Parameter Out Of Mandatory Range"},
              {0x32, "Role Switch Pending"},
              {0x34, "Reserved Slot Violation"},
              {0x35, "Role Switch Failed"},
              {0x36, "Extended Inquiry Response Too Large"},
              {0x37, "Secure Simple Pairing Not Supported By Host"},
              {0x38, "Host Busy - Pairing"},
              {0x39, "Connection Rejected due to No Suitable Channel Found"},
              {0x3A, "Controller Busy"},
              {0x3B, "Unacceptable Connection Parameters"},
              {0x3C, "Directed Advertising Timeout"},
              {0x3D, "Connection Terminated due to MIC Failure"},
              {0x3E, "Connection Failed to be Established"},
              {0x3F, "MAC Connection Failed"},
              {0x40, "Coarse Clock Adjustment Rejected but Will Try to Adjust Using Clock Dragging"},
              {0x41, "Type0 Submap Not Defined"},
              {0x42, "Unknown Advertising Identifier"},
              {0x43, "Limit Reached"},
              {0x44, "Operation Cancelled by Host"},
              {0x45, "Packet Too Long"},
              {0x46, "Too Late"},
              {0x47, "Too Early"},
              {0x48, "Insufficient Channels"},
        };

    public static string GetName(byte code)
        => s_names.TryGetValue(code, out string? name)
            ? name
            : $"Unknown Error (0x{code:X2})";
}
