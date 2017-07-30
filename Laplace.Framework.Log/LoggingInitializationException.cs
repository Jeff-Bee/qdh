using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laplace.Framework.Log
{
    /// <summary>
    /// An exception that is thrown when there is a problem with logging initialization.
    /// </summary>
    public class LoggingInitializationException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="LoggingInitializationException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The exception which caused this exception to be thrown.</param>
        public LoggingInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructs a new <see cref="LoggingInitializationException"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        public LoggingInitializationException(string message)
            : this(message, null)
        {
        }
    }
}
