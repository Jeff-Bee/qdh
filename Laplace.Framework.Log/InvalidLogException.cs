using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laplace.Framework.Log
{
    /// <summary>
    /// Thrown when a user attempts to write to in an invalid log.
    /// </summary>
    public class InvalidLogException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="InvalidLogException"/> object.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="logName">The name of the log.</param>
        /// <param name="innerException">The exception which caused this exception to be thrown.</param>
        public InvalidLogException(string message, string logName, Exception innerException)
            : base(message, innerException)
        {
            this.LogName = logName;
        }

        /// <summary>
        /// Constructs a new <see cref="InvalidLogException"/> object.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="logName">The name of the log.</param>
        public InvalidLogException(string message, string logName)
            : this(message, logName, null)
        {
        }

        /// <summary>
        /// The name of the log.
        /// </summary>
        public string LogName { get; private set; }
    }
}
