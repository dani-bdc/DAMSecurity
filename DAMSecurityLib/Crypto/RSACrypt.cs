using DAMSecurityLib.Data;
using DAMSecurityLib.Exceptions;
using Newtonsoft.Json;
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
    /// Class with some utilities to Encrypt/Decrypt using RSA algorithn
    /// </summary>
    public class RSACrypt
    {
        /// <summary>
        /// Save certificate public key to a file on disk
        /// </summary>
        /// <param name="pfxFilename">Pfx file path to extract public key</param>
        /// <param name="pfxPassword">Pfx password</param>
        /// <param name="publicKeyFile">File path to store certificate public key</param>
        public static void SavePublicKey(string pfxFilename, string pfxPassword, string publicKeyFile)
        {
            X509Certificate2 certificate = new X509Certificate2(pfxFilename, pfxPassword);
            
            RSA? publicKey = certificate.GetRSAPublicKey();
            if (publicKey!=null)
            {
                SavePublicKey(publicKey, publicKeyFile);
            }
        }

        /// <summary>
        /// Load Certificate public key stored in disk
        /// </summary>
        /// <param name="publicKeyFile">Public path file path</param>
        /// <returns>RSA certificate with public key</returns>
        public static RSA LoadPublicKey(string publicKeyFile)
        {
            RSAParameters publicKeyParams = new RSAParameters();
            using (StreamReader reader = new StreamReader(publicKeyFile))
            {
                string? line = reader.ReadLine();
                if (line != null)
                    publicKeyParams.Modulus = Convert.FromBase64String(line);
                line = reader.ReadLine();
                if (line != null)
                    publicKeyParams.Exponent = Convert.FromBase64String(line);
            }

            RSA rsa = RSA.Create();
            rsa.ImportParameters(publicKeyParams);
            return rsa;
        }

        /// <summary>
        /// Save RSA Public key to a file
        /// </summary>
        /// <param name="publicKey">RSA Public key to store in disk</param>
        /// <param name="publicKeyFile">Filesystem file path to store public key</param>
        public static void SavePublicKey(RSA publicKey, string publicKeyFile)
        {
            RSAParameters publickeyParams = publicKey.ExportParameters(false);
            using (StreamWriter writer = new StreamWriter(publicKeyFile))
            {
                byte[]? modulus = publickeyParams.Modulus;
                if (modulus != null)
                    writer.WriteLine(Convert.ToBase64String(modulus));
                byte[]? exponent = publickeyParams.Exponent;
                if (exponent != null)
                    writer.WriteLine(Convert.ToBase64String(exponent));
            }
        }
   
        /// <summary>
        /// Extract RSA Public key and converts it to byte[]
        /// </summary>
        /// <param name="rsa">RSA to extract public key</param>
        /// <returns>byte[] corresponding to public key</returns>
        public static byte[] PublicKey(RSA rsa)
        {
            RSAParameters publickeyParams = rsa.ExportParameters(false);
            string json = JsonConvert.SerializeObject(publickeyParams);
            return Encoding.UTF8.GetBytes(json);            
        }

        /// <summary>
        /// Create RSAParametres from byte array
        /// </summary>
        /// <param name="publickey">Byte[] to convert</param>
        /// <returns>RSAParameters associated to input</returns>
        public static RSAParameters LoadPublicKey(byte[] publickey)
        {
            string json = Encoding.UTF8.GetString(publickey);
            RSAParameters parameters = JsonConvert.DeserializeObject<RSAParameters>(json);

            return parameters;

        }

        /// <summary>
        /// Encrypt AESKey with RSA public key
        /// </summary>
        /// <param name="aesKey">AES Key to encrypt</param>
        /// <param name="publicKey">RSA public key to encrypt with</param>
        /// <returns>Encrypted AES key</returns>
        public static byte[] EncryptAESKey(byte[] aesKey, RSAParameters publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(publicKey);
                byte[] encryptedAesKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
                return encryptedAesKey;
            }
                
        }

        /// <summary>
        /// Decrypt AES key encryted with RSA algorithm
        /// </summary>
        /// <param name="encrypteKey">Encrypted key to decrypt</param>
        /// <param name="certificate">Certificate used to decrypt</param>
        /// <returns>byte[] corresponding to decrypted key</returns>
        public static byte[] DecryptAESKeyWithPrivateKey(byte[] encrypteKey, X509Certificate2 certificate)
        {
            using (RSA? rsa = certificate.GetRSAPrivateKey())
            {
                byte[] aesKey;
                if (rsa != null)
                    aesKey = rsa.Decrypt(encrypteKey, RSAEncryptionPadding.OaepSHA256);
                else
                    aesKey = new byte[0];
                return aesKey;

            }
            
        }

        /// <summary>
        /// Encrypt file using publicKey
        /// </summary>
        /// <param name="publicKey">PublicKey used to encript file</param>
        /// <param name="file">Byte[] representing file to encript</param>
        /// <returns>KeyFilePair withe key encripted in RSA and byte[] corresponding to file encripted with AES Key</returns>
        public static KeyFilePair Encrypt(RSAParameters publicKey, byte[] file)
        {
            KeyFilePair keyFilePair = new KeyFilePair();
            var aes = new DAMSecurityLib.Crypto.AESCrypt();
            aes.GenerateIV();
            var aesKey = aes.Key;

            keyFilePair.Key = DAMSecurityLib.Crypto.RSACrypt.EncryptAESKey(aesKey, publicKey);
            keyFilePair.File = aes.Encrypt(file);
            
            return keyFilePair;
        }

        /// <summary>
        /// Decrypt file encripted with rSA
        /// </summary>
        /// <param name="keyFilePair">KeyFilePair with encripted information</param>
        /// <param name="certificate">Certificate used to decript</param>
        /// <returns>byte[] corresponding to decripted file</returns>
        /// <exception cref="IncorrectParametersException"></exception>
        public static byte[] Decrypt(KeyFilePair keyFilePair, X509Certificate2 certificate)
        {
            if ((keyFilePair.Key == null) || (keyFilePair.File == null))
                throw new IncorrectParametersException("Incorrect decrypt KeyFilePair");

            byte[] aeskey = DecryptAESKeyWithPrivateKey(keyFilePair.Key, certificate);
            AESCrypt aes = new AESCrypt();
            aes.Key = aeskey;
            aes.GenerateIV();
            return aes.Decrypt(keyFilePair.File);            
        }
    }
}
