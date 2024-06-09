using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.DTO
{
    public class WalletUpdateSaldoDTO
    {
        public string? username { get; set; }
        public string? paymentid { get; set; }
        public string? paymentwallet { get; set; }
        public decimal saldo { get; set; }
    }
}