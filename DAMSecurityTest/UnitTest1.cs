using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DAMSecurityTest
{
    public class Tests
    {
        private string certFileName = "cert.pfx";
        private string certPassword = "123456";
        private string publicKeyFileName = "public.key";

        [SetUp]
        public void Setup()
        {
            DAMSecurityLib.Certificates.Autosigned.GeneratePfx(certFileName, certPassword);
        }

        [TearDown]
        public void Teardown()
        {
            File.Delete(certFileName);
        }

        [Test]
        public void TestInitIVVector()
        {
            DAMSecurityLib.Crypto.AESCrypt aes = new DAMSecurityLib.Crypto.AESCrypt();
            aes.GenerateIV();
            byte[] iv = aes.IV;
            byte[] key = aes.Key;

            DAMSecurityLib.Crypto.AESCrypt aes2 = new DAMSecurityLib.Crypto.AESCrypt();
            aes2.Key = key;
            aes2.GenerateIV();

            byte[] iv2 = aes2.IV;

            Assert.That(iv.SequenceEqual(iv2));
        }

        [Test]
        public void TestEncryptDecryptAES()
        {
            DAMSecurityLib.Crypto.AESCrypt aes = new DAMSecurityLib.Crypto.AESCrypt();
            aes.GenerateIV();
            byte[] key = aes.Key;

            string initialText = "Text to encrypt";
            byte[] initialBytes = Encoding.UTF8.GetBytes(initialText);
            byte[] encryptedBytes = aes.Encrypt(initialBytes);
            
            DAMSecurityLib.Crypto.AESCrypt aes2 = new DAMSecurityLib.Crypto.AESCrypt();
            aes2.Key = key;
            aes2.GenerateIV();

            byte[] decryptedBytes = aes2.Decrypt(encryptedBytes);
            string finalText = Encoding.UTF8.GetString(decryptedBytes);
            Assert.That(finalText, Is.EqualTo(initialText));

        }

        [Test]
        public void TestEncryptDecryptRSA()
        {
            createPublicKeyFile();
            var pk = DAMSecurityLib.Crypto.RSACrypt.LoadPublicKey(publicKeyFileName).ExportParameters(false);


            var aes = new DAMSecurityLib.Crypto.AESCrypt();
            var aesKey = aes.Key;

            var encryptedKey = DAMSecurityLib.Crypto.RSACrypt.EncryptAESKey(aesKey, pk);

            var cert = new X509Certificate2(certFileName, certPassword);
            var newKey = DAMSecurityLib.Crypto.RSACrypt.DecryptAESKeyWithPrivateKey(encryptedKey, cert);

            Assert.That(aesKey.SequenceEqual(newKey));

            removePublicFile();
        }

        private void createPublicKeyFile()
        {
            DAMSecurityLib.Crypto.RSACrypt.SavePublicKey(certFileName, certPassword, publicKeyFileName);

        }

        private void removePublicFile()
        {
            File.Delete(publicKeyFileName);
        }
    }
}