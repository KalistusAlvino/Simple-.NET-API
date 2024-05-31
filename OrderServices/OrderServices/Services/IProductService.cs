using OrderServices.DTO;
using OrderServices.Models;

namespace OrderServices.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product> GetProductById(int id);

        Task UpdateProductStock(ProductUpdateStockDTO productUpdateStockDTO);
    }
}