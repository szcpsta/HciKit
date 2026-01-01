// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HciKit.Parser;
using HciKit.Parser.Commands;

namespace HciKit.Tests.Parser.Commands;

public class HciCommandTests
{
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

    [Fact(Skip = "TODO: Opcode=0x207F")]
    public void LeSetExtendedAdvertisingParametersV2CommandTest()
    {
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
}
