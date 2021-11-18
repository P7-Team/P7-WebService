using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IBatchService
    {
        public void SaveBatch(Batch batch);
        public void SaveFile(InputFile file);
    }
}
