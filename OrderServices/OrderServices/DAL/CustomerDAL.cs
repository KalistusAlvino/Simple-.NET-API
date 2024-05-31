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
    public class CustomerDAL : ICustomer
    {
        private readonly IConfiguration _config;
        public CustomerDAL(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");

        }
        public void Delete(Customer obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Customers WHERE CustomerId = @CustomerId";
                var param = new { CustomerId = obj.CustomerId };
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

        public IEnumerable<Customer> GetAll()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Customers order by CustomerName";
                return connection.Query<Customer>(strSql);
            }
        }

        public Customer GetById(int id)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Customers WHERE CustomerId = @CustomerId";
                return connection.QueryFirstOrDefault<Customer>(strSql, new { CustomerId = id });
            }
        }

        public Customer Insert(Customer obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Customers (CustomerName) VALUES (@CustomerName);
                SELECT @@IDENTITY;";
                try
                {
                    var newId = connection.ExecuteScalar<int>(strSql,
                        new { CustomerName = obj.CustomerName });
                    obj.CustomerId = newId;
                    return obj;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }

        }

        public Customer Update(Customer obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Customers SET CustomerName = @CustomerName WHERE CustomerId = @CustomerId;";
                var param = new { CustomerName = obj.CustomerName, CustomerId = obj.CustomerId };
                try
                {
                    var updatedCustomer = connection.QueryFirstOrDefault<Customer>(strSql, param);
                    if (updatedCustomer == null)
                    {
                        throw new ArgumentException("Update gagal dilakukan");
                    }
                    return updatedCustomer;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException(sqlEx.Message);
                }
            }
        }
    }
}