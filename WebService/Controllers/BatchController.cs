using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
            SectionedDataReader reader = new SectionedDataReader(new MultipartReader(boundary, HttpContext.Request.Body));

            MultipartMarshaller<MultipartSection> batchMarshaller = new MultipartMarshaller<MultipartSection>(reader);

            Dictionary<string, string> fromData = batchMarshaller.GetFormData();
            List<FileStream> streams = batchMarshaller.GetFileStreams();

        }
    }
}
