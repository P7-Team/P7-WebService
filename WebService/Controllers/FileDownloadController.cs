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
        
        public FileDownloadController(BatchFileRepository batchFileRepository, IFileStore fileStore)
        {
            _fileStore = fileStore;
            _batchFileRepository = batchFileRepository;
        }

        [HttpGet]
        [Route("api/[controller]/{fileID}")]
        public IActionResult FetchBatchResult(string fileID)
        {
            string path = _fileStore.Directory;
            BatchFile file = _batchFileRepository.Read((path, fileID));
            
            if (file == null)
            {
                return NotFound();
            }

            Stream fileData = _fileStore.FetchFile(file);

            if (fileData == null)
                return NotFound();
            
            return Ok(fileData);
        }
    }
}