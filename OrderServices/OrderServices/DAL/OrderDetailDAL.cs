using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.DAL.Interface;
using OrderServices.Models;
using System.Data.SqlClient;
using Dapper;

namespace OrderServices.DAL
{
    public class OrderDetailDAL : IOrderDetail
    {
        private readonly IConfiguration _config;

        public OrderDetailDAL(IConfiguration config) 
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");
        }
        public void Delete(OrderDetail obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM OrderDetails order by OrderHeaderId";
                return connection.Query<OrderDetail>(strSql);
            }
        }

        public OrderDetail GetById(int id)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM ORderDetails WHERE OrderDetailId = @OrderDetailId";
                return  connection.QueryFirstOrDefault<OrderDetail>(strSql, new {OrderDetailId = id});
            }
        }

        public OrderDetail Insert(OrderDetail obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO OrderDetails (OrderHeaderId, ProductId, Quantity, Price) VALUES (@OrderHeaderId, @ProductId, @Quantity, @Price);
                SELECT @@IDENTITY";
                var param = new { OrderHeaderId = obj.OrderHeaderId, ProductId = obj.ProductId, Quantity = obj.Quantity, Price = obj.Price };
                try 
                {
                    var id = connection.ExecuteScalar<int>(strSql, param);
                    obj.OrderDetailId = id;
                    return obj;
                }
                catch (SqlException ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }

        public OrderDetail Update(OrderDetail obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE OrderDetails SET OrderHeaderId = @OrderHeaderId, ProductId = @ProductId, Quantity = @Quantity, Price = @Price WHERE OrderDetailId = @OrderDetailId;";
                var param = new { OrderHeaderId = obj.OrderHeaderId, ProductId = obj.ProductId, Quantity = obj.Quantity, Price = obj.Price, OrderDetailId = obj.OrderDetailId };
                try
                {
                    var updatedOrderDetail = connection.QueryFirstOrDefault<OrderDetail>(strSql, param);
                    if (updatedOrderDetail == null)
                    {
                        throw new ArgumentException("Update gagal");
                    }
                    return updatedOrderDetail;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }
        }

    }
}