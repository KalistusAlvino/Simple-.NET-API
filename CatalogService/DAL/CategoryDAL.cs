using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.DAL.Interfaces;
using CatalogService.Models;
using System.Data.SqlClient;

namespace CatalogService.DAL
{
    public class CategoryDAL : ICategory
    {
        private string GetConnectionString()
        {
            return "Data Source=KULIAH\\SQLEXPRESS01;Initial Catalog=CatalogDb;integrated security=True";
        }
        public void Delete(Category obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAll()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Categories order by CategoryName";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Category category = new Category();
                        category.CategoryID = Convert.ToInt32(dr["CategoryID"]);
                        category.CategoryName = dr["CategoryName"].ToString();
                        categories.Add(category);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();

                return categories;
            }
        }

        public Category GetById(int id)
        {
            Category category = new Category();
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Categories 
                            where CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CategoryID", id);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    category.CategoryID = Convert.ToInt32(dr["CategoryID"]);
                    category.CategoryName = dr["CategoryName"].ToString();

                }
                dr.Close();
                cmd.Dispose();
                conn.Close();

                return category;
            }
        }


        public void Insert(Category obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CategoryName", obj.CategoryName);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    if (result != 1)
                    {
                        throw new ArgumentException("Data gagal disimpan");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error : {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public void Update(Category obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Categories SET CategoryName = @CategoryName
                    WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CategoryName", obj.CategoryName);
                cmd.Parameters.AddWithValue("@CategoryID", obj.CategoryID);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    if (result != 1)
                    {
                        throw new ArgumentException("Data gagal diupdate");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error : {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Categories WHERE CategoryID = @CategoryID";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CategoryID", id);

                try
                {
                    conn.Open();
                    var result = cmd.ExecuteNonQuery();
                    if (result != 1)
                    {
                        throw new ArgumentException("Data gagal disimpan");
                    }
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error : {ex.Message}");
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public IEnumerable<Category> GetByName(string name)
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = $"SELECT * FROM Categories WHERE CategoryName LIKE '%' + @CategoryName + '%'";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@categoryName", name);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Category category = new Category();
                        category.CategoryID = Convert.ToInt32(dr["CategoryID"]);
                        category.CategoryName = dr["CategoryName"].ToString();
                        categories.Add(category);
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();

                return categories;
            }
        }
    }
}