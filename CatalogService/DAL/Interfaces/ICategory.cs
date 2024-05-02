using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.Models;

namespace CatalogService.DAL.Interfaces
{
    public interface ICategory : ICrud<Category>

    {
        void Delete(int id);
        IEnumerable<Category> GetByName(string name);
    }
}