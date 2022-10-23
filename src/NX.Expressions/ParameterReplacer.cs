using System.Linq.Expressions;

namespace NX.Expressions
{
    /// <summary>
    /// Expressionの引数を固定値に変換するクラス
    /// </summary>
    /// <typeparam name="T">引数の型</typeparam>
    internal class ParameterReplacer<T> : ExpressionVisitor
    {
        public ParameterExpression Parameter { get; }

        public ParameterReplacer(ParameterExpression? p = null)
        {
            Parameter = p ?? Expression.Parameter(typeof(T), "x");
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return Parameter;
        }
    }
}
