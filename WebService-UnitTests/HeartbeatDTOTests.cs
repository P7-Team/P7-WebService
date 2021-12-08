using System;
using System.Collections.Generic;
using System.Text;
using WebService.Models;
using WebService.Models.DTOs;
using Xunit;

namespace WebService_UnitTests
{
    public class HeartbeatDTOTests
    {
        [Fact]
        public void MapToHeartbeat_DTOHasStatus_SetsStatus()
        {
            HeartbeatDTO dto = new HeartbeatDTO() { Status = "error" };

            HeartBeat heartbeat = dto.MapToHeartbeat();

            Assert.Equal(MessageType.Error, heartbeat.GetMessageType());
        }
    }
}
