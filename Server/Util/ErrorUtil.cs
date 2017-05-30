using log4net;
using System;
using System.Diagnostics;

namespace VideoPaintballServer.Util
{
    public static class ErrorUtil
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ClientListener));

        /// <summary>
        /// Writes out an error to trace, nicely formatted, including all inner exceptions
        /// </summary>
        /// <param name="ex">The exception to write</param>
        /// <param name="origin">The origin of the exception (custom field for debugging info)</param>
        public static void WriteError(Exception ex, string origin)
        {
            _log.Error(origin, ex);

            _log.Error("\n An error occurred: " + ex.Message);
            _log.Error("\n \t Stack Trace: " + ex.StackTrace);
            _log.Error("\n \t Error Type: " + ex.GetType().ToString());
            System.Text.StringBuilder indents = new System.Text.StringBuilder(8);
            string indentString = null;
            while ((ex = ex.InnerException) != null)
            {
                indents.Append("\t");
                indentString = indents.ToString();
                _log.Error("\n" + indentString + "\t Inner Exception: " + ex.Message);
                _log.Error("\n" + indentString + "\t Inner Exception Stack Trace: " + ex.StackTrace);
                _log.Error("\n" + indentString + "\t Inner Exception Error Type: " + ex.GetType().ToString());
            }
            _log.Error("\tOrigin: " + origin);
        }

        /// <summary>
        /// Writes out an error to trace, nicely formatted, including all inner exceptions 
        /// </summary>
        /// <param name="ex">The exception to write</param>
        public static void WriteError(Exception ex)
        {
            WriteError(ex, string.Empty);
        }
    }
}
