using System;

namespace VXI_GAMS_US.HELPER
{
    /// <summary>
    /// Handles exception detailing
    /// </summary>
    public class ExceptionHandler
    {
        /// <summary>
        /// The current exception thrown
        /// </summary>
        public static Exception Error;

        /// <summary>
        /// [Recursive] - Get all the exception information even inner exceptions
        /// </summary>
        /// <param name="ex">The exception to extract</param>
        /// <param name="currentMessage">This will contain all extracted exceptions on the loop</param>
        /// <param name="inner">for inner exceptions</param>
        /// <returns>The complete information about the exception</returns>
        public static string GetMessages(Exception ex, string currentMessage = "", string inner = "")
        {
            if (inner == null)
            {
                return currentMessage.Trim();
            }

            if (ex == null)
            {
                return currentMessage.Trim();
            }

            currentMessage = currentMessage + ex.Message + Environment.NewLine;
            if (ex.InnerException == null)
            {
                return currentMessage.Trim();
            }

            inner = GetMessages(ex.InnerException);
            if (!currentMessage.Contains(inner.Trim()))
            {
                currentMessage += inner;
            }

            return currentMessage.Trim();
        }

        /// <summary>
        /// List of fixed specific filter scenarios
        /// </summary>
        public static readonly Filter[] Filters =
        {
            new Filter(400, "Bad Request. The server could not understand the request you are trying to make"),
            new Filter(401, "Your credential does not have access to this website"),
            new Filter(404, "This page doesn't exist"),
            new Filter(500, "Internal Server Error"),
            new Filter(600, "The remote name could not be resolved"),
            new Filter(601, "Cannot connect to Global Web Service"),
            new Filter(602, "Access Denied"),
            new Filter(603, "Hi as for now, we are only supporting the modern browsers like Google Chrome, Opera and Mozilla Firefox"),
        };

        /// <summary>
        /// Used in Filters array
        /// </summary>
        public class Filter
        {
            /// <summary>
            /// Add a new instance of filter
            /// </summary>
            /// <param name="code">Status Code</param>
            /// <param name="message">Status Message</param>
            public Filter(int code, string message)
            {
                Code = code;
                Message = message;
            }
            /// <summary>
            /// Status code of the filter
            /// </summary>
            public int Code { get; set; }
            /// <summary>
            /// Error message for the given error code
            /// </summary>
            public string Message { get; set; }
        }
    }
}
