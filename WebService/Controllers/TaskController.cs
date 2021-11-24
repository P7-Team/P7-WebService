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
        public void AddResult()
        {
            string boundary = MultipartHelper.GetBoundary(HttpContext.Request.ContentType);
            SectionedDataReader reader =
                new SectionedDataReader(new MultipartReader(boundary, HttpContext.Request.Body));

            MultipartMarshaller<MultipartSection> marshaller = new MultipartMarshaller<MultipartSection>(reader);

            Dictionary<string, string> formData = marshaller.GetFormData();
            List<FileStream> fileStreams = marshaller.GetFileStreams();

            CompletedTask completedTask = new CompletedTask(long.Parse(formData["id"]), fileStreams[0].Name);

            Task task = new Task(int.Parse(formData["batchId"]), int.Parse(formData["taskNumber"]), int.Parse(formData["taskSubNumber"]));
            
            Batch batch = _context.GetBatch((int)task.Id);
            Result result = new Result(String.Empty, Encoding.UTF8.EncodingName, fileStreams[0], batch, false, task);
            
            _context.SaveResult(result);
        }
    }
}
