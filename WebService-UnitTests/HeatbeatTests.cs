using System;
using WebService.Models;
using Xunit;

namespace WebService_UnitTests
{
    public class HeatbeatTests
    {
        [Fact]
        public void heatBeatMessageType_Parses()
        {
            HeartBeat heartBeat = new HeartBeat("Working");
            heartBeat.SetMessageType("Available");
            Assert.Equal(MessageType.Available, heartBeat.GetMessageType());
        }

        [Fact]
        public void heatBeatMessageType_Parses_With_LowerCase()
        {
            HeartBeat heartBeat = new HeartBeat("Working");
            heartBeat.SetMessageType("available");
            Assert.Equal(MessageType.Available, heartBeat.GetMessageType());
        }

        [Fact]
        public void heatBeatMessageType_Parses_With_Uppercase()
        {
            HeartBeat heartBeat = new HeartBeat("Working");
            heartBeat.SetMessageType("AVAILABLE");
            Assert.Equal(MessageType.Available, heartBeat.GetMessageType());
        }

        [Fact]
        public void Heartbeat_Instantiation_makes_error_on_wrong_input()
        {
            HeartBeat heartBeat = new HeartBeat("Working");
            heartBeat.SetMessageType("Test");
            Assert.Equal(MessageType.Error, heartBeat.GetMessageType());
        }
    }
}