using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebService.Models;
using WebService.Services;
using WebService.Services.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        BatchRepository repository;
        public BatchController(BatchRepository repository)
        {
            this.repository = repository;
        }

        // POST api/<BatchController>
        [HttpPost]
        public void Post()
        {
            string boundary = MultipartHelper.GetBoundary(HttpContext.Request.ContentType);
            SectionedDataReader reader = new SectionedDataReader(new MultipartReader(boundary, HttpContext.Request.Body));

            MultipartMarshaller<MultipartSection> batchMarshaller = new MultipartMarshaller<MultipartSection>(reader);

            Dictionary<string, string> formData = batchMarshaller.GetFormData();
            List<FileStream> streams = batchMarshaller.GetFileStreams();
            
            // TODO: Create an instance of the Batch model with the needed data, and use the BatchRepository to persist it to the DB
        }
    }
}
