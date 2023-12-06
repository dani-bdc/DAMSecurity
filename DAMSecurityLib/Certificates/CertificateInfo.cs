using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Certificates
{
    /// <summary>
    /// This class  establishes the necessary properties to obtain the information of the generated
    /// digital certificates
    /// </summary>
    public class CertificateInfo
    {
        /// <summary>
        /// Certificate CommonName
        /// This property is mandatory and its default value is SelfSignedCert
        /// </summary>
        public string CommonName { get; set; } = "SelfSignedCert";
        
        /// <summary>
        /// Certificate Organization Name
        /// This property is optional
        /// </summary>
        public string Organization { get; set;} = "UserName";
        
        /// <summary>
        /// Certificate Organization Locality
        /// This property is optional
        /// </summary>
        public string? Locality { get; set; } = null;

        /// <summary>
        /// Certificate Organization State
        /// This property is optional
        /// </summary>
        public string? State { get; set;} = null;

        /// <summary>
        /// Certification Organization Country
        /// This property is optional
        /// </summary>
        public string? Country { get; set; } = null;

        /// <summary>
        /// Certification Organization Email
        /// This property is optional
        /// </summary>
        public string? Email { get; set;} = null;

        /// <summary>
        /// Certification Organization Street Address
        /// This property is optional
        /// </summary>
        public string? Address { get;set; } = null;

        /// <summary>
        /// Certification Origanization Postal Code
        /// This property is optional
        /// </summary>
        public string? PostalCode { get; set;} = null;

        /// <summary>
        /// Certification Initial validation DateTimeOffset Period
        /// </summary>
        public DateTimeOffset NotBefore { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Certification final validation DatetimeOffset Period
        /// The certification doesn't work after this time
        /// </summary>
        public DateTimeOffset NotAfter { get; set; } = DateTimeOffset.UtcNow.AddYears(1);

        /// <summary>
        /// Default Certificate DistinguishedName 
        /// This propoerty is created from the others properties
        /// </summary>
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
