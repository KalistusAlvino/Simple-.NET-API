using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.DTO;
using CatalogServices.Models;

namespace CatalogService.DAL.Interfaces
{
    public interface IProduct : ICrud<Product>
    {
       void UpdateStockAfterOrder(ProductUpdateStockDTO productUpdateStockDTO);
    }
}