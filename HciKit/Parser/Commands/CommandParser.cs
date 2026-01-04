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
            HciOpcodes.RemoteNameRequestCancel => RemoteNameRequestCancelCommand.Parse(ref r),
            HciOpcodes.ReadRemoteSupportedFeatures => ReadRemoteSupportedFeaturesCommand.Parse(ref r),
            HciOpcodes.ReadRemoteExtendedFeatures => ReadRemoteExtendedFeaturesCommand.Parse(ref r),
            HciOpcodes.ReadRemoteVersionInformation => ReadRemoteVersionInformationCommand.Parse(ref r),
            HciOpcodes.ReadClockOffset => ReadClockOffsetCommand.Parse(ref r),
            HciOpcodes.ReadLmpHandle => ReadLmpHandleCommand.Parse(ref r),
            HciOpcodes.SetupSynchronousConnection => SetupSynchronousConnectionCommand.Parse(ref r),
            HciOpcodes.AcceptSynchronousConnectionRequest => AcceptSynchronousConnectionRequestCommand.Parse(ref r),
            HciOpcodes.RejectSynchronousConnectionRequest => RejectSynchronousConnectionRequestCommand.Parse(ref r),
            HciOpcodes.IoCapabilityRequestReply => IoCapabilityRequestReplyCommand.Parse(ref r),
            HciOpcodes.UserConfirmationRequestReply => UserConfirmationRequestReplyCommand.Parse(ref r),
            HciOpcodes.UserConfirmationRequestNegativeReply => UserConfirmationRequestNegativeReplyCommand.Parse(ref r),
            HciOpcodes.UserPasskeyRequestReply => UserPasskeyRequestReplyCommand.Parse(ref r),
            HciOpcodes.UserPasskeyRequestNegativeReply => UserPasskeyRequestNegativeReplyCommand.Parse(ref r),
            HciOpcodes.RemoteOobDataRequestReply => RemoteOobDataRequestReplyCommand.Parse(ref r),
            HciOpcodes.RemoteOobDataRequestNegativeReply => RemoteOobDataRequestNegativeReplyCommand.Parse(ref r),
            HciOpcodes.IoCapabilityRequestNegativeReply => IoCapabilityRequestNegativeReplyCommand.Parse(ref r),
            HciOpcodes.EnhancedSetupSynchronousConnection => EnhancedSetupSynchronousConnectionCommand.Parse(ref r),
            HciOpcodes.EnhancedAcceptSynchronousConnectionRequest => EnhancedAcceptSynchronousConnectionRequestCommand.Parse(ref r),
            HciOpcodes.TruncatedPage => TruncatedPageCommand.Parse(ref r),
            HciOpcodes.TruncatedPageCancel => TruncatedPageCancelCommand.Parse(ref r),
            HciOpcodes.SetConnectionlessPeripheralBroadcast => SetConnectionlessPeripheralBroadcastCommand.Parse(ref r),
            HciOpcodes.SetConnectionlessPeripheralBroadcastReceive => SetConnectionlessPeripheralBroadcastReceiveCommand.Parse(ref r),
            HciOpcodes.StartSynchronizationTrain => StartSynchronizationTrainCommand.Parse(ref r),
            HciOpcodes.ReceiveSynchronizationTrain => ReceiveSynchronizationTrainCommand.Parse(ref r),
            HciOpcodes.RemoteOobExtendedDataRequestReply => RemoteOobExtendedDataRequestReplyCommand.Parse(ref r),
            // 7.2 Link Policy commands (OGF: 0x02)
            HciOpcodes.HoldMode => HoldModeCommand.Parse(ref r),
            HciOpcodes.SniffMode => SniffModeCommand.Parse(ref r),
            HciOpcodes.ExitSniffMode => ExitSniffModeCommand.Parse(ref r),
            HciOpcodes.QoSSetup => QoSSetupCommand.Parse(ref r),
            HciOpcodes.RoleDiscovery => RoleDiscoveryCommand.Parse(ref r),
            HciOpcodes.SwitchRole => SwitchRoleCommand.Parse(ref r),
            HciOpcodes.ReadLinkPolicySettings => ReadLinkPolicySettingsCommand.Parse(ref r),
            HciOpcodes.WriteLinkPolicySettings => WriteLinkPolicySettingsCommand.Parse(ref r),
            HciOpcodes.ReadDefaultLinkPolicySettings => ReadDefaultLinkPolicySettingsCommand.Parse(ref r),
            HciOpcodes.WriteDefaultLinkPolicySettings => WriteDefaultLinkPolicySettingsCommand.Parse(ref r),
            HciOpcodes.FlowSpecification => FlowSpecificationCommand.Parse(ref r),
            HciOpcodes.SniffSubrating => SniffSubratingCommand.Parse(ref r),
            // 7.3 Controller & Baseband commands (OGF: 0x03)
            HciOpcodes.WriteScanEnable => WriteScanEnableCommand.Parse(ref r),
            // 7.4 Informational Parameters (OGF: 0x04)
            HciOpcodes.ReadLocalVersionInformation => ReadLocalVersionInformationCommand.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCommands => ReadLocalSupportedCommandsCommand.Parse(ref r),
            HciOpcodes.ReadLocalSupportedFeatures => ReadLocalSupportedFeaturesCommand.Parse(ref r),
            HciOpcodes.ReadLocalExtendedFeatures => ReadLocalExtendedFeaturesCommand.Parse(ref r),
            HciOpcodes.ReadBufferSize => ReadBufferSizeCommand.Parse(ref r),
            HciOpcodes.ReadBdAddr => ReadBdAddrCommand.Parse(ref r),
            HciOpcodes.ReadDataBlockSize => ReadDataBlockSizeCommand.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCodecsV1 => ReadLocalSupportedCodecsV1Command.Parse(ref r),
            HciOpcodes.ReadLocalSimplePairingOptions => ReadLocalSimplePairingOptionsCommand.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCodecsV2 => ReadLocalSupportedCodecsV2Command.Parse(ref r),
            HciOpcodes.ReadLocalSupportedCodecCapabilities => ReadLocalSupportedCodecCapabilitiesCommand.Parse(ref r),
            HciOpcodes.ReadLocalSupportedControllerDelay => ReadLocalSupportedControllerDelayCommand.Parse(ref r),
            // 7.5 Status Parameters (OGF: 0x05)
            HciOpcodes.ReadFailedContactCounter => ReadFailedContactCounterCommand.Parse(ref r),
            HciOpcodes.ResetFailedContactCounter => ResetFailedContactCounterCommand.Parse(ref r),
            HciOpcodes.ReadLinkQuality => ReadLinkQualityCommand.Parse(ref r),
            HciOpcodes.ReadRssi => ReadRssiCommand.Parse(ref r),
            HciOpcodes.ReadAfhChannelMap => ReadAfhChannelMapCommand.Parse(ref r),
            HciOpcodes.ReadClock => ReadClockCommand.Parse(ref r),
            HciOpcodes.ReadEncryptionKeySize => ReadEncryptionKeySizeCommand.Parse(ref r),
            HciOpcodes.GetMwsTransportLayerConfiguration => GetMwsTransportLayerConfigurationCommand.Parse(ref r),
            HciOpcodes.SetTriggeredClockCapture => SetTriggeredClockCaptureCommand.Parse(ref r),
            // 7.6 Testing commands (OGF: 0x06)
            HciOpcodes.ReadLoopbackMode => ReadLoopbackModeCommand.Parse(ref r),
            HciOpcodes.WriteLoopbackMode => WriteLoopbackModeCommand.Parse(ref r),
            HciOpcodes.EnableImplementationUnderTestMode => EnableImplementationUnderTestModeCommand.Parse(ref r),
            HciOpcodes.WriteSimplePairingDebugMode => WriteSimplePairingDebugModeCommand.Parse(ref r),
            HciOpcodes.WriteSecureConnectionsTestMode => WriteSecureConnectionsTestModeCommand.Parse(ref r),
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
