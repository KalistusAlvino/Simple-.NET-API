using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.DTO
{
    public class ProductUpdateStockDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}