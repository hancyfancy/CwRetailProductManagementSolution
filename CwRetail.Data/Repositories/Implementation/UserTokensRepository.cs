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
    public class UserTokensRepository : IUserTokensRepository
    {
        private readonly SqlConnection _connection;

        public UserTokensRepository()
        {
            _connection = new SqlConnection(ConnectionStrings.Test);
        }

        public string InsertOrUpdate(long userId, string token)
        {
            try
            {
                _connection.Open();

                string sql = $@"IF EXISTS (SELECT UserTokenId FROM auth.usertokens WHERE UserId = @UserId)
                                BEGIN
	                                IF (SELECT RefreshAt FROM auth.usertokens WHERE UserId = @UserId) > GETUTCDATE()
	                                BEGIN
		                                UPDATE 
			                                auth.usertokens
		                                SET	
			                                Token = @Token,
			                                RefreshAt = @RefreshAt
                                        OUTPUT inserted.Token
		                                WHERE
			                                UserId = @UserId
	                                END
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO auth.usertokens (UserId, Token, RefreshAt)
									OUTPUT inserted.Token 
	                                VALUES (@UserId, @Token, @RefreshAt)
                                END";
                var result = _connection.ExecuteScalar<string>(sql, new
                {
                    UserId = userId,
                    Token = token,
                    RefreshAt = DateTime.UtcNow.AddDays(1)
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
