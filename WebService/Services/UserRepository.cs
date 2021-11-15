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
    public class UserRepository : IRepository<User, int>
    {
        private readonly QueryFactory _db;

        public UserRepository(QueryFactory db)
        {
            _db = db;
        }

        public void Create(User item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int identifier)
        {
            throw new NotImplementedException();
        }

        public User Read(int identifer)
        {
            throw new NotImplementedException();
        }

        public void Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
