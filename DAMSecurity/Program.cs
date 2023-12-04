
using DAMSecurityLib.Certificates;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Collections.Specialized;

namespace DAMSecurity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string originalFileName = Path.ChangeExtension(Path.GetTempFileName(), "pdf");
            string signedFileName = Path.ChangeExtension(Path.GetTempFileName(), "pdf");

            using (var writer = new PdfWriter(new FileStream( originalFileName, FileMode.Create, FileAccess.Write)))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    Document document = new Document(pdf);
                    document.Add(new Paragraph("Pdf sample "));
                    document.Close();
                }
            }

            Console.WriteLine($"Original pdf file:{originalFileName}");

            Autosigned.SignPdfWithNewCertificate(originalFileName, signedFileName);

            Console.WriteLine($"Signed pdf file:{signedFileName}");
        }
    }
}