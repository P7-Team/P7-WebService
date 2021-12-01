using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using SqlKata.Execution;
using SqlKata.Compilers;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class UserRepository : IRepository<User, string>
    {
        private readonly QueryFactory _db;
        private const string table = "Users";

        public UserRepository(QueryFactory db)
        {
            _db = db;
        }

        public string Create(User item)
        {
            _db.Query(table).InsertGetId<string>(new
            {
                username = item.Username,
                password = item.Password,
                points = item.ContributionPoints
            });

            return item.Username;
        }

        public void Delete(string identifier)
        {
            // The '=' operator is specified for clarity, even though it is the default operator
            _db.Query(table).Where("username", "=", identifier).Delete();
        }

        public User Read(string identifier)
        {
            // Select first user with matching username (it is assumed that there is only one)
            return _db.Query(table).Select("username as Username", "password as Password", "points as ContributionPoints")
                    .Where("username", identifier).First<User>();
        }

        public void Update(User item)
        {
            _db.Query(table).Where("username", item.GetIdentifier()).Update(new { password = item.Password, points = item.ContributionPoints });
        }
    }
}
