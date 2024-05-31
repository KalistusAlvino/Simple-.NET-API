using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderServices.DTO;
using OrderServices.Models;

namespace OrderServices.Services
{
    public class WalletService : IWalletService
    {
        private HttpClient _httpClient;

        public WalletService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5047");
        }
        public async Task<Wallet> GetUserWalletById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/wallet/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                var wallet = JsonSerializer.Deserialize<Wallet>(results);
                if(wallet == null)
                {
                    throw new ArgumentException("Cannot get wallet");
                }
                return wallet;
            }
            else
            {
                throw new ArgumentException($"Cannot get wallet - httpstatus: {response.StatusCode}");
            }
        }

        public async Task UpdateSaldoWallet(WalletUpdateSaldoDTO walletUpdateSaldoDTO)
        {
            var json = JsonSerializer.Serialize(walletUpdateSaldoDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/wallet/updatesaldo", data);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Cannot update wallet saldo - httpstatus: {response.StatusCode}");
            }
        }
    }
}