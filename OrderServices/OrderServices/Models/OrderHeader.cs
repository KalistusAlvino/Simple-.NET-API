using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public int userId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}