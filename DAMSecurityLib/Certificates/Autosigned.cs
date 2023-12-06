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
        /// <summary>
        /// Generate Pfx  Certificate.
        /// This Method generates pfx certificate with default info,
        /// </summary>
        /// <param name="certFileName">Pfx certificate file path</param>
        /// <param name="certPassword">Pfx certificate password</param>
        public static void GeneratePfx(string certFileName, string? certPassword)
        {
            CertificateInfo info = new CertificateInfo();

            GeneratePfx(certFileName, certPassword, info);
        }

        /// <summary>
        /// Generate Pfx certificate.
        /// This method generates pfx certificates with CerticateInfo inside it. 
        /// </summary>
        /// <param name="certFileName">Pfx certificate file path</param>
        /// <param name="certPassword">Pfx certifiate password</param>
        /// <param name="info">Pfx certicicate info to display it</param>
        public static void GeneratePfx(string certFileName, string? certPassword, CertificateInfo info)
        {
            // Create a self-signed certificate 
            X509Certificate2 certificate = CreateNew(info);

            // Save certificate to a file   
            byte[] certBytes = certificate.Export(X509ContentType.Pfx, certPassword);

            // Save cert bytes to a file
            File.WriteAllBytes(certFileName, certBytes);
        }
   
        /// <summary>
        /// Geneates certificate witohout saving to disk
        /// </summary>
        /// <param name="info">PFx certiciate info</param>
        /// <returns>certificate object to work with it</returns>
        internal static X509Certificate2 CreateNew(CertificateInfo info)
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
