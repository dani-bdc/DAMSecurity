using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Certificates
{
    public class CertificateInfo
    {
        public string CommonName { get; set; } = "SelfSignedCert";
        public string? Organization { get; set;} = null;
        public string? Locality { get; set; } = null;
        public string? State { get; set;} = null;
        public string? Country { get; set; } = null;
        public string? Email { get; set;} = null;
        public string? Address { get;set; } = null;
        public string? PostalCode { get; set;} = null;
        
        public string DistinguishedName
        {
            get
            {
                string dn = $"CN={CommonName}";

                if (Organization != null)
                {
                    dn += $",={Organization}";
                }
                if (Locality != null)
                {
                    dn += $", L={Locality}";

                }
                if (State != null)
                {
                    dn += $", ST={State}";

                }
                if (Country != null)
                {
                    dn += $", C={Country}";
                }
                if (Email!=null)
                {
                    dn += $", Email={Email}";
                }
                if (Address != null)
                {
                    dn += $", StreetAddress={Address}";
                }
                if (PostalCode != null)
                {
                    dn += $", PostalCode={PostalCode}";
                }
                return dn;
            }
        }
    }
}
