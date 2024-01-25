using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using DAMUtils.PDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.Socket
{
    internal class ServerData
    {
        public IPDFGenerator? PdfGenerator { get; set; }

        public KeyFilePair? KeyFilePair { get; set; }

        public void ProcessInputData(ObjectPair data)
        {
            if (data.Obj1 == null || data.Obj2 == null)
                return;

            byte[] pdfData;

            object pdfInfo = data.Obj1;
            RSAParameters parameters = (RSAParameters)data.Obj2;

            if (PdfGenerator == null)
                pdfData = new byte[0];
            else
                pdfData = PdfGenerator.Generate(pdfInfo);

            KeyFilePair = Hybrid.Crypt(parameters, pdfData);
        }

        
    }
}
