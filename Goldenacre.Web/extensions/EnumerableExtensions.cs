using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Goldenacre.Web.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Create a select list from a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of each element in the collection.</typeparam>
        /// <param name="collection">The collection of items that will populate the select list.</param>
        /// <param name="caption">An expression to get the object property who's value will become the caption.</param>
        /// <param name="value">An expression to get the object property who's value will become the value.</param>
        /// <param name="selected">
        ///     An expression which determines which item will be selected. If the expression returns multiple
        ///     items then the first one is used.
        /// </param>
        /// <returns>A SelectList.</returns>
        public static SelectList ToSelectList<T>(
            this IEnumerable<T> collection,
            Expression<Func<T, object>> caption,
            Expression<Func<T, object>> value,
            Expression<Func<T, bool>> selected = null) where T : class
        {
            var valueName = value.GetMemberName();
            var captionName = caption.GetMemberName();

            if (selected != null)
            {
                var selectedValue = collection.Where(selected.Compile()).Select(value.Compile()).FirstOrDefault();

                if (selectedValue != null)
                {
                    return new SelectList(collection, valueName, captionName, selectedValue);
                }
            }

            return new SelectList(collection, valueName, captionName);
        }

        /// <summary>
        ///     Create a multi-select list from a collection of objects.
        /// </summary>
        /// <typeparam name="T">The type of each element in the collection.</typeparam>
        /// <param name="collection">The collection of items that will populate the select list.</param>
        /// <param name="caption">An expression to get the object property who's value will become the caption.</param>
        /// <param name="value">An expression to get the object property who's value will become the value.</param>
        /// <param name="selected">An expression which determines which items will be selected.</param>
        /// <returns>A MultiSelectList.</returns>
        public static MultiSelectList ToMultiSelectList<T>(
            this IEnumerable<T> collection,
            Expression<Func<T, object>> caption,
            Expression<Func<T, object>> value,
            Expression<Func<T, bool>> selected = null) where T : class
        {
            var valueName = value.GetMemberName();
            var captionName = caption.GetMemberName();

            if (selected != null)
            {
                var selectedItems = collection.Where(selected.Compile()).Select(value.Compile());

                return new MultiSelectList(collection, valueName, captionName, selectedItems.ToArray());
            }

            return new MultiSelectList(collection, valueName, captionName);
        }

        private static string GetMemberName(this LambdaExpression memberSelector)
        {
            Func<Expression, string> nameSelector = null;
            nameSelector = e =>
            {
                switch (e.NodeType)
                {
                    case ExpressionType.Parameter:
                        return ((ParameterExpression) e).Name;
                    case ExpressionType.MemberAccess:
                        return ((MemberExpression) e).Member.Name;
                    case ExpressionType.Call:
                        return ((MethodCallExpression) e).Method.Name;
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        return nameSelector(((UnaryExpression) e).Operand);
                    case ExpressionType.Invoke:
                        return nameSelector(((InvocationExpression) e).Expression);
                    case ExpressionType.ArrayLength:
                        return "Length";
                    default:
                        throw new Exception("not a proper member selector");
                }
            };

            return nameSelector(memberSelector.Body);
        }
    }
}