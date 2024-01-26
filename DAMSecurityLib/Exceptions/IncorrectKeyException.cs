using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Exceptions
{

    /// <summary>
    /// Incorrect Key Exception
    /// </summary>
    public class IncorrectKeyException : Exception
    {
        /// <summary>
        /// Iniit class with message
        /// </summary>
        /// <param name="message">Exception's message</param>
        public IncorrectKeyException(String message) : base(message) { }

    }
}
