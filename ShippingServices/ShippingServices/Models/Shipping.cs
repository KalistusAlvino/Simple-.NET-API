using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.Models
{
    public class Shipping
    {
        public string? shippingid { get; set; }
        public string? shippingvendor { get; set; }
        public DateTime shippingdate { get; set; }
        public string? shippingstatus { get; set; }
        public int orderHeaderId { get; set; }
        public int beratbarang { get; set; }
        public decimal biayashipping { get; set; }
    }
}