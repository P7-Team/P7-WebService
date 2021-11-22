using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebService.Models;
using WebService.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        public BatchController()
        {
        }

        // POST api/<BatchController>
        [HttpPost]
        public void Post()
        {
            string boundary = MultipartHelper.GetBoundary(HttpContext.Request.ContentType);
            SectionedDataReader reader =
                new SectionedDataReader(new MultipartReader(boundary, HttpContext.Request.Body));

            MultipartMarshaller<MultipartSection> batchMarshaller = new MultipartMarshaller<MultipartSection>(reader);

            Dictionary<string, string> formData = batchMarshaller.GetFormData();
            List<FileStream> streams = batchMarshaller.GetFileStreams();

        }

        [HttpGet]
        [Route("api/[controller]/status")]
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
        [Route("api/[controller]/result/{id:int}")]
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
            
            List<Stream> results = new List<Stream>();
            // TODO: Possible way to do it, ELSE USE STORAGE MANAGER (WHEN IT IS DONE)
            foreach (Tuple<string,string> pathAndFile in pathsAndFiles)
            {
                // Assumes that the filepath is stored with an ending directory separator
                string absPath = pathAndFile.Item1 + pathAndFile.Item2;
                Stream fileStream = System.IO.File.Open(absPath, FileMode.Open);
                results.Add(fileStream);
            }

            return Ok(results);
        }
    }
}
