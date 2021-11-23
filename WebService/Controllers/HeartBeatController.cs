using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebService.Models;
using WebService.Services;


namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartBeatController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(IDictionary<string, string> messageType)
        {
            HeartBeat statusMessage = new HeartBeat(messageType["status"]);

            switch (statusMessage.GetMessageType())
            {
                case MessageType.Working:
                case MessageType.Available:
                case MessageType.IsJobDone:
                    return Ok(statusMessage.GetMessageType().ToString());
                case MessageType.Done:
                    
                    return Ok(statusMessage.GetMessageType().ToString());
                default:
                    return BadRequest(
                        messageType["status is not valid, should be either: Working, Available, IsWorking or Done"]);
            }
        }
    }
}