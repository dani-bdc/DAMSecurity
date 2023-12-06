using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.Pkcs;

namespace DAMSecurityLib.Certificates
{
    /// <summary>
    /// Class to work with autosigned certifictes
    /// </summary>
    public class Autosigned
    {

        public static void GeneratePfx(string certFileName, string? certPassword)
        {

            // Create a self-signed certificate 
            X509Certificate2 certificate = CreateNew();

            // Save certificate to a file   
            byte[] certBytes = certificate.Export(X509ContentType.Pfx, certPassword);

            // Save cert bytes to a file
            File.WriteAllBytes(certFileName, certBytes);
        }
   
        internal static X509Certificate2 CreateNew()
        {
            using (RSA rsa = RSA.Create())
            {
                X500DistinguishedName dn = new X500DistinguishedName("CN=SelfSignedCert,O=userName");
                // Create a certificate request with the RSA key pair
                CertificateRequest certificateRequest = new CertificateRequest(dn, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                // Set the validity period of the certificate
                DateTimeOffset notBefore = DateTimeOffset.UtcNow;
                DateTimeOffset notAfter = notBefore.AddYears(1);

                // Create a self-signed certificate from the request
                X509Certificate2 certificate = certificateRequest.CreateSelfSigned(notBefore, notAfter);
                return certificate;
            }
         
        }   
    }
}
