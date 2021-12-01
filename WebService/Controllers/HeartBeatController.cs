using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebService.Models;
using WebService.Models.DTOs;
using WebService.Services;


namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartBeatController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] HeartbeatDTO heartbeatInput)
        {
            HeartBeat heartbeat = heartbeatInput.MapToHeartbeat();

            switch (heartbeat.GetMessageType())
            {
                case MessageType.Working:
                case MessageType.Available:
                case MessageType.IsJobDone:
                    return Ok(heartbeat.GetMessageType().ToString());
                case MessageType.Done:
                    
                    return Ok(heartbeat.GetMessageType().ToString());
                default:
                    return BadRequest("status is not valid, should be either: Working, Available, IsWorking or Done");
            }
        }
    }
}