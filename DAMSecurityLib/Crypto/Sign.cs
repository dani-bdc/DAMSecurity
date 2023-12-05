using DAMSecurityLib.Certificates;
using iText.Forms.Fields;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;


using System;
using iText.Kernel.Pdf;
using iText.Signatures;

namespace DAMSecurityLib.Crypto
{
    public class Sign
    {
        private Pkcs12Store pfxKeyStore = new Pkcs12Store();

        public void InitPkcs12StoreFromPfx(string pfxFilePath, string pfxPassword)
        {
            this.pfxKeyStore = new Pkcs12Store(new FileStream(pfxFilePath, FileMode.Open, FileAccess.Read), pfxPassword.ToCharArray());
        }

        public  void SignPdf(string inputFileName, string outputFileName)
        {
            var alias = GetAlias(this.pfxKeyStore);
            var key = this.pfxKeyStore.GetKey(alias).Key;
            var ce = this.pfxKeyStore.GetCertificateChain(alias);
            X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[ce.Length];
            using (var pdfReader = new PdfReader(inputFileName))
            {
                using (var pdfWriter = new PdfWriter(outputFileName))
                {
                    using (var stamper = new PdfDocument(pdfReader, pdfWriter))
                    {
                        
                        var signer = new PdfSigner(pdfReader, pdfWriter, new StampingProperties().UseAppendMode());
                        //var externalSignature = new PrivateKeySignature(key, DigestAlgorithms.SHA256);
                        //signer.SignDetached(externalSignature, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
                        
                    }
                }
            }
        }

        public  void SignImage(string inputFileName, string outputFileName)
        {

        }

        private static string GetAlias(Pkcs12Store pkcsStore)
        {
            foreach (string alias in pkcsStore.Aliases)
            {
                if(pkcsStore.IsKeyEntry(alias))
                {
                    return alias;
                }
            }
            throw new Exception("Alias not found");
        }
    }
}
