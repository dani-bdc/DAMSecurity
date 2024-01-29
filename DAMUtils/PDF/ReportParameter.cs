using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.PDF
{
    /// <summary>
    /// Report Parameter Class
    /// </summary>
    public class ReportParameter
    {
        /// <summary>
        ///  Report Parameter Name
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// If the parameter is Mandatory or not
        /// </summary>
        public bool Mandatory { get; set; } = false;

        /// <summary>
        ///  Report Parameter Value
        /// </summary>
        public object? Value { get; set; }
    }
}
