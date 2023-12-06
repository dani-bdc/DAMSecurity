using DAMSecurityLib.Certificates;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;

namespace DAMSecurityLib.Crypto
{
    public class Sign
    {
        private X509Certificate2? certificate;

        public  void SignPdf(string inputFileName, string outputFileName)
        {
            
        }

        public void SignFile(string inputFileName, string outputFileName)
        {
            if (certificate != null)
            {
                byte[] inputBytes = File.ReadAllBytes(inputFileName);
                byte[] outputBytes = SignDocument(certificate, inputBytes);

                File.WriteAllBytes(outputFileName, outputBytes);
            }
        }

        internal static byte[] SignDocument(X509Certificate2 certificate, byte[] document)
        {
            ContentInfo contentInfo = new ContentInfo(document);
            SignedCms signedCms = new SignedCms(contentInfo, false);
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, certificate);
            signedCms.ComputeSignature(signer);

            return signedCms.Encode();
        }

        public void InitCertificate(string pfxFileName, string pfxPassword)
        {
            certificate = new X509Certificate2(pfxFileName, pfxPassword);
        }
    }
}
