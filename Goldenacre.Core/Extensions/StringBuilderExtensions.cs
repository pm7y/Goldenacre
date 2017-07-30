using System;
using System.Text;

namespace Goldenacre.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendIfNotNullOrWhiteSpace(this StringBuilder @this, string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                @this.Append(text);
            }

            return @this;
        }

        public static StringBuilder AppendLineIfNotNullOrWhiteSpace(this StringBuilder @this, string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                @this.AppendLine(text);
            }

            return @this;
        }

        public static StringBuilder AppendIf(this StringBuilder @this, bool condition, string text)
        {
            if (condition)
            {
                @this.Append(text);
            }

            return @this;
        }

        public static StringBuilder AppendLineIf(this StringBuilder @this, bool condition, string text)
        {
            if (condition)
            {
                @this.AppendLine(text);
            }

            return @this;
        }

        public static StringBuilder AppendIf(this StringBuilder @this, Func<bool> condition, string text)
        {
            if (condition())
            {
                @this.AppendLine(text);
            }

            return @this;
        }

        public static StringBuilder AppendLineIf(this StringBuilder @this, Func<bool> condition, string text)
        {
            if (condition())
            {
                @this.AppendLine(text);
            }

            return @this;
        }
    }
}