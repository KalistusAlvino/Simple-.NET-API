using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.DTO;
using WalletService.Models;

namespace WalletService.Interface
{
    public interface IWallet : ICrud<Wallet>
    {
        void UpdateSaldoAfterOrder(WalletUpdateSaldoDTO walletUpdateSaldoDTO);
        void UpdateSaldoAfterTopUp(WalletUpdateSaldoDTO walletUpdateSaldoDTO);
    }
}