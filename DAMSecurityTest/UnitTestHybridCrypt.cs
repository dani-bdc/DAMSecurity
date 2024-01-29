using DAMSecurityLib.Crypto;
using DAMSecurityLib.Exceptions;
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
        private string fakeCertFileName = "fakecert.pfx";
        private string fakeCertPassword = "123456";

        [SetUp]
        public void Setup()
        {
            DAMSecurityLib.Certificates.Autosigned.GeneratePfx(certFileName, certPassword);
            DAMSecurityLib.Certificates.Autosigned.GeneratePfx(fakeCertFileName, fakeCertPassword);

        }

        [TearDown]
        public void Teardown()
        {
            File.Delete(certFileName);
            File.Delete(fakeCertFileName);
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

        [Test]
        public void TestCryptDecryptInvalidCertificate()
        {
            X509Certificate2 certificate = new X509Certificate2(certFileName, certPassword);
            X509Certificate2 fakeCertificate = new X509Certificate2(fakeCertFileName, fakeCertPassword);

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

            Assert.Throws<DecryptException>(()=> Hybrid.Decrypt(fakeCertificate, kfp));
            
        }
    }
}
