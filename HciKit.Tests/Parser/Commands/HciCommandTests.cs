// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Commands;

namespace HciKit.Tests.Parser.Commands;

public class HciCommandTests
{
    #region 7.1 Link Control commands (OGF: 0x01)
    [Fact]
    public void InquiryCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x1, 0x4, 0x5, 0x33, 0x8b, 0x9e, 0xa, 0x0];

        var cmd = parser.Parse(packet) as InquiryCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0401, cmd.Opcode.Value);
        Assert.Equal((uint)0x9e8b33, cmd.Lap);
        Assert.Equal(10, cmd.InquiryLength);
        Assert.Equal(0x00, cmd.NumResponses);
    }

    [Fact(Skip = "TODO: Opcode=0x0402")]
    public void InquiryCancelCommandTest()
    {
    }

    [Fact(Skip = "TODO: Opcode=0x0403")]
    public void PeriodicInquiryModeCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0404")]
    public void ExitPeriodicInquiryModeCommandTest()
    {

    }

    [Fact]
    public void CreateConnectionCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x5, 0x4, 0xd, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x18, 0xcc, 0x1, 0x0, 0x0, 0x0, 0x1];

        var cmd = parser.Parse(packet) as CreateConnectionCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0405, cmd.Opcode.Value);
        Assert.Equal((ulong)0x10e4c2975044, cmd.BdAdder);
        Assert.Equal(0xcc18, cmd.PacketType);
        Assert.Equal(0x01, cmd.PageScanRepetitionMode);
        Assert.Equal(0x00, cmd.Reserved);
        Assert.Equal(0x0000, cmd.ClockOffset);
        Assert.Equal(0x01, cmd.AllowRoleSwitch);
    }

    [Fact(Skip = "TODO: Opcode=0x0406")]
    public void DisconnectCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0408")]
    public void CreateConnectionCancelCommandTest()
    {

    }

    [Fact]
    public void AcceptConnectionRequestCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x9, 0x4, 0x7, 0xcf, 0xf0, 0xa, 0xce, 0x9e, 0xf4, 0x0];

        var cmd = parser.Parse(packet) as AcceptConnectionRequestCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0409, cmd.Opcode.Value);
        Assert.Equal((ulong)0xf49ece0af0cf, cmd.BdAdder);
        Assert.Equal(0x00, cmd.Role);
    }

    [Fact(Skip = "TODO: Opcode=0x040A")]
    public void RejectConnectionRequestCommandTest()
    {
    }

    [Fact]
    public void LinkKeyRequestReplyCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0xb, 0x4, 0x16, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x70, 0xc9, 0x4, 0x3d, 0x7, 0xc2, 0x99, 0x26, 0x64, 0x34, 0xe, 0xc2, 0xa4, 0x9c, 0xf9, 0xab];

        var cmd = parser.Parse(packet) as LinkKeyRequestReplyCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x040B, cmd.Opcode.Value);
        Assert.Equal((ulong)0x10e4c2975044, cmd.BdAdder);
        Assert.Equal([0x70, 0xc9, 0x4, 0x3d, 0x7, 0xc2, 0x99, 0x26, 0x64, 0x34, 0xe, 0xc2, 0xa4, 0x9c, 0xf9, 0xab], cmd.LinkKey);
    }

    [Fact(Skip = "TODO: Opcode=0x040C")]
    public void LinkKeyRequestNegativeReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x040D")]
    public void PinCodeRequestReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x040E")]
    public void PinCodeRequestNegativeReplyCommandTest()
    {

    }

    [Fact]
    public void ChangeConnectionPacketTypeCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0xf, 0x4, 0x4, 0x3, 0x0, 0x18, 0xcc];

        var cmd = parser.Parse(packet) as ChangeConnectionPacketTypeCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x040F, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
        Assert.Equal(0xcc18, cmd.PacketType);
    }

    [Fact]
    public void AuthenticationRequestedCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x11, 0x4, 0x2, 0x3, 0x0];

        var cmd = parser.Parse(packet) as AuthenticationRequestedCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0411, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
    }

    [Fact(Skip = "TODO: Opcode=0x0413")]
    public void SetConnectionEncryptionCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x13, 0x4, 0x3, 0x3, 0x0, 0x1];

        var cmd = parser.Parse(packet) as SetConnectionEncryptionCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0413, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
        Assert.Equal(0x01, cmd.EncryptionEnable);
    }

    [Fact(Skip = "TODO: Opcode=0x0415")]
    public void ChangeConnectionLinkKeyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0417")]
    public void LinkKeySelectionCommandTest()
    {

    }

    [Fact]
    public void RemoteNameRequestCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x19, 0x4, 0xa, 0xcf, 0xf0, 0xa, 0xce, 0x9e, 0xf4, 0x1, 0x0, 0x0, 0x0];

        var cmd = parser.Parse(packet) as RemoteNameRequestCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0419, cmd.Opcode.Value);
        Assert.Equal((ulong)0xf49ece0af0cf, cmd.BdAdder);
        Assert.Equal(0x01, cmd.PageScanRepetitionMode);
        Assert.Equal(0x00, cmd.Reserved);
        Assert.Equal(0x0000, cmd.ClockOffset);
    }

    [Fact(Skip = "TODO: Opcode=0x041A")]
    public void RemoteNameRequestCancelCommandTest()
    {

    }

    [Fact]
    public void ReadRemoteSupportedFeaturesCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x1b, 0x4, 0x2, 0x3, 0x0];

        var cmd = parser.Parse(packet) as ReadRemoteSupportedFeaturesCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x041B, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
    }

    [Fact]
    public void ReadRemoteExtendedFeaturesCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x1c, 0x4, 0x3, 0x3, 0x0, 0x2];

        var cmd = parser.Parse(packet) as ReadRemoteExtendedFeaturesCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x041C, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
        Assert.Equal(2, cmd.PageNumber);
    }

    [Fact]
    public void ReadRemoteVersionInformationCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x1d, 0x4, 0x2, 0x4, 0x0];

        var cmd = parser.Parse(packet) as ReadRemoteVersionInformationCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x041D, cmd.Opcode.Value);
        Assert.Equal(0x0004, cmd.ConnectionHandle);
    }

    [Fact]
    public void ReadClockOffsetCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x1f, 0x4, 0x2, 0x4, 0x0];

        var cmd = parser.Parse(packet) as ReadClockOffsetCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x041F, cmd.Opcode.Value);
        Assert.Equal(0x0004, cmd.ConnectionHandle);
    }

    [Fact(Skip = "TODO: Opcode=0x0420")]
    public void ReadLmpHandleCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0428")]
    public void SetupSynchronousConnectionCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0429")]
    public void AcceptSynchronousConnectionRequestCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x042A")]
    public void RejectSynchronousConnectionRequestCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x042B")]
    public void IoCapabilityRequestReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x042C")]
    public void UserConfirmationRequestReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x042D")]
    public void UserConfirmationRequestNegativeReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x042E")]
    public void UserPasskeyRequestReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x042F")]
    public void UserPasskeyRequestNegativeReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0430")]
    public void RemoteOobDataRequestReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0433")]
    public void RemoteOobDataRequestNegativeReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0434")]
    public void IoCapabilityRequestNegativeReplyCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x043D")]
    public void EnhancedSetupSynchronousConnectionCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x043E")]
    public void EnhancedAcceptSynchronousConnectionRequestCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x043F")]
    public void TruncatedPageCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0440")]
    public void TruncatedPageCancelCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0441")]
    public void SetConnectionlessPeripheralBroadcastCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0442")]
    public void SetConnectionlessPeripheralBroadcastReceiveCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0443")]
    public void StartSynchronizationTrainCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0444")]
    public void ReceiveSynchronizationTrainCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0445")]
    public void RemoteOobExtendedDataRequestReplyCommandTest()
    {

    }
    #endregion 7.1 Link Control commands (OGF: 0x01)

    #region 7.2 Link Policy commands (OGF: 0x02)
    [Fact(Skip = "TODO: Opcode=0x0801")]
    public void HoldModeCommandTest()
    {

    }

    [Fact]
    public void SniffModeCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x3, 0x8, 0xa, 0x4, 0x0, 0x20, 0x3, 0x90, 0x1, 0x4, 0x0, 0x1, 0x0];

        var cmd = parser.Parse(packet) as SniffModeCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0803, cmd.Opcode.Value);
        Assert.Equal(0x0004, cmd.ConnectionHandle);
        Assert.Equal(800, cmd.SniffMaxInterval);
        Assert.Equal(400, cmd.SniffMinInterval);
        Assert.Equal(4, cmd.SniffAttempt);
        Assert.Equal(1, cmd.SniffTimeout);
    }

    [Fact]
    public void ExitSniffModeCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x4, 0x8, 0x2, 0x3, 0x0];

        var cmd = parser.Parse(packet) as ExitSniffModeCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0804, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
    }

    [Fact(Skip = "TODO: Opcode=0x0807")]
    public void QoSSetupCommandTest()
    {

    }

    [Fact(Skip = "TODO: Opcode=0x0809")]
    public void RoleDiscoveryCommandTest()
    {

    }

    [Fact]
    public void SwitchRoleCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0xb, 0x8, 0x7, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x0];

        var cmd = parser.Parse(packet) as SwitchRoleCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x080B, cmd.Opcode.Value);
        Assert.Equal((ulong)0x10e4c2975044, cmd.BdAdder);
        Assert.Equal(0x00, cmd.Role);
    }

    [Fact(Skip = "TODO: Opcode=0x080C")]
    public void ReadLinkPolicySettingsCommandTest()
    {

    }

    [Fact]
    public void WriteLinkPolicySettingsCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0xd, 0x8, 0x4, 0x3, 0x0, 0x5, 0x0];

        var cmd = parser.Parse(packet) as WriteLinkPolicySettingsCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x080D, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
        Assert.Equal(0x0005, cmd.LinkPolicySettings);
    }

    [Fact(Skip = "TODO: Opcode=0x080E")]
    public void ReadDefaultLinkPolicySettingsCommandTest()
    {

    }

    [Fact]
    public void WriteDefaultLinkPolicySettingsCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0xf, 0x8, 0x2, 0x5, 0x0];

        var cmd = parser.Parse(packet) as WriteDefaultLinkPolicySettingsCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x080F, cmd.Opcode.Value);
        Assert.Equal(0x0005, cmd.DefaultLinkPolicySettings);
    }

    [Fact(Skip = "TODO: Opcode=0x0810")]
    public void FlowSpecificationCommandTest()
    {

    }

    [Fact]
    public void SniffSubratingCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x11, 0x8, 0x8, 0x3, 0x0, 0xb0, 0x4, 0x2, 0x0, 0x2, 0x0];

        var cmd = parser.Parse(packet) as SniffSubratingCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0811, cmd.Opcode.Value);
        Assert.Equal(0x0003, cmd.ConnectionHandle);
        Assert.Equal(1200, cmd.MaxLatency);
        Assert.Equal(2, cmd.MinRemoteTimeout);
        Assert.Equal(2, cmd.MinLocalTimeout);
    }
    #endregion 7.2 Link Policy commands (OGF: 0x02)

    #region 7.3 Controller & Baseband commands (OGF: 0x03)
    [Fact]
    public void WriteScanEnableCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x1a, 0xc, 0x1, 0x2];

        var cmd = parser.Parse(packet) as WriteScanEnableCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x0c1a, cmd.Opcode.Value);
        Assert.Equal(0x02, cmd.ScanEnable);
    }
    #endregion 7.3 Controller & Baseband commands (OGF: 0x03)

    #region 7.4 Informational parameters (OGF: 0x04)
    [Fact]
    public void ReadLocalVersionInformationCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x1, 0x10, 0x0];

        var cmd = parser.Parse(packet) as ReadLocalVersionInformationCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x1001, cmd.Opcode.Value);
    }

    [Fact]
    public void ReadLocalSupportedCommandsCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x2, 0x10, 0x0];

        var cmd = parser.Parse(packet) as ReadLocalSupportedCommandsCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x1002, cmd.Opcode.Value);
    }

    [Fact(Skip = "TODO: Opcode=0x1003")]
    public void ReadLocalSupportedFeaturesCommandTest()
    {
    }

    [Fact]
    public void ReadLocalExtendedFeaturesCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x4, 0x10, 0x1, 0x1];

        var cmd = parser.Parse(packet) as ReadLocalExtendedFeaturesCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x1004, cmd.Opcode.Value);
        Assert.Equal(1, cmd.PageNumber);
    }

    [Fact]
    public void ReadBufferSizeCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x5, 0x10, 0x0];

        var cmd = parser.Parse(packet) as ReadBufferSizeCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x1005, cmd.Opcode.Value);
    }

    [Fact]
    public void ReadBdAddrCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x9, 0x10, 0x0];

        var cmd = parser.Parse(packet) as ReadBdAddrCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x1009, cmd.Opcode.Value);
    }

    [Fact(Skip = "TODO: Opcode=0x100A")]
    public void ReadDataBlockSizeCommandTest()
    {
    }

    [Fact]
    public void ReadLocalSupportedCodecsV1CommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0xb, 0x10, 0x0];

        var cmd = parser.Parse(packet) as ReadLocalSupportedCodecsV1Command;

        Assert.NotNull(cmd);
        Assert.Equal(0x100B, cmd.Opcode.Value);
    }

    [Fact(Skip = "TODO: Opcode=0x100C")]
    public void ReadLocalSimplePairingOptionsCommandTest()
    {
    }

    [Fact(Skip = "TODO: Opcode=0x100D")]
    public void ReadLocalSupportedCodecsV2CommandTest()
    {
    }

    [Fact(Skip = "TODO: Opcode=0x100E")]
    public void ReadLocalSupportedCodecCapabilitiesCommandTest()
    {
    }

    [Fact(Skip = "TODO: Opcode=0x100F")]
    public void ReadLocalSupportedControllerDelayCommandTest()
    {
    }
    #endregion 7.4 Informational parameters (OGF: 0x04)

    #region 7.8 LE Controller commands (OCF: 0x08)
    [Fact]
    public void LeSetExtendedAdvertisingParametersV1CommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x36, 0x20, 0x19, 0x0, 0x13, 0x0, 0xb0, 0x1, 0x0, 0xb0, 0x1, 0x0, 0x7, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xf1, 0x1, 0x0, 0x1, 0x0, 0x0];

        var cmd = parser.Parse(packet) as LeSetExtendedAdvertisingParametersV1Command;

        Assert.NotNull(cmd);
        Assert.Equal(0x2036, cmd.Opcode.Value);
        Assert.Equal(0x00, cmd.AdvertisingHandle);
        Assert.Equal(0x0013, cmd.AdvertisingEventProperties);
        Assert.Equal((uint)432, cmd.PrimaryAdvertisingIntervalMin);
        Assert.Equal((uint)432, cmd.PrimaryAdvertisingIntervalMax);
        Assert.Equal(0x07, cmd.PrimaryAdvertisingChannelMap);
        Assert.Equal(0x01, cmd.OwnAddressType);
        Assert.Equal(0x00, cmd.PeerAddressType);
        Assert.Equal((ulong)0x000000000000, cmd.PeerAddress);
        Assert.Equal(-15, cmd.AdvertisingTxPower);
        Assert.Equal(0x01, cmd.PrimaryAdvertisingPhy);
        Assert.Equal(0, cmd.SecondaryAdvertisingMaxSkip);
        Assert.Equal(0x01, cmd.SecondaryAdvertisingPhy);
        Assert.Equal(0x00, cmd.AdvertisingSid);
        Assert.Equal(0x00, cmd.ScanRequestNotificationEnable);
    }

    [Fact]
    public void LeSetExtendedAdvertisingEnableCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x39, 0x20, 0x6, 0x1, 0x1, 0x0, 0x0, 0x0, 0x0];

        var cmd = parser.Parse(packet) as LeSetExtendedAdvertisingEnableCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x2039, cmd.Opcode.Value);
        Assert.Equal(0x01, cmd.Enable);
        Assert.Equal(0x01, cmd.NumSets);
        Assert.Single(cmd.Sets);
        Assert.Equal(0x00, cmd.Sets[0].AdvertisingHandle);
        Assert.Equal(0, cmd.Sets[0].Duration);
        Assert.Equal(0, cmd.Sets[0].MaxExtendedAdvertisingEvents);
    }

    [Fact]
    public void LeSetExtendedScanParametersCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x41, 0x20, 0x8, 0x1, 0x0, 0x1, 0x1, 0xc0, 0x12, 0xc0, 0x12];

        var cmd = parser.Parse(packet) as LeSetExtendedScanParametersCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x2041, cmd.Opcode.Value);
        Assert.Equal(0x01, cmd.OwnAddressType);
        Assert.Equal(0x00, cmd.ScanningFilterPolicy);
        Assert.Equal(1, cmd.ScanningPhys & 0x01);
        Assert.Equal(0, (cmd.ScanningPhys >> 2) & 0x01);
        Assert.Single(cmd.ScanParameters);
        Assert.Equal(0x01, cmd.ScanParameters[0].ScanType);
        Assert.Equal(4800, cmd.ScanParameters[0].ScanInterval);
        Assert.Equal(4800, cmd.ScanParameters[0].ScanWindow);
    }

    [Fact]
    public void LeSetExtendedScanEnableCommandTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x1, 0x42, 0x20, 0x6, 0x1, 0x0, 0x0, 0x0, 0x0, 0x0];

        var cmd = parser.Parse(packet) as LeSetExtendedScanEnableCommand;

        Assert.NotNull(cmd);
        Assert.Equal(0x2042, cmd.Opcode.Value);
        Assert.Equal(0x01, cmd.Enable);
        Assert.Equal(0x00, cmd.FilterDuplicates);
        Assert.Equal(0, cmd.Duration);
        Assert.Equal(0, cmd.Period);
    }

    [Fact(Skip = "TODO: Opcode=0x207F")]
    public void LeSetExtendedAdvertisingParametersV2CommandTest()
    {
    }
    #endregion 7.8 LE Controller commands (OCF: 0x08)
}
