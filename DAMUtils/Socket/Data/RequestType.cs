using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.Socket.Data
{
    /// <summary>
    /// Socket request type
    /// </summary>
    public enum RequestType
    {
        None = 0,
        ListReports = 1,
        GetReport = 2
    }
}
