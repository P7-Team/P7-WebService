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
            MultipartMarshaller<MultipartSection> batchMarshaller = createMarshaller(HttpContext.Request);

            Dictionary<string, string> formData = batchMarshaller.GetFormData();
            List<FileStream> streams = batchMarshaller.GetFileStreams();
            CreateBatch(formData, streams);
        }   

        private MultipartMarshaller<MultipartSection> createMarshaller(HttpRequest request)
        {
            string boundary = MultipartHelper.GetBoundary(request.ContentType);
            SectionedDataReader reader = new SectionedDataReader(new MultipartReader(boundary, request.Body));

            return new MultipartMarshaller<MultipartSection>(reader);
        }

        // TODO: move this functionality to BatchMarshaller
        private void CreateBatch(Dictionary<string, string> formData, List<FileStream> streams)
        {
            User user = new User("FakeUser", "Placeholder");
            Batch batch = new Batch();
            batch.OwnerUsername = user.Username;

            SourceFile sourceFile = new SourceFile(streams.Where(file => file.Name == "executable").First());
            batch.SourceFile = sourceFile;
            foreach (FileStream inputfile in streams.Where(file => file.Name != "executable"))
            {
                BatchFile file = new BatchFile(inputfile.Name, inputfile);
                file.Encoding = formData[inputfile.Name];
                batch.InputFiles.Add(file);
            }
            repository.Create(batch);
        }
    }
}
