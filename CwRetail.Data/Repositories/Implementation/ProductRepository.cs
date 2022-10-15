using CwRetail.Data.Extensions;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace CwRetail.Data.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnection _connection;

        public ProductRepository()
        {
            _connection = new SqlConnection(ConnectionStrings.Test);
        }

        public IEnumerable<Product> Get()
        {
            try
            {
                _connection.Open();

                string sql = $@"SELECT 
                                        p.Id,
	                                    p.Name, 
	                                    p.Price, 
	                                    p.Type, 
	                                    p.Active
                                    FROM 
	                                    production.products p";
                var result = _connection.Query<Product>(sql);

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return new List<Product>();
            }
        }

        public int Insert(Product product)
        {
            try
            {
                _connection.Open();

                string sql = $@"INSERT INTO production.products
                                    (                    
	                                    Name,
	                                    Price,
	                                    Type,
	                                    Active
                                    )
                                    VALUES 
                                    ( 
	                                    @Name,
	                                    @Price,
	                                    @Type,
	                                    @Active
                                    )";
                var result = _connection.Execute(sql, new
                {
                    Name = product.Name,
                    Price = product.Price,
                    Type = product.Type.ToString(),
                    Active = product.Active
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int Update(long id, string product)
        {
            try
            {
                string updateSql = product.AsUpdateSql();

                if (string.IsNullOrWhiteSpace(updateSql))
                {
                    return 0;
                }

                _connection.Open();

                string sql = $@"UPDATE
	                                production.products
                                SET
	                                {updateSql}
                                WHERE
                                    Id = @Id";
                var result = _connection.Execute(sql, new
                {
                    Id = id
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int Delete(long id)
        {
            try
            {
                _connection.Open();

                string sql = $@"DELETE FROM
	                                production.products
                                WHERE
	                                Id = @Id";
                var result = _connection.Execute(sql, new
                {
                    Id = id
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public void Subscribe()
        {
            try
            {
                using (var tableDependency = new SqlTableDependency<Product>(ConnectionStrings.Test))
                {
                    tableDependency.OnChanged += TableDependency_Changed;
                    tableDependency.Start();

                    Console.WriteLine("Waiting for receiving notifications...");
                    Console.WriteLine("Press a key to stop");
                    Console.ReadKey();

                    tableDependency.Stop();
                }

            }
            catch (Exception e)
            {
            }
        }

        private void TableDependency_Changed(object sender, RecordChangedEventArgs<Product> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                var changedEntity = e.Entity;
                Console.WriteLine("DML operation: " + e.ChangeType);
                Console.WriteLine("ID: " + changedEntity.Id);
                Console.WriteLine("Name: " + changedEntity.Name);
            }
        }
    }
}
