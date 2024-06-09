using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ShippingServices.Models;

namespace ShippingServices.Services
{
    public class OrderHeaderServices : IOrderHeaderServices
    {
        private HttpClient _httpClient;
        public OrderHeaderServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5265");
        }

        public async Task<OrderHeader> GetOrderHeaderById(int id)
        {
            var response = await _httpClient.GetAsync($"/orderHeader/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                var order = JsonSerializer.Deserialize<OrderHeader>(results);
                if (order == null)
                {
                    throw new ArgumentException("Cannot get OrderHeader");
                }
                return order;
            }
            else
            {
                throw new ArgumentException($"Cannot get OrderHeader - httpstatus: {response.StatusCode}");
            }
        }
    }
}