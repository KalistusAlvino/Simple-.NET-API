using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerServices.DTO;

namespace CustomerServices.Services
{
    public interface IWalletServices
    {
        Task PostWalletWhenRegister(CreateWalletDTO createWalletDTO);
    }
}