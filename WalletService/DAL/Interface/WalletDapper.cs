using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WalletService.DTO;
using WalletService.Interface;
using WalletService.Models;

namespace WalletService.DAL.Interface
{
    public class WalletDapper : IWallet
    {
        private readonly IConfiguration _config;
        public WalletDapper(IConfiguration config)
        {
            _config = config;
        }
        private string GetConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");

        }
        public Wallet GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM UserWallet
                            WHERE userId = @UserId";
                var param = new { UserId = id };
                var wallet = conn.QueryFirstOrDefault<Wallet>(strSql, param);
                if (wallet == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return wallet;
            }
        }

        public void Insert(Wallet obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO UserWallet (username, password, fullname, saldo) VALUES (@username, @password, @fullname, @saldo)";
                var param = new {username = obj.username, password = obj.password,fullname = obj.fullname, saldo = obj.saldo };
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

        public void UpdateSaldoAfterOrder(WalletUpdateSaldoDTO walletUpdateSaldoDTO)
        {
            var strSql = @"UPDATE UserWallet SET saldo = saldo - @saldo
                            WHERE userId = @userId";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var param = new { userId = walletUpdateSaldoDTO.userId, saldo = walletUpdateSaldoDTO.saldo };
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