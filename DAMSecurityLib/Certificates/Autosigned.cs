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
    class Autosigned
    {
        public static void Generate()
        {
            using (RSA rsa = RSA.Create())
            {
                // Create a certificate request with the RSA key pair
                CertificateRequest certificateRequest = new CertificateRequest("CN=SelfSignedCert", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                // Set the validity period of the certificate
                DateTimeOffset notBefore = DateTimeOffset.UtcNow;
                DateTimeOffset notAfter = notBefore.AddYears(1); 

                // Create a self-signed certificate from the request
                X509Certificate2 certificate = certificateRequest.CreateSelfSigned(notBefore, notAfter);

                // Save certificate to a file
                string certFilePath = "selfsigned.pfx";
                string certPassword = "123456";
                byte[] certBytes = certificate.Export(X509ContentType.Pfx, certPassword);
                // Falta guardar els bytes en un fitxer amb el nom indicat

                byte[] content = new byte[2]; // Això hauria de ser el byte[] del document/imatge a firmar
                ContentInfo contentInfo = new ContentInfo(content);
                
                SignedCms cms = new SignedCms(contentInfo, true);
                CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, certificate);
                cms.ComputeSignature(signer);
                byte[] signedData = cms.Encode();
            }
        }
    }
}
