using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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

        public UserToken Get(long userId)
        {
            try
            {
                _connection.Open();

                string sql = $@"SELECT 
                                        t.UserTokenId,
										t.Token,
										t.RefreshAt
                                    FROM 
	                                    auth.usertokens t
									WHERE
										t.UserId = @UserId";
                var result = _connection.Query<UserToken>(sql, new
                {
                    UserId = userId
                });

                _connection.Close();

                return result.FirstOrDefault();
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public User GetUser(string token)
        {
            try
            {
                _connection.Open();

                string sql = $@"SELECT 
                                        u.UserId,
										u.Username,
										u.Email,
										u.Phone,
										u.LastActive,
										t.RefreshAt AS Expiry
                                    FROM 
	                                    auth.usertokens t
										INNER JOIN auth.users u on u.UserId = t.UserId
									WHERE
										t.Token = @Token";
                var result = _connection.Query<User>(sql, new
                {
                    Token = token
                });

                _connection.Close();

                return result.FirstOrDefault();
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
