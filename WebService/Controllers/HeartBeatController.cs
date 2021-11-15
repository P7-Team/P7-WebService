using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using WebService.Models;
using WebService.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

            return statusMessage.GetMessageType() switch
            {
                MessageType.Working => Ok(statusMessage.GetMessageType().ToString()),
                MessageType.Available => Ok(statusMessage.GetMessageType().ToString()),
                MessageType.Done => Ok(statusMessage.GetMessageType().ToString()),
                MessageType.IsJobDone => Ok(statusMessage.GetMessageType().ToString()),
                _ => BadRequest(
                    messageType["status is not valid, should be either: Working, Available, IsWorking or Done"])
            };
        }
    }
}