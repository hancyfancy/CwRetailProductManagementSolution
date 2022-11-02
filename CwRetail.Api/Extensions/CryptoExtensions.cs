using CwRetail.Data.Models;
using System.Security.Cryptography;
using System.Text;

namespace CwRetail.Api.Extensions
{
    public static class CryptoExtensions
    {
        public static byte[] GenerateEncryptionKey()
        {
            try
            {
                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider())
                {
                    tripleDES.GenerateKey();
                    return tripleDES.Key;
                }
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public static string Encrypt(this string input, byte[] key)
        {
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);

                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider())
                {
                    tripleDES.Key = key;
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                    tripleDES.Clear();
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
            catch (Exception e)
            {
                return default;
            }
        }
        public static string Decrypt(this string input, byte[] key)
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(input.Replace(" ", "+"));

                using (TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider())
                {
                    tripleDES.Key = key;
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                    tripleDES.Clear();
                    return UTF8Encoding.UTF8.GetString(resultArray);
                }
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
