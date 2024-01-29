using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Exceptions
{
    /// <summary>
    /// Decrypt Exception class
    /// </summary>
    public class DecryptException : Exception
    {
        /// <summary>
        /// Init class with message
        /// </summary>
        /// <param name="message">Exception's message</param>
        public DecryptException(String message) : base(message) { }

    }
}
