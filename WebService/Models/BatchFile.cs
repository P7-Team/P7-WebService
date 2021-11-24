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
