// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using HciKit.Parser;
using HciKit.Parser.Events;

namespace HciKit.Tests.Parser.Events;

public class HciEventTests
{
    [Fact]
    public void InquiryCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x1, 0x1, 0x0];

        var evt = parser.Parse(packet) as InquiryCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x01, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
    }

    [Fact(Skip = "TODO: EventCode=0x02")]
    public void InquiryResultEventTest()
    {
    }

    [Fact]
    public void ConnectionCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x3, 0xb, 0x0, 0x3, 0x0, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x1, 0x0];

        var evt = parser.Parse(packet) as ConnectionCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x03, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal((ulong)0x10e4c2975044, evt.BdAddr);
        Assert.Equal(0x01, evt.LinkType);
        Assert.Equal(0x00, evt.EncryptionEnabled);
    }

    [Fact]
    public void ConnectionRequestEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x4, 0xa, 0xcf, 0xf0, 0xa, 0xce, 0x9e, 0xf4, 0x4, 0x4, 0x24, 0x1];

        var evt = parser.Parse(packet) as ConnectionRequestEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x04, evt.EventCode.Value);
        Assert.Equal((ulong)0xf49ece0af0cf, evt.BdAddr);
        Assert.Equal((uint)0x240404, evt.ClassOfDevice);
        Assert.Equal(0x01, evt.LinkType);
    }

    [Fact]
    public void DisconnectionCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x5, 0x4, 0x0, 0x4, 0x0, 0x13];

        var evt = parser.Parse(packet) as DisconnectionCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x05, evt.EventCode.Value);
        Assert.Equal(0x0004, evt.ConnectionHandle);
        Assert.Equal(0x013, evt.Reason);
    }

    [Fact]
    public void AuthenticationCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x6, 0x3, 0x0, 0x3, 0x0];

        var evt = parser.Parse(packet) as AuthenticationCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x06, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
    }

    [Fact]
    public void RemoteNameRequestCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x7, 0xff, 0x0, 0xcf, 0xf0, 0xa, 0xce, 0x9e, 0xf4, 0x53, 0x65, 0x6e, 0x61, 0x20, 0x36, 0x30, 0x53, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];

        var evt = parser.Parse(packet) as RemoteNameRequestCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x07, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal((ulong)0xf49ece0af0cf, evt.BdAddr);
        Assert.Equal(248, evt.RemoteName.Length);
        Assert.Equal("Sena 60S", Encoding.UTF8.GetString(evt.RemoteName).TrimEnd('\0'));
    }

    [Fact]
    public void EncryptionChangedV1EventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x8, 0x4, 0x0, 0x3, 0x0, 0x2];

        var evt = parser.Parse(packet) as EncryptionChangeV1Event;

        Assert.NotNull(evt);
        Assert.Equal(0x08, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(0x02, evt.EncryptionEnabled);
    }

    [Fact(Skip = "TODO: EventCode=0x09")]
    public void ChangeConnectionLinkKeyCompleteEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x0A")]
    public void LinkKeyTypeChangedEventTest()
    {
    }

    [Fact]
    public void ReadRemoteSupportedFeaturesCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0xb, 0xb, 0x0, 0x3, 0x0, 0xbf, 0xfe, 0xcf, 0xfe, 0xdb, 0xff, 0x7b, 0x87];

        var evt = parser.Parse(packet) as ReadRemoteSupportedFeaturesCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x0b, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(0x877bffdbfecffebf, evt.LmpFeatures);
    }

    [Fact]
    public void ReadRemoteVersionInformationCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0xc, 0x8, 0x0, 0x3, 0x0, 0x9, 0xf, 0x0, 0x6, 0x21];

        var evt = parser.Parse(packet) as ReadRemoteVersionInformationCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x0c, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(0x09, evt.Version);
        Assert.Equal(0x000f, evt.CompanyIdentifier);
        Assert.Equal(8454, evt.Subversion);
    }

    [Fact(Skip = "TODO: EventCode=0x0D")]
    public void QoSSetupCompleteEventTest()
    {
    }

    [Fact]
    public void CommandCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0xe, 0x4, 0x1, 0x3, 0xc, 0x0];

        var evt = parser.Parse(packet) as CommandCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x0e, evt.EventCode.Value);
        Assert.Equal(1, evt.NumHciCommandPackets);
        Assert.Equal(0x0c03, evt.CommandOpcode);
        Assert.Equal([0], evt.ReturnParameters);
    }

    [Fact]
    public void CommandStatusEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0xf, 0x4, 0x0, 0x1, 0x43, 0x20];

        var evt = parser.Parse(packet) as CommandStatusEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x0f, evt.EventCode.Value);
        Assert.Equal(0, evt.Status);
        Assert.Equal(1, evt.NumHciCommandPackets);
        Assert.Equal(0x2043, evt.CommandOpcode);
    }

    [Fact(Skip = "TODO: EventCode=0x10")]
    public void HardwareErrorEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x11")]
    public void FlushOccurredEventTest()
    {
    }

    [Fact]
    public void RoleChangeEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x12, 0x8, 0x0, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x1];

        var evt = parser.Parse(packet) as RoleChangeEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x12, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal((ulong)0x10e4c2975044, evt.BdAddr);
        Assert.Equal(0x01, evt.NewRole);
    }

    [Fact]
    public void NumberOfCompletedPacketsEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x13, 0x5, 0x1, 0x3, 0x90, 0x1, 0x0];

        var evt = parser.Parse(packet) as NumberOfCompletedPacketsEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x13, evt.EventCode.Value);
        Assert.Equal(1, evt.NumHandles);
        Assert.Equal(0x9003, evt.Handles[0].ConnectionHandle);
        Assert.Equal(1, evt.Handles[0].NumCompletedPackets);
    }

    [Fact]
    public void ModeChangeEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x14, 0x6, 0x0, 0x3, 0x0, 0x2, 0x0, 0x3];

        var evt = parser.Parse(packet) as ModeChangeEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x14, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(0x02, evt.CurrentMode);
        Assert.Equal(768, evt.Interval);
    }

    [Fact(Skip = "TODO: EventCode=0x15")]
    public void ReturnLinkKeysEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x16")]
    public void PinCodeRequestEventTest()
    {
    }

    [Fact]
    public void LinkKeyRequestEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x17, 0x6, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10];

        var evt = parser.Parse(packet) as LinkKeyRequestEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x17, evt.EventCode.Value);
        Assert.Equal((ulong)0x10e4c2975044, evt.BdAddr);
    }

    [Fact(Skip = "TODO: EventCode=0x18")]
    public void LinkKeyNotificationEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x19")]
    public void LoopbackCommandEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x1A")]
    public void DataBufferOverflowEventTest()
    {
    }

    [Fact]
    public void MaxSlotsChangeEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x1b, 0x3, 0x3, 0x0, 0x5];

        var evt = parser.Parse(packet) as MaxSlotsChangeEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x1B, evt.EventCode.Value);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(5, evt.LmpMaxSlots);
    }

    [Fact]
    public void ReadClockOffsetCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x1c, 0x5, 0x0, 0x3, 0x0, 0x67, 0x1d];

        var evt = parser.Parse(packet) as ReadClockOffsetCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x1C, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(0x1d67, evt.ClockOffset);
    }

    [Fact]
    public void ConnectionPacketTypeChangedEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x1d, 0x5, 0x0, 0x3, 0x0, 0x18, 0xcc];

        var evt = parser.Parse(packet) as ConnectionPacketTypeChangedEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x1D, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(0xcc18, evt.PacketType);
    }

    [Fact(Skip = "TODO: EventCode=0x1E")]
    public void QoSViolationEventTest()
    {
    }

    [Fact]
    public void PageScanRepetitionModeChangeEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x20, 0x7, 0x44, 0x50, 0x97, 0xc2, 0xe4, 0x10, 0x0];

        var evt = parser.Parse(packet) as PageScanRepetitionModeChangeEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x20, evt.EventCode.Value);
        Assert.Equal((ulong)0x10e4c2975044, evt.BdAddr);
        Assert.Equal(0x00, evt.PageScanRepetitionMode);
    }

    [Fact(Skip = "TODO: EventCode=0x21")]
    public void FlowSpecificationCompleteEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x22")]
    public void InquiryResultWithRssiEventTest()
    {
    }

    [Fact]
    public void ReadRemoteExtendedFeaturesCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x23, 0xd, 0x0, 0x3, 0x0, 0x1, 0x2, 0xf, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];

        var evt = parser.Parse(packet) as ReadRemoteExtendedFeaturesCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x23, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(1, evt.PageNumber);
        Assert.Equal(2, evt.MaxPageNumber);
        Assert.Equal((ulong)0x0000000f, evt.ExtendedLmpFeatures);
    }

    [Fact(Skip = "TODO: EventCode=0x2C")]
    public void SynchronousConnectionCompleteEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x2D")]
    public void SynchronousConnectionChangedEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x2E")]
    public void SniffSubratingEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x2F")]
    public void ExtendedInquiryResultEventTest()
    {
    }

    [Fact]
    public void EncryptionKeyRefreshCompleteEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x30, 0x3, 0x0, 0x3, 0x0];

        var evt = parser.Parse(packet) as EncryptionKeyRefreshCompleteEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x30, evt.EventCode.Value);
        Assert.Equal(0x00, evt.Status);
        Assert.Equal(0x0003, evt.ConnectionHandle);
    }

    [Fact(Skip = "TODO: EventCode=0x31")]
    public void IoCapabilityRequestEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x32")]
    public void IoCapabilityResponseEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x33")]
    public void UserConfirmationRequestEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x34")]
    public void UserPasskeyRequestEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x35")]
    public void RemoteOobDataRequestEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x36")]
    public void SimplePairingCompleteEventTest()
    {
    }

    [Fact]
    public void LinkSupervisionTimeoutChangedEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x38, 0x4, 0x3, 0x0, 0x40, 0x1f];

        var evt = parser.Parse(packet) as LinkSupervisionTimeoutChangedEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x38, evt.EventCode.Value);
        Assert.Equal(0x0003, evt.ConnectionHandle);
        Assert.Equal(8000, evt.LinkSupervisionTimeout);
    }

    [Fact(Skip = "TODO: EventCode=0x39")]
    public void EnhancedFlushCompleteEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x3B")]
    public void UserPasskeyNotificationEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x3C")]
    public void KeypressNotificationEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x3D")]
    public void RemoteHostSupportedFeaturesNotificationEventTest()
    {
    }

    [Fact]
    public void LeMetaEventTest()
    {
        var parser = new HciParser();
        byte[] packet = [0x4, 0x3e, 0x1a, 0xd, 0x1, 0x1b, 0x0, 0x1, 0xe7, 0x12, 0x4d, 0x2b, 0x38, 0x61, 0x1, 0x0, 0xff, 0x7f, 0xda, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0];

        var evt = parser.Parse(packet) as LeMetaEvent;

        Assert.NotNull(evt);
        Assert.Equal(0x3E, evt.EventCode.Value);
        Assert.Equal(0x0d, evt.SubeventCode);
    }

    [Fact(Skip = "TODO: EventCode=0x48")]
    public void NumberOfCompletedDataBlocksEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x4E")]
    public void TriggeredClockCaptureEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x4F")]
    public void SynchronizationTrainCompleteEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x50")]
    public void SynchronizationTrainReceivedEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x51")]
    public void ConnectionlessPeripheralBroadcastReceiveEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x52")]
    public void ConnectionlessPeripheralBroadcastTimeoutEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x53")]
    public void TruncatedPageCompleteEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x54")]
    public void PeripheralPageResponseTimeoutEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x55")]
    public void ConnectionlessPeripheralBroadcastChannelMapChangeEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x56")]
    public void InquiryResponseNotificationEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x57")]
    public void AuthenticatedPayloadTimeoutExpiredEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x58")]
    public void SamStatusChangeEventTest()
    {
    }

    [Fact(Skip = "TODO: EventCode=0x59")]
    public void EncryptionChangeV2EventTest()
    {
    }
}
