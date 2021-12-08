using Dapper;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
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
        private readonly QueryFactory _db;
        private readonly TaskRepository _taskRepository;
        private readonly SourceFileRepository _sourceFileRepository;
        private readonly BatchFileRepository _batchFileRepository;

        public EligibleBatchesService(IDBConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            var conn = connectionFactory.GetConnection();

            _db = connectionFactory.CreateQueryFactory(conn);
            _taskRepository = new TaskRepository(_db);
            _sourceFileRepository = new SourceFileRepository(_db);
            _batchFileRepository = new BatchFileRepository(_db);
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
                                                                FROM Batch b
                                                                INNER JOIN Users U on b.ownedBy = U.username
                                                                WHERE U.points >= @Points
                                                                 AND EXISTS (SELECT 1 FROM Task t WHERE t.finishedOn IS NULL AND t.id = b.id)",
                                                        new { Points = pointLimit });

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
