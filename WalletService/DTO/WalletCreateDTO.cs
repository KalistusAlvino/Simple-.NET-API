using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.DTO
{
    public class WalletCreateDTO
    {
        public int userId { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? fullname { get; set; }
        public int saldo { get; set; }
    }
}