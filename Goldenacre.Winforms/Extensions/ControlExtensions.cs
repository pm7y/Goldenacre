using System;
using System.Collections.Generic;
using System.Windows.Forms;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class ControlExtensions
    {
        private static readonly object _invokeLock = new object();

        /// <summary>
        ///     Invokes the action on the control using the same thread as the UI thread.
        /// </summary>
        /// <param name="@this">The control to invoke from.</param>
        /// <param name="handler">The code to execute.</param>
        public static void InvokeThreadSafe(this Control @this, Action handler)
        {
            lock (_invokeLock)
            {
                if (@this.InvokeRequired)
                {
                    @this.Invoke(handler);
                }
                else
                {
                    handler();
                }
            }
        }

        /// <summary>
        ///     Get all the parent control of the specific control.
        /// </summary>
        /// <param name="@this">The specified control to return the parents of.</param>
        public static IEnumerable<Control> ParentHierarchy(this Control @this)
        {
            var c = @this == null ? default(Control) : @this.Parent;

            while (c != null)
            {
                yield return c;

                c = c.Parent;
            }
        }
    }
}