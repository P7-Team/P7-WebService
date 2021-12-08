using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebService.Helper;
using WebService.Interfaces;
using WebService.Models;
using WebService.Models.DTOs;
using WebService.Services;
using WebService.Services.Repositories;
using Task = WebService.Models.Task;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskContext _context;
        private readonly IScheduler _scheduler;

        public TaskController(ITaskContext context, IScheduler scheduler)
        {
            _context = context;
            _scheduler = scheduler;
        }

        [HttpGet]
        [AuthenticationHelpers.Authorize]
        [Route("ready")]
        public IActionResult GetReadyTask([FromBody] ProviderDTO providerDto)
        {
            User user = providerDto.MapToUser();

            Task task = _scheduler.AllocateTask(user);
            if (task == null) return Ok();
            Dictionary<string, int> output = new Dictionary<string, int>
            {
                ["id"] = task.Id,
                ["number"] = task.Number,
                ["subNumber"] = task.SubNumber
            };
            return Ok(JsonConvert.SerializeObject(output));
        }

        [HttpPost]
        [AuthenticationHelpers.Authorize]
        [Route("complete")]
        public void AddResult([FromForm] ResultDTO resultInput)
        {
            Result result = resultInput.MapToResult();
            _context.SaveResult(result);
            _context.UpdateCompletedTask(result.TaskId, result.TaskNumber, result.TaskSubnumber);
        }
    }
}
