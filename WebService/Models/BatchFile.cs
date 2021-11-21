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
        public string Encoding { get; set; }
        public FileStream Data { get; set; }
        public int BatchId { get; set; }

        public BatchFile(string filename, FileStream data)
        {
            Filename = filename;
            Data = data;
        }

        public (string path, string filename) GetIdentifier()
        {
            return (Path, Filename);
        }
    }
}
