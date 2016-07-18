using System;
using System.IO;
using System.Security.Cryptography;

namespace CryptoSystemDissertation.BusinessLogic
{
    public class AESDecryption<T>
    {
        private byte[] cipherText;
        private byte[] key;
        private byte[] IV;

        public AESDecryption(string cipherText, byte[] IV)
        {
            this.cipherText = Convert.FromBase64String(cipherText);
            this.key = this.GetKey();
            this.IV = IV;
        }

        public T DecryptParameters()
        {
            string plainText = DecryptStringFromBytes();
            T parameters = Helper.Deserialize<T>(plainText);

            return parameters;
        }

        private string DecryptStringFromBytes()
        {
            this.CheckArguments();
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = this.key;
                rijAlg.IV = this.IV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private byte[] GetKey()
        {       
            var path = @"D:\file.txt";
            byte[] key = null;
            if (File.Exists(path))
            {
                key = File.ReadAllBytes(path);
            }

            return key;
        }

        private void CheckArguments()
        {
            if (this.cipherText == null || this.cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (this.key == null || this.key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (this.IV == null || this.IV.Length <= 0)
                throw new ArgumentNullException("IV");
        }
    }
}