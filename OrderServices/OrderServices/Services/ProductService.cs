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
    public class ProductService : IProductService
    {
        private HttpClient _httpClient;

        //http client
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5062");
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync("/api/products");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(results);
                if (products == null) {
                    throw new ArgumentException("Cannot get product");
                }
                return products;
            }
            else
            {
                throw new ArgumentException($"Cannot get products - httpstatus: {response.StatusCode}");
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/products/{id}");
            if(response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                var product = JsonSerializer.Deserialize<Product>(results);
                if (product == null) 
                {
                    throw new ArgumentException("Cannot get product");
                }
                return product;
            }
            else
            {
                throw new ArgumentException($"Cannot get product - httpstatus: {response.StatusCode}");
            }
        }

        public async Task UpdateProductStock(ProductUpdateStockDTO productUpdateStockDTO)
        {
            var json = JsonSerializer.Serialize(productUpdateStockDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json"); 
            var response = await _httpClient.PutAsync("/api/products/updatestock", data);
            if(!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Cannot update product stock - httpstatus: {response.StatusCode}");
            }
        }
    }
}