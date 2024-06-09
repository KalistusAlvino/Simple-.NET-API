using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingServices.DTO;
using ShippingServices.Models;

namespace ShippingServices.Services
{
    public interface IWalletServices
    {
        Task<Wallet> GetUserWalletByUsername(string username);

        Task UpdateSaldoWallet(WalletUpdateSaldoDTO walletUpdateSaldoDTO);
    }
}