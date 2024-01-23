using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Crypto
{
    /// <summary>
    /// Class used to encrypt/decrypt using aES alghorithm
    /// </summary>
    public class AESCrypt
    {
        #region Private attributes

        // Aes class used to encrypt/decrypt data
        private Aes aes;

        #endregion

        #region Public properties

        /// <summary>
        /// Key used to encrypt/decrypt data
        /// </summary>
        public byte[] Key
        {
            get { return aes.Key; }
            set { aes.Key = value; }
        }

        /// <summary>
        /// Initialization Vector (IV) to encrypt/decrypt data
        /// </summary>
        public byte[] IV
        {
            get { return aes.IV; }
            set { aes.IV = value; }
        }

        #endregion

        /// <summary>
        /// Default constructor
        /// Initializes AESCrypt with random key and IV
        /// </summary>
        public AESCrypt()
        {
            aes= Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();
        }

        /// <summary>
        /// Generate IV (Initialization Vector)
        /// We need to use this function only if we want generate IV vector from key. If we want a randon IV vector we must not use this function
        /// </summary>
        public void GenerateIV()
        {
            // Generate IV vector from key
            byte[] ivBytes = new byte[16];
            for (int i=0;i<16;i++)
            {
                ivBytes[i] = aes.Key[i];
            }
            aes.IV = ivBytes;   
        }

        /// <summary>
        /// Encrypte byte data using Aes and returns encrypted value
        /// </summary>
        /// <param name="bytes">byte[] to encrypt</param>
        /// <returns>byte[] corresponding to encrypted data</returns>
        public byte[] Encrypt(byte[] bytes)
        {
            ICryptoTransform ct = aes.CreateEncryptor();
            byte[] encryptedData;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                {
                   
                    cs.Write(bytes, 0, bytes.Length);
                    
                }
                encryptedData = ms.ToArray();
            }

            return encryptedData;
        }

        /// <summary>
        /// Encrypts text and returns encrypted value
        /// </summary>
        /// <param name="text">Text to encrypt</param>
        /// <returns>byte[] corresponding to encrypted text</returns>
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
        
        /// <summary>
        /// Decrypts input data and returns decrypted value
        /// </summary>
        /// <param name="encryptedData">Data to decrypt</param>
        /// <returns>byte[] corresponding to decrypted data</returns>
        public byte[] Decrypt(byte[] encryptedData)
        {
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decryptedData;
            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream msDecrypt = new MemoryStream())
                    {
                        cs.CopyTo(msDecrypt);
                        decryptedData = msDecrypt.ToArray(); 
                    }
                }
            }
            return decryptedData;
        }

        /// <summary>
        /// Decrypts encrypted data to a file in disk
        /// </summary>
        /// <param name="encryptedData">Encrypted data to decrypt</param>
        /// <param name="outFileName">File path to store decrypted data</param>
        public void DecryptToFile(byte[] encryptedData, string outFileName)
        {
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] decryptedData;
            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream msDecrypt = new MemoryStream())
                    {
                        cs.CopyTo(msDecrypt);
                        decryptedData = msDecrypt.ToArray();
                        File.WriteAllBytes(outFileName, decryptedData);
                    }
                }
            }
        }

    }
}
