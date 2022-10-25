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
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly SqlConnection _connection;

        public UserRolesRepository()
        {
            _connection = new SqlConnection(ConnectionStrings.Test);
        }

        public int Insert(long userId)
        {
            try
            {
                _connection.Open();

                string sql = $@"INSERT INTO auth.userroles
                                (                    
	                                UserId,
									RoleId
                                )
                                VALUES 
                                ( 
	                                @UserId,
									(SELECT RoleId FROM auth.roles WHERE Role = 'User' AND SubRole = 'Standard')
                                )";
                var result = _connection.Execute(sql, new
                {
                    UserId = userId
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
