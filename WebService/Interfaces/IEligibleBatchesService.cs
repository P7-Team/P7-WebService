using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IEligibleBatchesService
    {
        public IEnumerable<Batch> GetEligibleBatches(int pointLimit);
    }
}
