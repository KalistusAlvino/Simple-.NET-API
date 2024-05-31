using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.DTO
{
    public class ProductUpdateStockDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}