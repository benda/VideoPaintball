using System;
using System.Diagnostics;

namespace VideoPaintballServer.Util
{
    public static class ErrorUtil
    {
        /// <summary>
        /// Writes out an error to trace, nicely formatted, including all inner exceptions
        /// </summary>
        /// <param name="ex">The exception to write</param>
        /// <param name="origin">The origin of the exception (custom field for debugging info)</param>
        public static void WriteError(Exception ex, string origin)
        {
            Trace.WriteLine("\n An error occurred: " + ex.Message);
            Trace.WriteLine("\n \t Stack Trace: " + ex.StackTrace);
            Trace.WriteLine("\n \t Error Type: " + ex.GetType().ToString());
            System.Text.StringBuilder indents = new System.Text.StringBuilder(8);
            string indentString = null;
            while ((ex = ex.InnerException) != null)
            {
                indents.Append("\t");
                indentString = indents.ToString();
                Trace.WriteLine("\n" + indentString + "\t Inner Exception: " + ex.Message);
                Trace.WriteLine("\n" + indentString + "\t Inner Exception Stack Trace: " + ex.StackTrace);
                Trace.WriteLine("\n" + indentString + "\t Inner Exception Error Type: " + ex.GetType().ToString());
            }
            Trace.WriteLine("\tOrigin: " + origin);
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
