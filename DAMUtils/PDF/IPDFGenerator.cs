using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.PDF
{
    public interface IPDFGenerator
    {
        /// <summary>
        /// Generate PDF Reports 
        /// </summary>
        /// <param name="prms">Parameters to generate PDF Report</param>
        /// <returns>Report's byte array</returns>
        public byte[] Generate(object prms);

        /// <summary>
        /// Gets list of available reports
        /// </summary>
        /// <returns>List of available reports</returns>
        public List<ReportInfo> AvailableReports();
    }
}
