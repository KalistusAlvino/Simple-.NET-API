using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerServices.DTO
{
    public class CreateWalletDTO
    {
        public string? username { get; set; }
        public decimal saldo { get; set; }
    }
}