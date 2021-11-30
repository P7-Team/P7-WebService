using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;

namespace WebService.Models
{
    public class BatchFile : IAggregateRoot<(string path, string filename)>
    {
        public string Path { get; set; }
        public string Filename { get; set; }
        public string OriginalExtension { get; set; }
        public string Encoding { get; set; }
        public Stream Data { get; set; }
        public int BatchId { get; set; }

        public BatchFile(string originalExtension, string encoding, Stream data, Batch batch)
        {
            OriginalExtension = originalExtension;
            Encoding = encoding;
            Data = data;
            BatchId = batch.Id;
        }

        // Same as above constructor, except this accepts an integer for the batch id, rather than extracting it from an instance of Batch
        public BatchFile(string originalExtension, string encoding, Stream data, int batchId)
        {
            OriginalExtension = originalExtension;
            Encoding = encoding;
            Data = data;
            BatchId = batchId;
        }

        /// <summary>
        /// Contructor used by the <see cref="BatchFileRepository"/> to convert the DB representation to a BatchFile object
        /// </summary>
        /// <param name="path">The path to where the BatchFile is stored on the filesystem</param>
        /// <param name="filename">The name of the BatchFile stored on the filesystem</param>
        /// <param name="encoding">The encoding used by the content of the file on the filesystem</param>
        /// <param name="includedIn">The ID of the Batch that the file is included in</param>
        public BatchFile(string path, string filename, string encoding, int includedIn)
        {
            Path = path;
            Filename = filename;
            Encoding = encoding;
            BatchId = includedIn;
        }

        public BatchFile()
        {
            // Left in for DB conversion...
        }

        public BatchFile WithPath(string path)
        {
            Path = path;
            return this;
        }

        public BatchFile WithFileName(string filename)
        {
            Filename = filename;
            return this;
        }

        public (string path, string filename) GetIdentifier()
        {
            return (Path, Filename);
        }

        /// <summary>
        /// Used to determine whether the given file can be used in interaction with the filesystem for fetch and delete operations
        /// </summary>
        /// <returns>true if valid, false otherwise</returns>
        public bool ValidForFileSystem()
        {
            return Path != null && Filename != null;
        }
    }
}
