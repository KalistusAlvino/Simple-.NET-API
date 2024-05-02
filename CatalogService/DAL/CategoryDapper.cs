using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogService.DAL.Interfaces;
using System.Data.SqlClient;
using CatalogService.Models;
using Dapper;

namespace CatalogService.DAL
{
    public class CategoryDapper : ICategory
    {
         private string GetConnectionString()
        {
            return "Data Source=KULIAH\\SQLEXPRESS01;Initial Catalog=CatalogDb;integrated security=True";
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))     
            {
                var strSql = @"SELECT * FROM Categories order by CategoryName";
            }

        }    

        public Category GetById(int id)
        {
             using (SqlConnection conn = new SqlConnection(GetConnectionString()))     
            {

            }
        }

        public IEnumerable<Category> GetByName(string name)
        {
             using (SqlConnection conn = new SqlConnection(GetConnectionString()))     
            {
                 var strSql = $"SELECT * FROM Categories WHERE CategoryName LIKE @CategoryName";   
                 var param = new { CategoryName = $"%{name}%"};
                 var categories = conn.Query<Category>(strSql, param);
                 return categories;
            }
        }

        public void Insert(Category obj)
        {
            throw new NotImplementedException();
        }

        public void Update(Category obj)
        {
            throw new NotImplementedException();
        }
    }
}