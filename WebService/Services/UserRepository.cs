using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services
{
    public class UserRepository : IRepository<User, int>
    {
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
