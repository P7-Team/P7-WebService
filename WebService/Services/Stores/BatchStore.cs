using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;
using WebService.Services;
using WebService.Interfaces;

namespace WebService.Services.Stores
{
    public class BatchStore
    {
        private readonly IRepository<Batch, int> _batchRepository;

        public BatchStore(IRepository<Batch, int> batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public void Store(Batch batch)
        {
            _batchRepository.Create(batch);
        }
    }
}
