using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Services
{
    public class BatchMarshaller
    {
        public static Batch MarshalBatch(Dictionary<string, string> formdata, List<FileStream> files, User owner)
        {
            return new Batch() { OwnerUsername = owner.Username };
        }
    }
}
