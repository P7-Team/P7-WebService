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
            Assert.True(heartBeat.SetMessageType("Available"));
        }
        
        [Fact]
        public void heatBeatMessageType_Parses_With_LowerCase()
        {
            HeartBeat heartBeat = new HeartBeat("Working");
            Assert.True(heartBeat.SetMessageType("available"));
        }

        [Fact]
        public void heatBeatMessageType_Parses_With_Uppercase()
        {
            HeartBeat heartBeat = new HeartBeat("Working");
            Assert.True(heartBeat.SetMessageType("AVAILABLE"));
        }
        [Fact]
        public void heatBeatMessageType_Doesnt_Parse()
        {
            HeartBeat heartBeat = new HeartBeat("Working");
            Assert.False(heartBeat.SetMessageType("Test"));
        }

        [Fact]
        public void Heartbeat_Instantiation_throws_error()
        {
            Assert.Throws<Exception>(() => new HeartBeat("WrongInput"));
        }
    }
}