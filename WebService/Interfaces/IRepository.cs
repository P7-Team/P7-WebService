using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    /***
     * A generic repository interface
     * Type T is the type of the objects that are manipulated by the repository.
     * Type K is the type that is used as an identifier for the objects or a type 
     * from which the the identifier can be derived.
     **/
    public interface IRepository<T, K> where T : IAggregateRoot<K>
    {
        public void Create(T item);
        public T Read(K identifier);
        public void Update(T item);
        public void Delete(K identifier);
    }
}
