using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services
{
    public class EligibleBatchesService : IEligibleBatchesService
    {
        private readonly IDBConnectionFactory _connectionFactory;
        public EligibleBatchesService(IDBConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Batch> GetEligibleBatches(int pointLimit)
        {
            MySqlConnection conn = _connectionFactory.GetConnection();
            IEnumerable<Batch> result = conn.Query<Batch>(@"SELECT *
                                            FROM Batch
                                            INNER JOIN Users U on Batch.ownedBy = U.username
                                            WHERE U.points >= @Points", new { Points = pointLimit });
            return result;
        }
    }
}
