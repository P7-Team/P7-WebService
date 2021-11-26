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
using WebService.Interfaces;
using WebService.Models;
using WebService.Models.DTOs;
using WebService.Services;
using WebService.Services.Repositories;
using Task = WebService.Models.Task;

namespace WebService.Controllers
{
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskContext _context;

        public TaskController(ITaskContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("api/task/ready")]
        public Task GetReadyTask()
        {
            return _context.FirstOrDefault(k => k.IsReady);
        }

        [HttpPost]
        [Route("api/task/complete")]
        public void AddResult([FromForm] ResultDTO resultInput)
        {
            Result result = resultInput.MapToResult();
            _context.SaveResult(result);
        }
    }
}
