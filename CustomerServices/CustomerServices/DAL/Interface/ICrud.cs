using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICrud<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByUsername(string username);
        void Register(T obj);
    }
}