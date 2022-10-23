using System;
using System.Linq;
using System.Linq.Expressions;

namespace NX.Expressions
{
    public static class ExpressionHelper
    {
        /// <summary>
        /// 複数の <c>(x: T) => bool</c> 形式の Lambda の条件部分を <c>AND (&&)</c> で結合した新しい Lambda に変換します。
        /// </summary>
        public static Expression<Func<T, bool>> AndAlso<T>(Expression<Func<T, bool>> e1, Expression<Func<T, bool>> e2,
            params Expression<Func<T, bool>>[] expressions)
        {
            var parameter = new ParameterReplacer<T>();
            var body = Expression.AndAlso(e1.Body, e2.Body);

            foreach (var e in expressions)
            {
                body = Expression.AndAlso(body, e.Body);
            }

            return Expression.Lambda<Func<T, bool>>(parameter.Visit(body)!, parameter.Parameter);
        }

        /// <summary>
        /// 複数の <c>(x: T) => bool</c> 形式の Lambda の条件部分を <c>OR (||)</c> で結合した新しい Lambda に変換します。
        /// </summary>
        public static Expression<Func<T, bool>> OrElse<T>(Expression<Func<T, bool>> e1, Expression<Func<T, bool>> e2,
            params Expression<Func<T, bool>>[] expressions)
        {
            var parameter = new ParameterReplacer<T>();
            var body = Expression.OrElse(e1.Body, e2.Body);

            foreach (var e in expressions)
            {
                body = Expression.OrElse(body, e.Body);
            }

            return Expression.Lambda<Func<T, bool>>(parameter.Visit(body)!, parameter.Parameter);
        }
    }
}
