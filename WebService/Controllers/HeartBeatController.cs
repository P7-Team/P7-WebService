using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;
using WebService.Models;
using WebService.Models.DTOs;
using WebService.Services;


namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartBeatController : ControllerBase
    {
        private readonly ISchedulerWorkedOnHelper _SchedulerWorkedOnHelper;

        public HeartBeatController(ISchedulerWorkedOnHelper schedulerWorkedOnHelper)
        {
            _SchedulerWorkedOnHelper = schedulerWorkedOnHelper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] HeartbeatDTO heartbeatInput)
        {
            HeartBeat heartbeat = heartbeatInput.MapToHeartbeat();

            switch (heartbeat.GetMessageType())
            {
                case MessageType.Working:
                    _SchedulerWorkedOnHelper.UpdateLastPing(heartbeatInput.Provider, DateTime.Now);
                    return Ok();
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