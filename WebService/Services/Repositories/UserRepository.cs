using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using SqlKata.Execution;
using SqlKata.Compilers;

namespace WebService.Services
{
    public class UserRepository : IRepository<User, string>
    {
        private readonly QueryFactory _db;
        private const string table = "Users";

        public UserRepository(QueryFactory db)
        {
            _db = db;
        }

        public void Create(User item)
        {
            _db.Query(table).Insert(new
            {
                username = item.Username,
                password = item.Password,
                points = item.ContributionPoints
            });
        }

        public void Delete(string identifier)
        {
            // The '=' operator is specified for clarity, even though it is the default operator
            _db.Query(table).Where("username", "=", identifier).Delete();
        }

        public User Read(string identifer)
        {
            // Select first user with matching username (it is assumed that there is only one)
            return _db.Query(table).Where("username", identifer).First();
        }

        public void Update(User item)
        {
            _db.Query(table).Where("username", item.GetIdentifier()).Update(new { password = item.Password, points = item.ContributionPoints });
        }
    }
}
