using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CustomerServices.DTO;

namespace CustomerServices.Services
{
    public class WalletService : IWalletServices
    {
        private HttpClient _httpClient;
        public WalletService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5047");
        }

        public async Task PostWalletWhenRegister(CreateWalletDTO createWalletDTO)
        {
            var json = JsonSerializer.Serialize(createWalletDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/wallet", data);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Gagal Register Wallet User - httpstatus: {response.StatusCode}");
            }
        }
    }
}