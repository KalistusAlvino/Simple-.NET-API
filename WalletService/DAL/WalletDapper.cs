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

        public void Insert(Wallet obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO UserWallet (username, saldo) VALUES (@username, @saldo)";
                var param = new { username = obj.username, saldo = obj.saldo };
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

            var insertSql = @"INSERT INTO pembayaran (username, paymentid, paymentwallet, saldo) 
                                         VALUES (@username, @paymentid, @paymentwallet, @saldo)";
            var updateSql = @"UPDATE UserWallet SET saldo = saldo - @saldo
                            WHERE username = @username";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var insertParam = new
                {
                    username = walletUpdateSaldoDTO.username,
                    paymentid = walletUpdateSaldoDTO.paymentid,
                    paymentwallet = walletUpdateSaldoDTO.paymentwallet,
                    saldo = walletUpdateSaldoDTO.saldo
                };
                var updateParam = new { username = walletUpdateSaldoDTO.username, saldo = walletUpdateSaldoDTO.saldo };
                try
                {
                    conn.Execute(insertSql, insertParam);
                    conn.Execute(updateSql, updateParam);
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

        public void UpdateSaldoAfterTopUp(WalletUpdateSaldoDTO walletUpdateSaldoDTO)
        {
            var insertSql = @"INSERT INTO topup (username, paymentid, paymentwallet, saldo) 
                                         VALUES (@username, @paymentid, @paymentwallet, @saldo)";
            var updateSql = @"UPDATE UserWallet SET saldo = saldo + @saldo
                            WHERE username = @username";
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var insertParam = new
                {
                    username = walletUpdateSaldoDTO.username,
                    paymentid = walletUpdateSaldoDTO.paymentid,
                    paymentwallet = walletUpdateSaldoDTO.paymentwallet,
                    saldo = walletUpdateSaldoDTO.saldo
                };
                var updateParam = new { username = walletUpdateSaldoDTO.username, saldo = walletUpdateSaldoDTO.saldo };
                try
                {
                    conn.Execute(insertSql, insertParam);
                    conn.Execute(updateSql, updateParam);
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

        public IEnumerable<Wallet> GetByUsername(string username)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM UserWallet
                            WHERE username = @username";
                var param = new { username = username };
                var user = conn.Query<Wallet>(strSql, param);
                return user;
            }
        }

        public IEnumerable<Wallet> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM UserWallet ORDER BY username";
                var wallet = conn.Query<Wallet>(strSql);
                return wallet;
            }
        }
    }
}