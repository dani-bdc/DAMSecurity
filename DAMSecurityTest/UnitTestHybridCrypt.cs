using DAMSecurityLib.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityTest
{
    public class UnitTestHybridCrypt
    {
        private string certFileName = "cert.pfx";
        private string certPassword = "123456";
        
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
        public void TestCryptDecrypt()
        {
            X509Certificate2 certificate = new X509Certificate2(certFileName, certPassword);
            
            var cPk = certificate.GetRSAPublicKey();
            if (cPk == null)
            {
                Assert.IsTrue(false);
                return;
            }
            var publicKey = cPk.ExportParameters(false);
            string initialText = "Text to encrypt";
            byte[] initialBytes = Encoding.UTF8.GetBytes(initialText);

            var kfp = Hybrid.Crypt(publicKey, initialBytes);
            byte[] finalBytes = Hybrid.Decrypt(certificate, kfp);
            string finalText = Encoding.UTF8.GetString(finalBytes);
            Assert.That(finalText, Is.EqualTo(initialText));
        }

    }
}
