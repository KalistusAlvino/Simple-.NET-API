using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.DAL.Interface
{
    public interface ICrud<T>
    {
        void Insert(T obj);
        void UpdateStatus(T obj);
        IEnumerable<T> GetAll();
        T GetById(int id);

    }
}