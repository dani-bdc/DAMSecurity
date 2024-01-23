using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Exceptions
{
    /// <summary>
    /// Exception throwed when the parameters are incorrect
    /// </summary>
    public class IncorrectParametersException : Exception
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">Message associated with the exception</param>
        public IncorrectParametersException(String message) : base(message) { }
    }
}
