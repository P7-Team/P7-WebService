using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        public BatchController(BatchStore store)
        {
            _store = store;
        }

        // POST api/<BatchController>
        [HttpPost]
        public void Post([FromForm] BatchDTO batchInput)
        {
            Batch batch = batchInput.MapToBatch();
            //TODO: set OwnerUsername property of the batch to a real user
            batch.OwnerUsername = getUser().Username;
            _store.Store(batch);
        }

        private User getUser()
        {
            return new User("fakeUser", "fakePassword");
        }

        [HttpGet]
        [Route("status")]
        public IActionResult GetBatchStatus()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString();
            string user = new TokenStore().Fetch(token);
            if (user == String.Empty)
            {
                return BadRequest();
            }

            // TODO: Lookup batches for the user and get the status information
            List<BatchStatus> batchStatus = new List<BatchStatus>();

            return Ok(batchStatus);
        }

        [HttpGet]
        [Route("result/{id:int}")]
        public IActionResult FetchBatchResult(int id)
        {
            // TODO: Lookup the batch with {id} to check for existence
            bool batchExists = true;
            if (!batchExists)
            {
                return NotFound();
            }

            // TODO: Lookup the filepath and filenames for all result files : Tuple(path, filename)
            List<Tuple<string, string>> pathsAndFiles = new List<Tuple<string, string>>();

            Dictionary<string, Stream> files = new Dictionary<string, Stream>();
            // TODO: Possible way to do it, ELSE USE STORAGE MANAGER (WHEN IT IS DONE)

            foreach (Tuple<string,string> pathAndFile in pathsAndFiles)
            {
                // Assumes that the filepath is stored with an ending directory separator
                string absPath = pathAndFile.Item1 + pathAndFile.Item2;
                Stream fileStream = System.IO.File.Open(absPath, FileMode.Open);
                files.Add(pathAndFile.Item2, fileStream);
            }

            MultipartFormDataContent content = MultipartFormDataHelper.CreateContent(new Dictionary<string, string>(), files);
            return Ok(content);
        }
    }
}
