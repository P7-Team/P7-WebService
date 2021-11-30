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
        //public Task Task { get; set; }
        public int TaskId { get; set; }
        public int TaskNumber { get; set; }
        public int TaskSubnumber { get; set; }

        public Result(string originalExtension, string encoding, Stream data, Batch batch, bool verified, Task task)
            : base(originalExtension, encoding, data, batch)
        {
            Verified = verified;
            TaskId = task.Id;
            TaskNumber = task.Number;
            TaskSubnumber = task.SubNumber;
        }

        // Same as abpve constructor, except this accepts an integer containing the batch id rather than an instance of Batch
        public Result(string originalExtension, string encoding, Stream data, int batchId, bool verified, Task task)
            : base(originalExtension, encoding, data, batchId)
        {
            Verified = verified;
            TaskId = task.Id;
            TaskNumber = task.Number;
            TaskSubnumber = task.SubNumber;
        }

        public Result()
        {
            // Left empty for DB conversion...
        }

        public (string path, string filename) GetIdentifier()
        {
            return (Path, Filename);
        }
    }
}
