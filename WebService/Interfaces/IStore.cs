using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Interfaces
{
    public interface IStore<T>
    {
        public void Store(T item);
    }
}
