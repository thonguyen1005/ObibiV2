using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace VSW.Core
{
    public static class LogicalExpressionExtensions
    {
        public static LambdaExpression Or(this LambdaExpression expr1, LambdaExpression expr2)
        {
            return Expression.Lambda(Expression.OrElse(expr1.Body, expr2.Body), expr1.Parameters);
        }

        public static LambdaExpression And(this LambdaExpression expr1, LambdaExpression expr2)
        {
            return Expression.Lambda(Expression.AndAlso(expr1.Body, expr2.Body), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, expr2.Body), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, expr2.Body), expr1.Parameters);
        }
    }
}
