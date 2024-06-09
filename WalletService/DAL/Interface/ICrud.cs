using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Interface
{
    public interface ICrud<T>
    {
        IEnumerable<T> GetAll();
        IEnumerable<Wallet> GetByUsername(string username);
        void Insert(T obj);
    }
}