using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository()
        {
            _connection = new SqlConnection(ConnectionStrings.Test);
        }

        public int Insert(User user)
        {
            try
            {
                _connection.Open();

                string sql = $@"INSERT INTO auth.users
                                    (                    
	                                    Username,
	                                    Email,
	                                    Phone,
	                                    LastActive
                                    )
                                    VALUES 
                                    ( 
	                                    @Username,
	                                    @Email,
	                                    @Phone,
	                                    @LastActive
                                    )";
                var result = _connection.Execute(sql, new
                {
                    Username = user.Username,
                    Email = user.Email,
                    Phone = user.Phone,
                    LastActive = user.LastActive,
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int UpdateLastActive(long userId)
        {
            try
            {
                _connection.Open();

                string sql = $@"UPDATE
	                                auth.users
                                SET
	                                LastActive = @LastActive
                                WHERE
                                    UserId = @UserId";
                var result = _connection.Execute(sql, new
                {
                    UserId = userId,
                    LastActive = DateTime.UtcNow
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
