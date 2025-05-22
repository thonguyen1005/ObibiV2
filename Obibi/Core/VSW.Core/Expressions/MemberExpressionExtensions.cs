using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace VSW.Core
{
    public static class MemberExpressionExtensions
    {
        public static Tuple<string, string, PropertyInfo> GetInfo(this MemberExpression expression)
        {
            //string param = null;

            //if (expression.Expression is ParameterExpression)
            //{
            //    param = (expression.Expression as ParameterExpression).Name;
            //}
            //else if (expression.Expression is MemberExpression)
            //{
            //    var memberParent = expression.Expression is MemberExpression
            //}

            var param = expression.ParamName();
            var memberName = expression.FullName();

            var property = expression.Member as PropertyInfo;

            return new Tuple<string, string, PropertyInfo>(param, memberName, property);
        }

        public static bool HasParameterExpression(this MemberExpression expression)
        {
            if (expression.Expression is ParameterExpression)
            {
                return true;
            }
            else if (expression.Expression is MemberExpression)
            {
                var exp = expression.Expression as MemberExpression;
                return exp.HasParameterExpression();
            }

            return false;
        }

        public static MemberExpression CreateMemberExpression(ParameterExpression param, string propName)
        {
            var propInfo = TypeManager.GetProperty(param.Type, propName);
            if (propInfo == null)
            {
                throw new Exception("[DEBUG] Property {0} not exist in Type {1}".Format(propName, param.Type.FullName));
            }

            return Expression.Property(param, propInfo.Property);
        }

        public static MemberExpression CreateMemberExpression(Type t, string propName, string paramName = ExpressionExtensions.DEFAULT_PARAM)
        {
            var param = ExpressionExtensions.GetParameterExpression(t, paramName);
            return CreateMemberExpression(param, propName);
        }

        public static LambdaExpression CreateLambdaExpression(Type t, Type returnType, string propName, string paramName = ExpressionExtensions.DEFAULT_PARAM)
        {
            var parameter = ExpressionExtensions.GetParameterExpression(t, paramName);
            return CreateLambdaExpression(parameter, returnType, propName);
        }

        public static LambdaExpression CreateLambdaExpression(ParameterExpression parameter, Type returnType, string propName)
        {
            var t = parameter.Type;
            Expression memberExpression = CreateMemberExpression(parameter, propName);

            if (memberExpression.Type.IsValueType)
            {
                memberExpression = Expression.Convert(memberExpression, returnType);
            }
            var delegateType = typeof(Func<,>).MakeGenericType(t, returnType);

            var lbaExp = ExpressionExtensions.GetExpressionLambdaMethod();
            var methodInfo = lbaExp.MakeGenericMethod(delegateType);
            var lambda = methodInfo.Invoke(null, new object[] { memberExpression, new[] { parameter } });

            return (LambdaExpression)lambda;
        }

        public static Expression<Func<T, T1>> GetExpression<T, T1>(string propName)
        {
            return (Expression<Func<T, T1>>)CreateLambdaExpression(typeof(T), typeof(T1), propName);
        }
    }
}
