using DAMSecurityLib.Certificates;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Signatures;
using Org.BouncyCastle.Pkcs;
using System;

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
            
        }

        public  void SignImage(string inputFileName, string outputFileName)
        {

        }
    }
}
