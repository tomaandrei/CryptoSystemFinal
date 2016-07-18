using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptoSystemDissertation.BusinessLogic
{
    public class RSAEncryptParameters<T>
    {
        private RSAParameters publicKey;
        private T plainText;
        private RSACryptoServiceProvider csp;

        public RSAEncryptParameters(string publicKeyString, T plainText)
        {
            this.csp = new RSACryptoServiceProvider(2048);
            this.publicKey = ToRSAParameters(publicKeyString);
            this.plainText = plainText;
        }     

        public RSAEncryptParameters(RSAParameters publicKey, T plainText)
        {
            this.publicKey = publicKey;
            this.plainText = plainText;
        }

        public string Encrypt()
        {
            csp.ImportParameters(publicKey);

            var bytesPlainTextData = Encoding.Unicode.GetBytes(GetPlainTextString());
            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);
            var cypherText = Convert.ToBase64String(bytesCypherText);

            return cypherText;
        }

        private string GetPlainTextString()
        {
            return Helper.Serialize(plainText);
        }

        private RSAParameters ToRSAParameters(string publicKeyString)
        {
            RSAParameters rsaParams;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(publicKeyString);
                rsaParams = RSA.ExportParameters(false);
            }
            return rsaParams;
        }
    }
}