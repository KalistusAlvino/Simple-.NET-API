using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.DTO
{
    public class WalletCreateDTO
    {
        public string? username { get; set; }
        public int saldo { get; set; }
    }
}