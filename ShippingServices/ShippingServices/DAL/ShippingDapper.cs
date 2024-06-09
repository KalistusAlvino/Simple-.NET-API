using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ShippingServices.DAL.Interface;
using ShippingServices.Models;

namespace ShippingServices.DAL
{
    public class ShippingDapper : IShipping
    {
        private readonly IConfiguration _config;
        public ShippingDapper(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");

        }
        public IEnumerable<Shipping> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM [shipping]";
                var ship = conn.Query<Shipping>(strSql);
                return ship;
            }
        }

        public Shipping GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM shipping
                            WHERE shippingid = @shippingid";
                var param = new { shippingid = id };
                var shipping = conn.QueryFirstOrDefault<Shipping>(strSql, param);
                if (shipping == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return shipping;
            }
        }

        public void Insert(Shipping obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO shipping (shippingid, shippingvendor, shippingdate, shippingstatus, orderHeaderId, beratbarang, biayashipping) 
                VALUES (@shippingid, @shippingvendor, @shippingdate, @shippingstatus, @orderHeaderId, @beratBarang, @biayashipping)";
                var param = new
                {
                    shippingid = obj.shippingid,
                    shippingvendor = obj.shippingvendor,
                    shippingdate = obj.shippingdate,
                    shippingstatus = obj.shippingstatus,
                    orderHeaderId = obj.orderHeaderId,
                    beratbarang = obj.beratbarang,
                    biayashipping = obj.biayashipping
                };
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

        public void UpdateStatus(Shipping obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE shipping SET shippingstatus = @shippingstatus WHERE shippingid = @shippingid";

                var param = new { shippingstatus = obj.shippingstatus, shippingid = obj.shippingid  };
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