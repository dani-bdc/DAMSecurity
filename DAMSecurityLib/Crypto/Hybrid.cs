using DAMSecurityLib.Data;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Crypto
{
    /// <summary>
    /// Class with some utilities using mixed cryptography
    /// </summary>
    public class Hybrid
    {

        /// <summary>
        /// Crypt file using RSA/AES. Crypts AES key with RSA and file with AES.
        /// Stores key and file used to code insde KeyFilePair
        /// </summary>
        /// <param name="publicKey">Public Key to crypt with RSA</param>
        /// <param name="file">File to encrypt</param>
        /// <returns>Key an file with encrypted values</returns>
        public static KeyFilePair Crypt(RSAParameters publicKey, byte[] file)
        {
            KeyFilePair keyFilePair = new KeyFilePair();
            AESCrypt aes = new AESCrypt();
            aes.GenerateIV();

            keyFilePair.Key = RSACrypt.EncryptAESKey(aes.Key, publicKey);
            keyFilePair.File = aes.Encrypt(file);

            return keyFilePair;
        }

        /// <summary>
        /// Decrypts file crypted with AES/RSA using certificate
        /// </summary>
        /// <param name="certificate">Certificate to decrypt with</param>
        /// <param name="keyFilePair">Encrypted key and file</param>
        /// <returns>byte[] corresponding to decrypted file</returns>
        public static byte[] Decrypt(X509Certificate2 certificate, KeyFilePair keyFilePair)
        {
            byte[] aesKey = RSACrypt.DecryptAESKeyWithPrivateKey(keyFilePair.Key, certificate);
            AESCrypt aes = new AESCrypt();
            aes.Key = aesKey;
            aes.GenerateIV();

            return aes.Decrypt(keyFilePair.File);
        }

    }
}
