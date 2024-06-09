using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.DTO;
using OrderServices.Models;

namespace OrderServices.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetUserWalletByUsername(string username);

        Task UpdateSaldoWallet(WalletUpdateSaldoDTO walletUpdateSaldoDTO);
    }
}