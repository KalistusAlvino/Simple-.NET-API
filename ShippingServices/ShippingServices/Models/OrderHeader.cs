using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public string? username { get; set; }
        public DateTime OrderDate { get; set; }
    }
}