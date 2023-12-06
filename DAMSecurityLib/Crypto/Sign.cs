using DAMSecurityLib.Certificates;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using iText.Kernel.Pdf;
using iText.Signatures;
using iText.Bouncycastle.Crypto;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using Org.BouncyCastle.Crypto;


namespace DAMSecurityLib.Crypto
{
    public class Sign
    {
        private X509Certificate2? certificate;
        private Pkcs12Store pkcs12Store = new Pkcs12StoreBuilder().Build();

        public  void SignPdf(string inputFileName, string outputFileName)
        {
            string testFileName = @"c:\test\test1.pdf";
            string outTestFileName = @"c:\test\test1_sign2.pdf";
            string storePath = @"c:\test\cert.pfx";
            char[] storePass = "123456".ToCharArray();
            string storeAlias = "";

            Pkcs12Store pkcs12 = new Pkcs12StoreBuilder().Build();
            pkcs12.Load(new FileStream(storePath, FileMode.Open, FileAccess.Read), storePass);
            foreach (string currentAlias in pkcs12.Aliases)
            {
                if (pkcs12.IsKeyEntry(currentAlias))
                {
                    storeAlias = currentAlias;
                    break;
                }
            }
            AsymmetricKeyParameter key = pkcs12.GetKey(storeAlias).Key;

            X509CertificateEntry[] chainEntries = pkcs12.GetCertificateChain(storeAlias);
            IX509Certificate[] chain = new IX509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = new X509CertificateBC(chainEntries[i].Certificate);
            PrivateKeySignature signature = new PrivateKeySignature(new PrivateKeyBC(key), "SHA256");

            using (PdfReader pdfReader = new PdfReader(testFileName))
            using (FileStream result = File.Create(outTestFileName))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                pdfSigner.SignDetached(signature, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
            }
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

            pkcs12Store.Load(new FileStream(pfxFileName, FileMode.Open, FileAccess.Read), pfxPassword.ToCharArray());
        }
    }
}
