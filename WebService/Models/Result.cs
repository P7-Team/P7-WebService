using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;

namespace WebService.Models
{
    public class Result : IAggregateRoot<(string path, string filename)>
    {
        public string Path { get; set; }
        public string Filename { get; set; }
        public bool Verified { get; set; }
        public Task Task { get; set; }

        public Result(string path, string filename, bool verified, Task task)
        {
            Path = path;
            Filename = filename;
            Verified = verified;
            Task = task;
        }

        public (string path, string filename) GetIdentifier()
        {
            return (Path, Filename);
        }
    }
}
