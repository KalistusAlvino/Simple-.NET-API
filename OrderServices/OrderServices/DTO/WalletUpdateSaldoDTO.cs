using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.DTO
{
    public class WalletUpdateSaldoDTO
    {
        public int userId { get; set; }
        public decimal saldo { get; set; }
    }
}