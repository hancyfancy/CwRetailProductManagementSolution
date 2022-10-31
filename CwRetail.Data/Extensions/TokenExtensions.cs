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
        private static readonly char[] _chars;

        static TokenExtensions()
        {
            _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        }

        public static string GetUniqueKey(int size = 200)
        {
            try
            {
                byte[] data = new byte[4 * size];
                using (var crypto = RandomNumberGenerator.Create())
                {
                    crypto.GetBytes(data);
                }
                StringBuilder result = new StringBuilder(size);
                for (int i = 0; i < size; i++)
                {
                    var rnd = BitConverter.ToUInt32(data, i * 4);
                    var idx = rnd % _chars.Length;

                    result.Append(_chars[idx]);
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
