using System;
using System.Text;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class ExceptionExtentions
    {
        /// <summary>
        ///     <para>Creates a log-string from the Exception.</para>
        ///     <para>
        ///         The result includes the stacktrace, innerexception et cetera, separated by
        ///         <seealso cref="Environment.NewLine" />.
        ///     </para>
        /// </summary>
        /// <param name="ex">The exception to create the string from.</param>
        /// <param name="additionalMessage">Additional message to place at the top of the string, maybe be empty or null.</param>
        /// <returns></returns>
        public static string ToLogString(this Exception @this, string additionalMessage = null)
        {
            var msg = new StringBuilder();

            msg.AppendLine(@this.GetType().FullName);
            msg.AppendLine(DateTime.Now.ToSqlString());
            msg.AppendLine("");

            msg.AppendLineIfNotNullOrWhiteSpace(additionalMessage.Trim() + Environment.NewLine);

            var orgEx = @this;

            while (orgEx != null)
            {
                msg.AppendLine(orgEx.Message);
                msg.AppendLineIfNotNullOrWhiteSpace(orgEx.HelpLink);
                orgEx = orgEx.InnerException;
            }

            foreach (var i in @this.Data)
            {
                msg.AppendLine("Data :");
                msg.AppendLine(i.ToString());
            }

            if (!string.IsNullOrWhiteSpace(@this.StackTrace))
            {
                msg.AppendLine("");
                msg.AppendLine(@this.StackTrace.Trim());
            }

            if (@this.TargetSite != null)
            {
                msg.AppendLine("");
                if (@this.TargetSite.DeclaringType != null)
                {
                    msg.Append(@this.TargetSite.DeclaringType.FullName);
                }
                msg.Append(@this.TargetSite.Name);
                msg.AppendLine("");
            }
            return msg.ToString().Trim();
        }
    }
}