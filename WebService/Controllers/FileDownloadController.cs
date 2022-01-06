using System.IO;
using Microsoft.AspNetCore.Mvc;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services.Repositories;

namespace WebService.Controllers
{
    public class FileDownloadController : ControllerBase
    {
        private readonly BatchFileRepository _batchFileRepository;
        private readonly IFileStore _fileStore;
        private readonly BatchRepository _batchRepository;

        public FileDownloadController(BatchFileRepository batchFileRepository, IFileStore fileStore,
            BatchRepository batchRepository)
        {
            _fileStore = fileStore;
            _batchRepository = batchRepository;
            _batchFileRepository = batchFileRepository;
        }

        [HttpGet]
        [AuthenticationHelpers.Authorize]
        [Route("api/[controller]/{fileID}")]
        public IActionResult FetchBatchResult(string fileID)
        {
            string path = _fileStore.Directory;
            BatchFile file = _batchFileRepository.Read((path, fileID));
            if (fileID.Contains("Result"))
            {
                Batch batch = _batchRepository.Read(file.BatchId);
                if (batch.OwnerUsername != (string) HttpContext.Items["User"]) return NotFound();
            }
            

            Stream fileData = _fileStore.FetchFile(file);

            if (fileData == null)
                return NotFound();

            return Ok(fileData);
        }
    }
}