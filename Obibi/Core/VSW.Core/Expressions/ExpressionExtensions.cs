using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace VSW.Core
{
    public static class ExpressionExtensions
    {
        public const string DEFAULT_PARAM = "x";

        public static object GetValue(Expression exp)
        {
            if (exp is ConstantExpression)
            {
                return (exp as ConstantExpression).Value;
            }
            else
            {
                return Expression.Lambda(exp).Compile().DynamicInvoke();
            }
        }

        public static Func<PropertyInfo, bool> GetPrimitivePropertiesPredicate()
        {
            return p => p.CanWrite && (p.PropertyType.IsValueType || p.PropertyType == typeof(string) || p.PropertyType == typeof(byte[]));
        }

        public static ParameterExpression GetParameterExpression(Type t, string name = DEFAULT_PARAM)
        {
            return Expression.Parameter(t, name);
        }

        public static Expression IgnoreConvertType(this Expression exp)
        {
            if (exp.NodeType == ExpressionType.Convert)
            {
                var ue = exp as UnaryExpression;
                return ((ue != null) ? ue.Operand : null);
            }

            return exp;
        }

        public static LambdaExpression ChangeParameter(this LambdaExpression expr1, ParameterExpression param)
        {
            return Expression.Lambda(expr1.Body, param);
        }

        public static Expression<Func<T, bool>> ChangeParameter<T>(this Expression<Func<T, bool>> expr1, ParameterExpression param)
        {
            return Expression.Lambda<Func<T, bool>>
                  (expr1.Body, param);
        }

        public static ParameterExpression GetParameterExpression<T>()
        {
            return GetParameterExpression(typeof(T));
        }

        public static Expression<Func<T2, bool>> Convert<T1, T2>(this Expression<Func<T1, bool>> exp)
        {
            if (exp == null)
            {
                return null;
            }

            return Expression.Lambda<Func<T2, bool>>(exp.Body, exp.Parameters);
        }

        public static Expression<Func<T2, object>> Convert<T1, T2>(this Expression<Func<T1, object>> exp)
        {
            if (exp == null)
            {
                return null;
            }

            return Expression.Lambda<Func<T2, object>>(exp.Body, exp.Parameters);
        }

        public static Expression<Func<T2, TPropert>> Convert<T1, T2, TPropert>(this Expression<Func<T1, TPropert>> exp)
        {
            if (exp == null)
            {
                return null;
            }

            return Expression.Lambda<Func<T2, TPropert>>(exp.Body, exp.Parameters);
        }

        /// <summary>
        /// Get Full Name Of Property: Name1.Prop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string FullName<T>(this Expression<Func<T, object>> exp)
        {
            LambdaExpression lambda = exp;
            return FullName(lambda);
        }

        public static string FullName(this LambdaExpression exp)
        {
            if (exp == null)
            {
                return null;
            }

            MemberExpression member = null;
            if (exp.Body is UnaryExpression)
            {
                var temp = exp.Body as UnaryExpression;
                if (temp.Operand is MemberExpression)
                {
                    member = temp.Operand as MemberExpression;
                }
            }
            else if (exp.Body is MemberExpression)
            {
                member = exp.Body as MemberExpression;
            }

            if (member != null)
            {
                var memberPath = member.ToString();
                var parameterName = exp.Parameters.IsNotEmpty() ? exp.Parameters.First().Name : "";
                var iFirstSeparateSymbol = memberPath.IndexOf(StringConst.PROPERTY_SEPARATE_TOKEN);
                if (iFirstSeparateSymbol >= 0 && memberPath.StartsWith(parameterName + StringConst.PROPERTY_SEPARATE_TOKEN))
                {
                    return memberPath.Substring(iFirstSeparateSymbol + 1);
                }

                return memberPath;
            }

            return "";
        }

        public static string Name(this LambdaExpression exp)
        {
            if (exp == null)
            {
                return null;
            }

            MemberExpression member = null;
            if (exp.Body is UnaryExpression)
            {
                var temp = exp.Body as UnaryExpression;
                if (temp.Operand is MemberExpression)
                {
                    member = temp.Operand as MemberExpression;
                }
            }
            else if (exp.Body is MemberExpression)
            {
                member = exp.Body as MemberExpression;
            }

            if (member != null)
            {
                var memberPath = member.ToString();
                return memberPath.GetNameOfClass();
            }

            return "";
        }

        public static string Name(this MemberExpression exp)
        {
            return exp.Member.Name;
        }

        public static string FullName(this MemberExpression exp)
        {
            var memberPath = exp.ToString();
            var iFirstSeparateSymbol = memberPath.IndexOf(StringConst.PROPERTY_SEPARATE_TOKEN);
            if (iFirstSeparateSymbol > 0)
            {
                return memberPath.Substring(iFirstSeparateSymbol + 1);
            }

            return memberPath;
        }

        public static string ParamName(this MemberExpression exp)
        {
            var memberPath = exp.ToString();
            var iFirstSeparateSymbol = memberPath.IndexOf(StringConst.PROPERTY_SEPARATE_TOKEN);
            if (iFirstSeparateSymbol > 0)
            {
                return memberPath.Substring(0, iFirstSeparateSymbol);
            }

            return "";
        }

        /// <summary>
        /// Get Full Name Of Property: Name1.Prop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string Name<T>(this Expression<Func<T, object>> exp)
        {
            LambdaExpression lambda = exp;
            return Name(lambda);
        }

        /// <summary>
        /// Get Full Name Of Property: Name1.Prop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string FullName<T, TProperty>(this Expression<Func<T, TProperty>> exp)
        {
            LambdaExpression lambda = exp;
            return FullName(lambda);
        }

        /// <summary>
        /// Get Full Name Of Property: Name1.Prop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string Name<T, TProperty>(this Expression<Func<T, TProperty>> exp)
        {
            LambdaExpression lambda = exp;
            return Name(lambda);
        }

        public static string StaticFullName(this Expression<Func<object, object>> exp)
        {
            var key = exp.FullName().Split(StringConst.PROPERTY_SEPARATE_TOKEN).LastOrDefault();
            var body = exp.Body as MemberExpression;
            key = body.Member.ReflectedType.FullName.Replace("+", StringConst.PROPERTY_SEPARATE_TOKEN) + StringConst.PROPERTY_SEPARATE_TOKEN + key;
            return key;
        }

        public static string GetUniqueKey(this ExpressionType type, string methodName = "")
        {
            var rs = type.GetType().FullName + StringConst.PROPERTY_SEPARATE_TOKEN + type.GetName();
            if (methodName.IsNotEmpty())
            {
                rs += StringConst.PROPERTY_SEPARATE_TOKEN + methodName;
            }

            return rs;
        }

        public static MethodInfo GetExpressionLambdaMethod()
        {
            return typeof(Expression)
                .GetMethods()
                .Where(m => m.Name == "Lambda")
                .Select(m => new
                {
                    Method = m,
                    Params = m.GetParameters(),
                    Args = m.GetGenericArguments()
                })
                .Where(x => x.Params.Length == 2 && x.Args.Length == 1)
                .Select(x => x.Method)
                .First();
        }

        public static Expression<Func<T, bool>> BuildPrimaryEqual<T>(string name, object value) where T : class
        {
            var param = GetParameterExpression(typeof(T));
            var member = MemberExpressionExtensions.CreateMemberExpression(param, name);
            Expression constant = Expression.Constant(value);
            var exp = Expression.Equal(member, constant);
            return Expression.Lambda<Func<T, bool>>(exp, param);
        }
    }
}
