using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface ITaskContext : IList<Task>
    {
        public void SaveResult(Result result);
        /// <summary>
        /// Gets the <see cref="Batch"/> with the given ID
        /// </summary>
        /// <param name="batchId">The ID of the persisted <see cref="Batch"/></param>
        /// <returns>The <see cref="Batch"/> with the given ID</returns>
        public Batch GetBatch(int batchId);
    }
}