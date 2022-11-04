using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories.Implementation
{
    public class UserEncryptionRepository : IUserEncryptionRepository
    {
        private readonly SqlConnection _connection;

        public UserEncryptionRepository()
        {
            _connection = new SqlConnection(ConnectionStrings.Test);
        }

        public byte[] InsertOrUpdate(long userId, byte[] encryptionKey)
        {
            try
            {
                _connection.Open();

                string sql = $@"USE CwRetail

                                    IF EXISTS (SELECT UserEncryptionId FROM auth.userencryption WHERE UserId = @UserId)
                                    BEGIN
		                                UPDATE 
			                                auth.userencryption
		                                SET	
			                                EncryptionKey = @EncryptionKey
                                        OUTPUT inserted.EncryptionKey
		                                WHERE
			                                UserId = @UserId
                                    END
                                    ELSE
                                    BEGIN
                                        INSERT INTO auth.userencryption (UserId, EncryptionKey)
									    OUTPUT inserted.EncryptionKey 
	                                    VALUES (@UserId, @EncryptionKey)
                                    END";

                var result = _connection.ExecuteScalar<byte[]>(sql, new
                {
                    UserId = userId,
                    EncryptionKey = encryptionKey
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public UserEncryption Get(long userId)
        {
            try
            {
                _connection.Open();

                string sql = $@"USE CwRetail

                                    SELECT 
                                        e.UserEncryptionId,
										e.EncryptionKey
                                    FROM 
	                                    auth.userencryption e
									WHERE
										e.UserId = @UserId";

                var result = _connection.Query<UserEncryption>(sql, new
                {
                    UserId = userId
                });

                _connection.Close();

                UserEncryption userEncryption = result.FirstOrDefault();

                userEncryption.UserId = userId;

                return userEncryption;
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
