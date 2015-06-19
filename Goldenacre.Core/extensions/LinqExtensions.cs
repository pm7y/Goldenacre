using System;
using System.Linq.Expressions;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class LinqExtensions
    {
        public static string MemberNameOf(this LambdaExpression @this)
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

            return nameSelector(@this.Body);
        }
    }
}