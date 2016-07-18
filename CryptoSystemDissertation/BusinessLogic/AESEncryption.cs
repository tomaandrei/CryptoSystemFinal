using System;
using System.IO;
using System.Security.Cryptography;

namespace CryptoSystemDissertation.BusinessLogic
{
    public class AESEncryption<T>
    {
        private string plainText;
        private byte[] key;
        private byte[] initializationVector;

        public AESEncryption(T plainText)
        {
            this.plainText = Helper.Serialize(plainText);         
        }

        public byte[] GenerateAesKeys()
        {
            try
            {
                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.GenerateIV();
                    this.initializationVector = myRijndael.IV;
                    this.GetAESKey(myRijndael);                                                               
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            return this.initializationVector;           
        }

        public string EncryptAES()
        {
            byte[] encrypted = EncryptStringToBytes();
            var cypherParameters = Convert.ToBase64String(encrypted);

            return cypherParameters;
        }

        private byte[] EncryptStringToBytes()
        {
            this.CheckArguments();
            byte[] encrypted;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = this.key;
                rijAlg.IV = this.initializationVector;
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(this.plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        private void CheckArguments()
        {
            if (this.plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (this.key == null || this.key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (this.initializationVector == null || this.initializationVector.Length <= 0)
                throw new ArgumentNullException("IV");
        }

        private void GetAESKey(RijndaelManaged myRijndael)
        {
            var path = @"D:\file.txt";
            if (!File.Exists(path))
            {
                myRijndael.GenerateKey();
                this.key = myRijndael.Key;
                this.WriteFile(path);
            }
            else
            {
                this.ReadFile(path);
            }
        }

        private void WriteFile(string path)
        {
            using (FileStream fileStream = File.Create(path))
            {
                fileStream.Write(this.key, 0, this.key.Length);
            }
            File.Encrypt(path);
        }

        private void ReadFile(string path)
        {
            this.key = File.ReadAllBytes(path);
        }
    }
}