using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.PDF.Fake
{
    public class FakePDFGenerator : IPDFGenerator
    {
        byte[] IPDFGenerator.Generate(object prms)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfWriter writer = new PdfWriter(ms))
                {
                    using (PdfDocument pdfDocument = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdfDocument))
                        {
                            document.Add(new Paragraph("Hello world!"));
                        }
                    }
                }
                return ms.ToArray();
            }
            
        }
    }
}
