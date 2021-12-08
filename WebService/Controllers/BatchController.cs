using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using WebService.Interfaces;
using WebService.Models;
using WebService.Models.DTOs;
using WebService.Services;
using WebService.Services.Repositories;
using WebService.Services.Stores;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        BatchStore _store;
        private BatchRepository _batchRepository;
        private TaskRepository _taskRepository;
        private ResultRepository _resultRepository;
        private IFileStore _fileStore;
        private BatchFileRepository _batchFileRepository;

        public BatchController(BatchStore store, BatchRepository batchRepository, TaskRepository taskRepository,
            ResultRepository resultRepository, BatchFileRepository batchFileRepository, IFileStore fileStore)
        {
            _store = store;
            _batchRepository = batchRepository;
            _taskRepository = taskRepository;
            _resultRepository = resultRepository;
            _fileStore = fileStore;
            _batchFileRepository = batchFileRepository;
        }

        // POST api/<BatchController>
        [HttpPost]
        [AuthenticationHelpers.Authorize]
        public void Post([FromForm] BatchDTO batchInput)
        {
            Batch batch = batchInput.MapToBatch();
            //TODO: set OwnerUsername property of the batch to a real user
            batch.OwnerUsername = getUser();
            _store.Store(batch);
        }

        private string getUser()
        {
            return HttpContext.Items["User"].ToString();
        }

        [HttpGet]
        [Route("status")]
        [AuthenticationHelpers.Authorize]
        public IActionResult GetBatchStatus()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString();
            string user = new TokenStore().Fetch(token);
            if (user == String.Empty)
            {
                return BadRequest();
            }

            List<Batch> userBatches = _batchRepository.Read(user);
            List<BatchStatus> statusList = new List<BatchStatus>();
            foreach (Batch batch in userBatches)
            {
                List<Task> tasks = _taskRepository.Read(batch.Id);
                int totalTasks = tasks.Count;
                int finishedTask = tasks.Where(t => t.FinishedOn != null).Count();
                bool batchFinished = totalTasks == finishedTask;
                BatchStatus status = new BatchStatus(batch.Id, batchFinished, finishedTask, totalTasks);

                foreach (Task task in tasks)
                {
                    if (task.FinishedOn == null) continue;

                    Result taskResult = _resultRepository.Read((task.Id, task.Number, task.SubNumber));
                    if (taskResult != null)
                        status.AddFile(taskResult.Filename);
                }

                statusList.Add(status);
            }

            return Ok(System.Text.Json.JsonSerializer.Serialize(statusList));
        }
    }
}