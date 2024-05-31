
using System.Data.SqlClient;
using CatalogService.DAL.Interfaces;
using CatalogService.DTO;
using CatalogServices.Models;
using Dapper;

namespace CatalogService.DAL
{
    public class ProductDapper : IProduct
    {
        private readonly IConfiguration _config;
        public ProductDapper(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");
          
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Products 
                               WHERE ProductID = @ProductID";
                var param = new { ProductID = id };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public IEnumerable<Product> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT c.CategoryName, p.CategoryID, p.Name, p.Description, p.Price, p.Quantity
                        FROM Products p
                        JOIN Categories c ON p.CategoryID = c.CategoryID";
                var products = conn.Query<Product>(strSql);
                return products;
            }
        }

        public Product GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Products
                            WHERE ProductID = @ProductID";
                var param = new { ProductID = id };
                var product = conn.QueryFirstOrDefault<Product>(strSql, param);
                if (product == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return product;
            }
        }

        public IEnumerable<Product> GetByName(string name)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Products
                            WHERE Name LIKE @Name";
                var param = new { Name = $"%{name}%" };
                var products = conn.Query<Product>(strSql, param);
                return products;
            }
        }

        public void Insert(Product obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Products (CategoryID, Name, Description, Price, Quantity) VALUES (@CategoryID, @Name, @Description, @Price, @Quantity)";
                var param = new { CategoryID = obj.CategoryID, Name = obj.Name, Description = obj.Description, Price = obj.Price, Quantity = obj.Quantity };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public void Update(Product obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Products SET CategoryID = @CategoryID, Name = @Name, Description = @Description, Price = @Price, Quantity = @Quantity WHERE ProductID = @ProductID";

                var param = new { CategoryID = obj.CategoryID, Name = obj.Name, Description = obj.Description, Price = obj.Price, Quantity = obj.Quantity, ProductID = obj.ProductID };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public void UpdateStockAfterOrder(ProductUpdateStockDTO productUpdateStockDTO)
        {
            var strSql = @"UPDATE Products SET Quantity = Quantity - @Quantity
                            WHERE ProductID = @ProductID";
            using (SqlConnection conn = new SqlConnection(GetConnectionString())) {
                var param = new { ProductId = productUpdateStockDTO.ProductId, Quantity = productUpdateStockDTO.Quantity };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch(SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch(Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }
    }
}