
using DAMSecurityLib.Certificates;
using DAMSecurityLib.Crypto;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;

namespace DAMSecurity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DAMSecurityLib.Crypto.RSACrypt.SavePublicKey(@"c:\test\cert.pfx", "123456", @"c:\test\publik.key");

            var cert = new X509Certificate2(@"c:\test\cert.pfx", "123456");
            var pk = DAMSecurityLib.Crypto.RSACrypt.LoadPublicKey(@"c:\test\publik.key").ExportParameters(false);
            

            var aes = new DAMSecurityLib.Crypto.AESCrypt();
            var aesKey = aes.Key;

            var encryptedKey = DAMSecurityLib.Crypto.RSACrypt.EncryptAESKey(aesKey, pk);

            var newKey=DAMSecurityLib.Crypto.RSACrypt.DecryptAESKeyWithPrivateKey(encryptedKey, cert);

            Console.WriteLine("Final");

        }
    }
}