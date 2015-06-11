using System;
using System.Linq.Expressions;
using System.Windows.Forms;
using Goldenacre.Extensions;

namespace Goldenacre.Winforms.Extensions
{
    public static class ControlExtensions
    {

        /// <summary>
        /// Invokes the action on the control using the same thread as the UI thread.
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

        /// <summary>Databinding with strongly typed object names</summary>
        /// <param name="control">The Control you are binding to</param>
        /// <param name="controlProperty">The property on the control you are binding to</param>
        /// <param name="dataSource">The object you are binding to</param>
        /// <param name="dataSourceProperty">The property on the object you are binding to</param>
        public static Binding Bind<TControl, TDataSourceItem>(this TControl control,
            Expression<Func<TControl, object>> controlProperty, object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty)
            where TControl : Control
        {
            return control.DataBindings.Add(controlProperty.MemberNameOf(), dataSource,
                dataSourceProperty.MemberNameOf());
        }

        public static Binding Bind<TControl, TDataSourceItem>(this TControl control,
            Expression<Func<TControl, object>> controlProperty, object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty, bool formattingEnabled)
            where TControl : Control
        {
            return control.DataBindings.Add(controlProperty.MemberNameOf(), dataSource,
                dataSourceProperty.MemberNameOf(), formattingEnabled);
        }

        public static Binding Bind<TControl, TDataSourceItem>(this TControl control,
            Expression<Func<TControl, object>> controlProperty, object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty, bool formattingEnabled,
            DataSourceUpdateMode updateMode)
            where TControl : Control
        {
            return control.DataBindings.Add(controlProperty.MemberNameOf(), dataSource,
                dataSourceProperty.MemberNameOf(), formattingEnabled, updateMode);
        }

        public static Binding Bind<TControl, TDataSourceItem>(this TControl control,
            Expression<Func<TControl, object>> controlProperty, object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty, bool formattingEnabled,
            DataSourceUpdateMode updateMode, object nullValue)
            where TControl : Control
        {
            return control.DataBindings.Add(controlProperty.MemberNameOf(), dataSource,
                dataSourceProperty.MemberNameOf(), formattingEnabled, updateMode, nullValue);
        }

        public static Binding Bind<TControl, TDataSourceItem>(this TControl control,
            Expression<Func<TControl, object>> controlProperty, object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty, bool formattingEnabled,
            DataSourceUpdateMode updateMode, object nullValue, string formatString)
            where TControl : Control
        {
            return control.DataBindings.Add(controlProperty.MemberNameOf(), dataSource,
                dataSourceProperty.MemberNameOf(), formattingEnabled, updateMode, nullValue, formatString);
        }

        public static Binding Bind<TControl, TDataSourceItem>(this TControl control,
            Expression<Func<TControl, object>> controlProperty, object dataSource,
            Expression<Func<TDataSourceItem, object>> dataSourceProperty, bool formattingEnabled,
            DataSourceUpdateMode updateMode, object nullValue, string formatString, IFormatProvider formatInfo)
            where TControl : Control
        {
            return control.DataBindings.Add(controlProperty.MemberNameOf(), dataSource,
                dataSourceProperty.MemberNameOf(), formattingEnabled, updateMode, nullValue, formatString, formatInfo);
        }
    }
}