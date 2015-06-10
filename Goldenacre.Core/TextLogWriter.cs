using System;
using System.Diagnostics;
using System.IO;

namespace Goldenacre.Core
{
    /// <author>Paul Mcilreavy</author>
    /// <summary>
    ///     Static Class for logging messages to a text file.
    /// </summary>
    public static class TextLogWriter
    {
        /// <summary>
        ///     Removes new line breaks for a string so that it is just one line.
        /// </summary>
        /// <param name="textLine">the string to parse</param>
        /// <returns>a string with no line breaks</returns>
        private static string RemoveLineBreaks(string textLine)
        {
            return RemoveLineBreaks(textLine, false);
        }

        /// <summary>
        ///     Removes new line breaks for a string so that it is just one line.
        /// </summary>
        /// <param name="textLine">the string to parse</param>
        /// <param name="replaceLineBreaksWithArrows">if True then line breaks will be replaced with "-->"</param>
        /// <returns>a string with no line breaks</returns>
        private static string RemoveLineBreaks(string textLine, bool replaceLineBreaksWithArrows)
        {
            string retVal = null;

            if (textLine == null)
            {
                throw new ArgumentException("Cannot be null string", "textLine");
            }

            if (replaceLineBreaksWithArrows)
            {
                retVal = textLine.Replace("\n", " --> ")
                    .Replace("\r", " --> ")
                    .Replace("  ", " ")
                    .Replace("--> -->", "-->");
            }
            else
            {
                retVal = textLine.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
            }

            return retVal;
        }

        /// <author>Paul Mcilreavy</author>
        /// <date>13/06/2007</date>
        /// <summary>
        ///     writes an entry to the debug log
        /// </summary>
        /// <param name="message">message to log</param>
        public static void NewDebugLogEntry(string message)
        {
            const int STACK_FRAME_INDEX = 3;

            string logTime = null;
            string logMessage = null;
            string debugLogPath = null;
            var logFound = false;
            TraceListener tr = null;

            string callingMethod = null;
            string logLine = null;

            logTime = GetLogTimeString();
            logMessage = message.Replace(",", string.Empty);
            debugLogPath = Path.Combine(Environment.CurrentDirectory, "debugoutput.txt");

            callingMethod = GetRaisingMethodName(STACK_FRAME_INDEX);
            logLine = string.Format("{0},{1},'{2}'", logTime, callingMethod, RemoveLineBreaks(message, true));

            // iterate through the Listeners collection
            // and check if we have already got a trace log
            // for this message type
            foreach (TraceListener tl in Trace.Listeners)
            {
                if (tl.Name == "TextLogWriter")
                {
                    // we already have a trace log for this message type
                    logFound = true;
                    break;
                }
            }

            // a trace log for this message type was not found
            // so create one and add it to the collection
            tr = new DefaultTraceListener();
            if (logFound == false)
            {
                tr = new TextWriterTraceListener(debugLogPath);
                tr.Name = "TextLogWriter";
                Trace.Listeners.Add(tr);
            }
            else
            {
                tr = Trace.Listeners["TextLogWriter"];
            }


            tr.WriteLine(logLine);
            tr.Flush();
        }

        /// <author>Paul Mcilreavy</author>
        /// <date>13/06/2007</date>
        /// <summary>
        ///     returns a formatted string containing
        ///     todays date and time and milliseconds
        ///     i.e. 01/01/2000 12:00:00.000
        /// </summary>
        /// <returns></returns>
        private static string GetLogTimeString()
        {
            var logTimeNow = DateTime.Now;
            return logTimeNow + "." + string.Format("{0:00#}", logTimeNow.Millisecond);
        }

        /// <author>Paul Mcilreavy</author>
        /// <date>13/06/2007</date>
        /// <summary>
        ///     return the name of the function that called it.
        /// </summary>
        /// <param name="frameIndex">how many hops we need to go back in the stack</param>
        /// <returns>string</returns>
        private static string GetRaisingMethodName(int frameIndex)
        {
            StackTrace objStackTrace = null;
            var strMethodName = string.Empty;

            try
            {
                objStackTrace = new StackTrace(true);
                strMethodName = objStackTrace.GetFrame(frameIndex).GetMethod().Name + "()";
            }
            catch (Exception ee)
            {
                // Don't rethrow cos this method is called by catch blocks in other
                // methods so throw an error here could create an infinite loop.
                strMethodName = "Unknown: " + ee.Message;
            }
            finally
            {
                if (objStackTrace != null)
                {
                    objStackTrace = null;
                }
            }

            return strMethodName;
        }
    }
}