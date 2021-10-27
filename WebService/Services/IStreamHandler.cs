using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Services
{
    public interface IStreamHandler
    {
        public void HandleStream(Stream stream);
    }
}
