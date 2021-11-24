using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;

namespace WebService.Models
{
    public class Result : BatchFile
    {
        public bool Verified { get; set; }
        public Task Task { get; set; }

        public Result(string originalExtension, string encoding, Stream data, Batch batch, bool verified, Task task)
            : base(originalExtension, encoding, data, batch)
        {
            Verified = verified;
            Task = task;
        }

        public (string path, string filename) GetIdentifier()
        {
            return (Path, Filename);
        }
    }
}
