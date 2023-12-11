using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Crypto
{
    internal class AESCrypt
    {
        private Aes aes;

        public byte[] Key
        {
            get { return aes.Key; }
            set { aes.Key = value; }
        }

        public byte[] IV
        {
            get { return aes.IV; }
            set { aes.IV = value; }
        }

        public AESCrypt()
        {
            aes= Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();
        }

        public byte[] Encrypt(string text)
        {
            ICryptoTransform ct = aes.CreateEncryptor();
            byte[] encryptedData;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(text);
                    }
                    encryptedData = ms.ToArray();
                }
            }

            return encryptedData;
        }
    }
}
