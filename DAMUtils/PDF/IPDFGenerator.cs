using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.PDF
{
    public interface IPDFGenerator
    {
        public byte[] Generate(object prms);
    }
}
