using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CustomerServices.Models;
using DAL.Interface;
using Dapper;

namespace CustomerServices.DAL
{
    public class CustomerDapper : ICustomer
    {
        private readonly IConfiguration _config;
        public CustomerDapper(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");

        }

        public IEnumerable<Customer> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM [user]";
                var cus = conn.Query<Customer>(strSql);
                return cus;
            }
        }

        public IEnumerable<Customer> GetByUsername(string username)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM [user]
                            WHERE username LIKE @username";
                var param = new { username = $"%{username}%" };
                var user = conn.Query<Customer>(strSql, param);
                return user;
            }
        }

        public void Register(Customer obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO [user] (username, password, fullname) VALUES (@username, @password, @fullname)";
                var param = new { username = obj.username, password = obj.password, fullname =  obj.fullname };
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
    }
}