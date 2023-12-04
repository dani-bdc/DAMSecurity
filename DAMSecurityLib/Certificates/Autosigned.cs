using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.Pkcs;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace DAMSecurityLib.Certificates
{
    /// <summary>
    /// Class to work with autosigned certifictes
    /// </summary>
    public class Autosigned
    {

        public static void GeneratePfx(string certFileName, string? certPassword)
        {
            using (RSA rsa = RSA.Create())
            {

                // Create a self-signed certificate 
                X509Certificate2 certificate = CreateNew();

                // Save certificate to a file   
                byte[] certBytes = certificate.Export(X509ContentType.Pfx, certPassword);
                // Falta guardar els bytes en un fitxer amb el nom indicat
                File.WriteAllBytes(certFileName, certBytes);

            }
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

        public static void SignPdfWithNewCertificate(string inputFileName, string outFileName)
        {
            
            X509Certificate2 certificate = Autosigned.CreateNew();
            using(PdfReader pdfReader = new PdfReader(inputFileName))
            {
                using (FileStream outputStream = new FileStream(outFileName, FileMode.Create))
                {
                    using (PdfWriter pdfWriter = new PdfWriter(outputStream))
                    {

                    }
                }
            }
            byte[] input = File.ReadAllBytes(inputFileName);
            byte[] output = Sign(certificate, input);
            File.WriteAllBytes(outFileName, output);
        }

        public static byte[] Sign(X509Certificate2 certificate, byte[]document)
        {
            ContentInfo contentInfo = new ContentInfo(document);
            SignedCms signedCms = new SignedCms(contentInfo, true);
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, certificate);
            signedCms.ComputeSignature(signer);

            return signedCms.Encode();
        }


    }
}
