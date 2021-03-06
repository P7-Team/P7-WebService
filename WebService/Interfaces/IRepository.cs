using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    /// <summary>
    /// A generic repository interface
    /// </summary>
    /// <typeparam name="T">the type of the objects that are manipulated by the repository.</typeparam>
    /// <typeparam name="K">the type that is used as an identifier for the objects or a type from which the the identifier can be derived.</typeparam>
    public interface IRepository<T, K> where T : IAggregateRoot<K>
    {
        public K Create(T item);
        public T Read(K identifier);
        public void Update(T item);
        public void Delete(K identifier);
    }
}
