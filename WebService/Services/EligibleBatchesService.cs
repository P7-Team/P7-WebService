using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services.Repositories;

namespace WebService.Services
{
    public class EligibleBatchesService : IEligibleBatchesService
    {
        private readonly IDBConnectionFactory _connectionFactory;
        private readonly TaskRepository _taskRepository;
        private readonly SourceFileRepository _sourceFileRepository;
        private readonly BatchFileRepository _batchFileRepository;

        public EligibleBatchesService(IDBConnectionFactory connectionFactory, TaskRepository taskRepository, SourceFileRepository sourceFileRepository, BatchFileRepository batchFileRepository)
        {
            _connectionFactory = connectionFactory;
            _taskRepository = taskRepository;
            _sourceFileRepository = sourceFileRepository;
            _batchFileRepository = batchFileRepository;
        }

        /// <summary>
        /// Get all batches belonging to users with sufficient points
        /// </summary>
        /// <param name="pointLimit"></param>
        /// <returns>IEnumerable<Batch> containing batches that should be scheduled</returns>
        public IEnumerable<Batch> GetEligibleBatches(int pointLimit)
        {
            MySqlConnection conn = _connectionFactory.GetConnection();
            IEnumerable<Batch> result = conn.Query<Batch>(@"SELECT *
                                            FROM Batch
                                            INNER JOIN Users U on Batch.ownedBy = U.username
                                            WHERE U.points >= @Points", new { Points = pointLimit });

            foreach (Batch batch in result)
            {
                batch.SourceFile = _sourceFileRepository.Read(batch.Id);
                batch.Tasks = _taskRepository.Read(batch.Id);
                foreach (Task task in batch.Tasks)
                {
                    task.Executable = batch.SourceFile;
                    task.Input = _batchFileRepository.Read(task.Id, task.Number, task.SubNumber);
                }
            }
            return result;
        }
    }
}
