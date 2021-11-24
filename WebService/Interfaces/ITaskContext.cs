using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface ITaskContext : IList<Task>
    {
        public void SaveResult(Result result);
        public Batch GetBatch(int batchId);
    }
}