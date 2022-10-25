﻿using CwRetail.Data.Models;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CwRetail.Api.Helpers
{
    public static class JwtHelper
    {
        public static string CreateToken(this string privateRsaKey, UserVerification userVerification)
        {
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateRsaKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair is null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                Dictionary<string, object?> payload = userVerification.GetType().GetProperties().ToDictionary(property => property.Name, property => property.GetValue(userVerification));
                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
            }
        }

        public static string DecodeToken(this string publicRsaKey, string token)
        {
            RSAParameters rsaParams;

            using (var tr = new StringReader(publicRsaKey))
            {
                var pemReader = new PemReader(tr);
                var publicKeyParams = pemReader.ReadObject() as RsaKeyParameters;
                if (publicKeyParams == null)
                {
                    throw new Exception("Could not read RSA public key");
                }
                rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                return Jose.JWT.Decode(token, rsa, Jose.JwsAlgorithm.RS256);
            }
        }
    }
}
