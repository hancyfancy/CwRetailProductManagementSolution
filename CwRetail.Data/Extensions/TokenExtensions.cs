using CwRetail.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Extensions
{
    public static class TokenExtensions
    {
        private static readonly char[] _numbers;
        private static readonly char[] _chars;

        static TokenExtensions()
        {
            _numbers = "1234567890".ToCharArray();
            _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Concat(_numbers).ToArray();
        }

        public static string GetUniqueKey(UserContactTypeEnum userContactTypeEnum, int size)
        {
            try
            {
                char[] dataset = userContactTypeEnum == UserContactTypeEnum.Email ? _chars 
                    : userContactTypeEnum == UserContactTypeEnum.Phone ? _numbers : null;

                byte[] data = new byte[4 * size];
                using (var crypto = RandomNumberGenerator.Create())
                {
                    crypto.GetBytes(data);
                }
                StringBuilder result = new StringBuilder(size);
                for (int i = 0; i < size; i++)
                {
                    var rnd = BitConverter.ToUInt32(data, i * 4);
                    var idx = rnd % dataset.Length;

                    result.Append(dataset[idx]);
                }

                return result.ToString();
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
