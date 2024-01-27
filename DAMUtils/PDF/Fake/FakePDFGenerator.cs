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
        /// <summary>
        /// Generate list of available reports
        /// </summary>
        /// <returns>List of avaiable reports</returns>
        List<ReportInfo> IPDFGenerator.AvailableReports()
        {
            List<ReportInfo> list = new List<ReportInfo>();
            ReportInfo report;
            ReportParameter param;

            report = new ReportInfo();
            report.Name = "Report1";
            list.Add(report);

            report = new ReportInfo();
            report.Name = "Report2";

            param = new ReportParameter();
            param.Mandatory = true;
            param.Name = "Param1";
            report.Parameters.Add(param);

            list.Add(report);

            return list;
        }

        /// <summary>
        ///  Generate report passed by param
        /// </summary>
        /// <param name="prms"></param>
        /// <returns></returns>
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
