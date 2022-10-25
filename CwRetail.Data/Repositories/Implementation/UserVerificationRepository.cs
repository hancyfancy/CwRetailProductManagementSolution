﻿using CwRetail.Data.Extensions;
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
    public class UserVerificationRepository : IUserVerificationRepository
    {
        private readonly SqlConnection _connection;

        public UserVerificationRepository()
        {
            _connection = new SqlConnection(ConnectionStrings.Test);
        }

        public int Insert(UserVerification userVerification)
        {
            try
            {
                _connection.Open();

                string sql = $@"INSERT INTO auth.userverification
                                (                    
	                                UserId,
	                                EmailVerified,
	                                PhoneVerified
                                )
                                VALUES 
                                ( 
	                                @UserId,
	                                @EmailVerified,
	                                @PhoneVerified
                                )";
                var result = _connection.Execute(sql, new
                {
                    UserId = userVerification.UserId,
                    EmailVerified = userVerification.EmailVerified,
                    PhoneVerified = userVerification.PhoneVerified,
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int UpdateEmailVerified(long userId)
        {
            try
            {
                _connection.Open();

                string sql = $@"UPDATE
	                                auth.userverification
                                SET
	                                EmailVerified = true
                                WHERE
                                    UserId = @UserId";
                var result = _connection.Execute(sql, new
                {
                    UserId = userId,
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int UpdatePhoneVerified(long userId)
        {
            try
            {
                _connection.Open();

                string sql = $@"UPDATE
	                                auth.userverification
                                SET
	                                PhoneVerified = true
                                WHERE
                                    UserId = @UserId";
                var result = _connection.Execute(sql, new
                {
                    UserId = userId,
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public UserVerification Get(string username)
        {
            try
            {
                _connection.Open();

                string sql = $@"SELECT 
                                        v.UserVerificationId,
										u.UserId,
										v.EmailVerified,
										v.PhoneVerified,
										u.Email,
										u.Phone,
										u.LastActive
                                    FROM 
	                                    auth.users u
										INNER JOIN auth.userverification v on v.UserId = u.UserId
									WHERE 
										u.Username = @Username";
                var result = _connection.Query<UserVerification>(sql, new
                {
                    Username = username
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