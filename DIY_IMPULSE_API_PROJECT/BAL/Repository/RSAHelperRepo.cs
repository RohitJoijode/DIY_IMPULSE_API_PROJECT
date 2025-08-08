using DIY_IMPULSE_API_PROJECT.BAL.IRepository;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DIY_IMPULSE_API_PROJECT.BAL.Repository
{
    public class RSAHelperRepo : IRepository.IRSAHelperRepo
    {
        private readonly RSACryptoServiceProvider _PrivateKey;
        private readonly RSACryptoServiceProvider _PublicKey;
        private RSAParameters _privateParaKey;
        private RSAParameters _publicParaKey;


        public RSAHelperRepo()
        {
            string public_pem = @".\KEYS\mypublickey.pem";
            string private_pem = @".\KEYS\mykey.pem";
            _PrivateKey = GetPrivateKeyFromPemFile(private_pem);
            _PublicKey = GetPublicKeyFromPemFile(public_pem);

            //_privateParaKey = _PrivateKey.ExportParameters(false);
            //_publicParaKey = _PublicKey.ExportParameters(true);
        }

        public string Encrypt(string text)
        {
            var encryptedBytes = _PublicKey.Encrypt(Encoding.UTF8.GetBytes(text), false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encrypted)
        {
            var decrytedBytes = _PrivateKey.Decrypt(Convert.FromBase64String(encrypted), false);
            return Encoding.UTF8.GetString(decrytedBytes);
        }

        public static RSACryptoServiceProvider GetPrivateKeyFromPemFile(string filePath)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                AsymmetricCipherKeyPair readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();
                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();// cspParams);
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

        public static RSACryptoServiceProvider GetPublicKeyFromPemFile(string filePath)
        {
            using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParam);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();// cspParams);
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

    }
}
