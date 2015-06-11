using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
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
            var valueName = value.MemberNameOf();
            var captionName = caption.MemberNameOf();
            var items = collection.ToArray();

            if (selected != null)
            {
                var selectedValue = items.Where(selected.Compile()).Select(value.Compile()).FirstOrDefault();

                if (selectedValue != null)
                {
                    return new SelectList(items, valueName, captionName, selectedValue);
                }
            }

            return new SelectList(items, valueName, captionName);
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
            var valueName = value.MemberNameOf();
            var captionName = caption.MemberNameOf();
            var items = collection.ToArray();

            if (selected != null)
            {
                var selectedItems = items.Where(selected.Compile()).Select(value.Compile());

                return new MultiSelectList(items, valueName, captionName, selectedItems.ToArray());
            }

            return new MultiSelectList(items, valueName, captionName);
        }
    }
}