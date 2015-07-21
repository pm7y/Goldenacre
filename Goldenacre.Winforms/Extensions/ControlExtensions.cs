using System;
using System.Windows.Forms;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class ControlExtensions
    {
        /// <summary>
        ///     Invokes the action on the control using the same thread as the UI thread.
        /// </summary>
        /// <param name="control">The control to invoke from.</param>
        /// <param name="handler">The code to execute.</param>
        public static void InvokeThreadSafe(this Control control, Action handler)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(handler);
            }
            else
            {
                handler();
            }
        }
    }
}