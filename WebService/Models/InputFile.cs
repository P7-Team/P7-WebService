using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using WebService.Services;

namespace WebService.Models
{
    public class InputFile : IAggregateRoot<(string path, string filename)>
    {
        public string Path { get; set; }

        public string Filename { get; set; }

        public string Encoding { get; set; }

        public int BatchId { get; set; }

        public (string path, string filename) GetIdentifier()
        {
            throw new NotImplementedException();
        }
    }
}
