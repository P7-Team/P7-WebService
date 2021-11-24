using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services.Repositories;

namespace WebService.Services.Stores
{
    public class FileStore : IFileStore
    {
        private readonly BatchFileRepository _batchFileRepository;
        private readonly ResultRepository _resultRepository;
        private readonly SourceFileRepository _sourceFileRepository;

        private IFileSaver _fileSaver;
        private IFileFetcher _fileFetcher;
        private IFileDeleter _fileDeleter;
        
        private string Directory { get; set; }

        public FileStore(BatchFileRepository batchFileRepository, ResultRepository resultRepository,
            SourceFileRepository sourceFileRepository, IFileSaver fileSaver, IFileFetcher fileFetcher,
            IFileDeleter fileDeleter, string fileDirectory)
        {
            _batchFileRepository = batchFileRepository;
            _resultRepository = resultRepository;
            _sourceFileRepository = sourceFileRepository;
            _fileSaver = fileSaver;
            _fileFetcher = fileFetcher;
            _fileDeleter = fileDeleter;

            Directory = fileDirectory;
            CreateDirectoryIfNotExists(Directory);
        }

        public void StoreFile(SourceFile sourceFile)
        {
            // Create path and filename
            string filename = ExtractFileName(sourceFile);
            sourceFile.WithPath(Directory).WithFileName(filename);
            
            // Store as BatchFile
            _batchFileRepository.Create(sourceFile);

            // Store as SourceFile
            _sourceFileRepository.Create(sourceFile);
            
            // Store in filesystem
            _fileSaver.Store(Directory, filename, sourceFile.OriginalExtension, sourceFile.Data);
        }

        public void StoreFile(Result resultFile)
        {
            // Create path and filename
            string filename = ExtractFileName(resultFile);
            resultFile.WithPath(Directory).WithFileName(filename);

            // Store as BatchFile
            _batchFileRepository.Create(resultFile);

            // Store as ResultFile
            _resultRepository.Create(resultFile);

            // Store in filesystem
            _fileSaver.Store(Directory, filename, resultFile.OriginalExtension, resultFile.Data);
        }

        public void StoreFiles(IEnumerable<BatchFile> batchFiles)
        {
            string[] filenames = ExtractFileNames(batchFiles);

            int counter = 0;
            foreach (BatchFile file in batchFiles)
            {
                string filename = filenames[counter];
                file.WithPath(Directory).WithFileName(filename);

                // Store as BatchFile
                _batchFileRepository.Create(file);

                // Store in filesystem
                _fileSaver.Store(Directory, filename, file.OriginalExtension, file.Data);
                
                // Count up
                counter++;
            }

        }

        private string[] ExtractFileNames(IEnumerable<BatchFile> files)
        {
            int postFixId = 1;
            string[] names = new string[files.Count()];
            foreach (BatchFile file in files)
            {
                names[postFixId - 1] = file.BatchId + "-" + postFixId;
                postFixId++;
            }
            return names;
        }

        private string ExtractFileName(Result resultFile)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(resultFile.BatchId);
            sb.Append("-");
            sb.Append(resultFile.Task.Id);
            sb.Append("-");
            sb.Append(resultFile.Task.SubNumber);
            return sb.ToString();
        }

        private string ExtractFileName(SourceFile sourceFile)
        {
            return sourceFile.BatchId.ToString();
        }

        /// <summary>
        /// Creates a directory at the given path if not already exist
        /// </summary>
        /// <param name="directoryPath">The path of the directory to create</param>
        /// <returns>The <see cref="DirectoryInfo"/> of the newly created directory, null if directory already existed</returns>
        private static DirectoryInfo CreateDirectoryIfNotExists(string directoryPath)
        {
            if (!System.IO.Directory.Exists(directoryPath))
            {
                return System.IO.Directory.CreateDirectory(directoryPath);
            }
            return null;
        }
    }
}