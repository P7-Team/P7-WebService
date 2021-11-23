using WebService.Interfaces;
using WebService.Models;
using WebService.Services.Repositories;

namespace WebService.Services.Stores
{
    public class FileStore
    {
        private readonly BatchFileRepository _batchFileRepository;
        private readonly ResultRepository _resultRepository;
        private readonly SourceFileRepository _sourceFileRepository;

        private IFileSaver _fileSaver;
        private IFileFetcher _fileFetcher;
        private IFileDeleter _fileDeleter;

        private FileStore(BatchFileRepository batchFileRepository, ResultRepository resultRepository,
            SourceFileRepository sourceFileRepository, IFileSaver fileSaver, IFileFetcher fileFetcher,
            IFileDeleter fileDeleter)
        {
            _batchFileRepository = batchFileRepository;
            _resultRepository = resultRepository;
            _sourceFileRepository = sourceFileRepository;
            _fileSaver = fileSaver;
            _fileFetcher = fileFetcher;
            _fileDeleter = fileDeleter;
        }

        public void StoreFile(SourceFile sourceFile)
        {
            // Create path and filename
            // Store as BatchFile
            // Store as SourceFile
        }

        public void StoreFile(Result resultFile)
        {
            // Create path and filename
            // Store as BatchFile
            // Store as ResultFile            
        }

        private void StoreFile(BatchFile batchFile)
        {
            // Store as BatchFile
            // Store as SourceFile            
        }

    }
}