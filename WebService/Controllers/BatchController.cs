using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebService.Interfaces;
using WebService.Models;
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
        public void Post()
        {
            MultipartMarshaller<MultipartSection> batchMarshaller = createMarshaller(HttpContext.Request);
            
            Dictionary<string, string> formData = batchMarshaller.GetFormData();
            List<FileStream> streams = batchMarshaller.GetFileStreams();
            Batch batch = BatchMarshaller.MarshalBatch(formData, streams.Select(stream => (stream.Name, (Stream)stream)).ToList(), getUser());
            _store.Store(batch);
        }   

        private MultipartMarshaller<MultipartSection> createMarshaller(HttpRequest request)
        {
            string boundary = MultipartHelper.GetBoundary(request.ContentType);
            SectionedDataReader reader = new SectionedDataReader(new MultipartReader(boundary, request.Body));

            return new MultipartMarshaller<MultipartSection>(reader);
        }

        private User getUser()
        {
            return new User("fakeUser", "fakePassword");
        }
    }
}
