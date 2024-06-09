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
    public class OrderHeaderDAL : IOrderHeader
    {
        private readonly IConfiguration _config;
        public OrderHeaderDAL(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");
        }
        public void Delete(OrderHeader obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM OrderHeaders WHERE OrderHeaderId = @OrderHeaderId";
                var param = new { OrderHeaderId = obj.OrderHeaderId };
                try
                {
                    connection.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }
        }

        public IEnumerable<OrderHeader> GetAll()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM OrderHeaders";
                return connection.Query<OrderHeader>(strSql);
            }
        }

        public OrderHeader GetById(int id)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Orderheaders WHERE OrderHeaderId = @OrderHeaderId";
                return connection.QueryFirstOrDefault<OrderHeader>(strSql, new { OrderHeaderId = id });
            }
        }

        public OrderHeader Insert(OrderHeader obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO OrderHeaders (username, OrderDate) VALUES (@username, @OrderDate);
                SELECT @@IDENTITY";
                var param = new { username = obj.username, OrderDate = DateTime.Now };
                try
                {
                    var id = connection.ExecuteScalar<int>(strSql, param);
                    obj.OrderHeaderId = id;
                    return obj;
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

        public OrderHeader Update(OrderHeader obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE OrderHeaders SET username = @username, OrderDate = @OrderDate WHERE OrderHeaderId = @OrderHeaderId;";
                var param = new { username = obj.username, OrderDate = obj.OrderDate, OrderHeaderId = obj.OrderHeaderId };
                try
                {
                    var updatedOrderHeader = connection.QueryFirstOrDefault<OrderHeader>(strSql, param);
                    if (updatedOrderHeader == null)
                    {
                        throw new ArgumentException("Update gagal dilakukan");
                    }
                    return updatedOrderHeader;
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

    }
}