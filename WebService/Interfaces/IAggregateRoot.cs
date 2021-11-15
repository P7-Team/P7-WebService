using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    /***
     * The IAggregateRoot interface should be implemented by models that should be fetched 
     * from the database.
     * The type parameter T should be a type that can be used to identify the needed data 
     * in the database.
     * For models that use data from a single table in the DB, T could be an integer
     * representing the primary key.
     * For models that aggregate data from multiple tables, T could be a class containing 
     * the tables and corresponding keys for all the needed data.
     * 
     * The identifier is used by a repository to retrieve the data. 
     ***/
    public interface IAggregateRoot<T>
    {
        public T GetIdentifier();
    }
}
