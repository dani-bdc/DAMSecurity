using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.PDF
{
    /// <summary>
    /// Class representing information about report
    /// </summary>
    public class ReportInfo
    {
        /// <summary>
        /// Report Name
        /// </summary>
        public string Name { get; set; } = "";
        
        /// <summary>
        /// Parameters available to class report
        /// </summary>
        public List<ReportParameter> Parameters { get; set; } = new List<ReportParameter>();
    }
}
