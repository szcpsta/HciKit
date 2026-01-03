// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;

namespace HciKit.Parser.Commands;

internal class CommandParser
{
    private enum Offset : byte
    {
        Opcode = 0,
        ParameterTotalLength = 2,
        Parameter = 3,
    }

    public HciPacket Parse(ReadOnlySpan<byte> p)
    {
        if (p.Length != (byte)Offset.Parameter + GetParameterTotalLength(p))
        {
            return new UnknownHciPacket(HciUnknownReason.InvalidLength, HciPacketType.Command);
        }

        HciSpanReader r = new HciSpanReader(p);
        r.Skip((int)Offset.Parameter);

        var opcode = GetOpcode(p);
        return opcode switch
        {
            // 7.1 Link Control commands (OGF: 0x01)
            HciOpcodes.Inquiry => InquiryCommand.Parse(ref r),
            HciOpcodes.InquiryCancel => InquiryCancelCommand.Parse(ref r),
            HciOpcodes.PeriodicInquiryMode => PeriodicInquiryModeCommand.Parse(ref r),
            HciOpcodes.ExitPeriodicInquiryMode => ExitPeriodicInquiryModeCommand.Parse(ref r),
            HciOpcodes.CreateConnection => CreateConnectionCommand.Parse(ref r),
            HciOpcodes.Disconnect => DisconnectCommand.Parse(ref r),
            HciOpcodes.CreateConnectionCancel => CreateConnectionCancelCommand.Parse(ref r),
            HciOpcodes.AcceptConnectionRequest => AcceptConnectionRequestCommand.Parse(ref r),
            HciOpcodes.RejectConnectionRequest => RejectConnectionRequestCommand.Parse(ref r),
            HciOpcodes.LinkKeyRequestReply => LinkKeyRequestReplyCommand.Parse(ref r),
            HciOpcodes.LinkKeyRequestNegativeReply => LinkKeyRequestNegativeReplyCommand.Parse(ref r),
            HciOpcodes.PinCodeRequestReply => PinCodeRequestReplyCommand.Parse(ref r),
            HciOpcodes.PinCodeRequestNegativeReply => PinCodeRequestNegativeReplyCommand.Parse(ref r),
            HciOpcodes.ChangeConnectionPacketType => ChangeConnectionPacketTypeCommand.Parse(ref r),
            HciOpcodes.AuthenticationRequested => AuthenticationRequestedCommand.Parse(ref r),
            HciOpcodes.SetConnectionEncryption => SetConnectionEncryptionCommand.Parse(ref r),
            HciOpcodes.ChangeConnectionLinkKey => ChangeConnectionLinkKeyCommand.Parse(ref r),
            HciOpcodes.LinkKeySelection => LinkKeySelectionCommand.Parse(ref r),
            HciOpcodes.RemoteNameRequest => RemoteNameRequestCommand.Parse(ref r),
            // 7.2 Link Policy commands (OGF: 0x02)
            HciOpcodes.SniffMode => SniffModeCommand.Parse(ref r),
            HciOpcodes.ExitSniffMode => ExitSniffModeCommand.Parse(ref r),
            HciOpcodes.SniffSubrating => SniffSubratingCommand.Parse(ref r),
            // 7.3 Controller & Baseband commands (OGF: 0x03)
            HciOpcodes.WriteScanEnable => WriteScanEnableCommand.Parse(ref r),
            // 7.8 LE Controller commands (OCF: 0x08)
            HciOpcodes.LeSetExtendedAdvertisingParametersV1 => LeSetExtendedAdvertisingParametersV1Command.Parse(ref r),
            HciOpcodes.LeSetExtendedAdvertisingEnable => LeSetExtendedAdvertisingEnableCommand.Parse(ref r),
            HciOpcodes.LeSetExtendedScanParameters => LeSetExtendedScanParametersCommand.Parse(ref r),
            HciOpcodes.LeSetExtendedScanEnable => LeSetExtendedScanEnableCommand.Parse(ref r),
            HciOpcodes.LeSetExtendedAdvertisingParametersV2 => LeSetExtendedAdvertisingParametersV2Command.Parse(ref r),

            _ => new UnknownHciPacket(HciUnknownReason.UnsupportedOpcode, HciPacketType.Command, opcode)
        };
    }

    private static ushort GetOpcode(ReadOnlySpan<byte> p)
        => BinaryPrimitives.ReadUInt16LittleEndian(p.Slice((byte)Offset.Opcode, 2));

    private static byte GetParameterTotalLength(ReadOnlySpan<byte> p)
        => p[(byte)Offset.ParameterTotalLength];

    private static ReadOnlySpan<byte> GetParameter(ReadOnlySpan<byte> p)
        => p.Slice((byte)Offset.Parameter);
}
