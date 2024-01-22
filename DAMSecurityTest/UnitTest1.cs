using System.Security.Cryptography.X509Certificates;

namespace DAMSecurityTest
{
    public class Tests
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
        public void TestAES()
        {
            DAMSecurityLib.Crypto.AESCrypt aes = new DAMSecurityLib.Crypto.AESCrypt();
            byte[] key = aes.Key;

            
            byte[] key2 = aes.Key;

            Assert.That(key.SequenceEqual(key2));
        }
    }
}