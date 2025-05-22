using VSW.Core.Caching;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace VSW.Core
{
    public static class TypeManager
    {
        private static CachedTypes _cacheType = null;
        private const string TRUE_STRING = "true";
        private const string FALSE_STRING = "false";

        private static readonly Dictionary<Type, object> SYSTEM_TYPE = new Dictionary<Type, object>() {
                                                                    {typeof(String), null},
                                                                    {typeof(int), default(int)},
                                                                    {typeof(Int16), default(Int16)},
                                                                    {typeof(Int64), default(Int64)},
                                                                    {typeof(float), default(float)},
                                                                    {typeof(decimal), default(decimal)},
                                                                    {typeof(double), default(double)},
                                                                    {typeof(bool), default(bool)},
                                                                    {typeof(DateTime), DateTimeHelper.MIN}
                                                                    };


        public static void Init(Func<AssemblySetting> config)
        {
            _cacheType = new CachedTypes(config);
            _cacheType.Load();
        }

        internal static TypePropertyCollection GetPropertiesFromCached(this Type type)
        {
            return _cacheType.PropertyCache[type];
        }

        public static bool IsSystemType(this Type type)
        {
            return SYSTEM_TYPE.ContainsKey(type);
        }

        public static object DefaultValue(this Type type)
        {
            if (SYSTEM_TYPE.ContainsKey(type))
            {
                return SYSTEM_TYPE[type];
            }

            return null;
        }

        public static bool IsDefaultValue(this object value)
        {
            if (value == null)
            {
                return true;
            }
            var type = value.GetType();
            if (SYSTEM_TYPE.ContainsKey(type))
            {
                return SYSTEM_TYPE[type] == value;
            }

            return false;
        }


        /// <summary>
        /// Kiểm tra xem có phải Property là Prop lồng nhau không
        /// </summary>
        /// <param name="propPath"></param>
        /// <returns></returns>
        public static bool IsNestProp(this string propPath)
        {
            return propPath.Contains(StringConst.PROPERTY_SEPARATE_TOKEN);
        }

        /// <summary>
        /// Kiểm tra xem có phải Property là Prop lồng nhau không
        /// </summary>
        /// <param name="propPath"></param>
        /// <returns></returns>
        public static bool IsNestProp<T, TMember>(this Expression<Func<T, TMember>> propPath)
        {
            var fullName = propPath.FullName();
            return IsNestProp(fullName);
        }


        /// <summary>
        /// Change Type of Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T To<T>(this object obj, bool ignoreNullable = false)
        {
            if (obj == null || obj is DBNull)
            {
                return default(T);
            }

            var rs = (T)obj.To(typeof(T), ignoreNullable);
            return rs;
        }


        public static bool Is(this object obj, Type checkType)
        {
            if (obj == null || obj is DBNull)
            {
                return false;
            }

            var type = obj.GetType();

            if (type == checkType || checkType.IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Change Type of Object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static object To(this object obj, Type destType, bool ignoreNullable = false)
        {
            if (obj == null || obj is DBNull)
            {
                return null;
            }

            var type = obj.GetType();

            if (destType == type || destType.IsAssignableFrom(type))
            {
                return obj;
            }

            if (!ignoreNullable && type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (!destType.IsSystemType() && !destType.IsEnum)
            {
                return null;
            }

            if (obj is string)
            {
                var val = obj as string;
                //Nếu là True hoặc False cần convert sang kiểu số thì gán lại giá trị
                if (val.EqualsIgnoreCase(TRUE_STRING) || val.EqualsIgnoreCase(FALSE_STRING))
                {
                    if (destType.IsNumeric())
                    {
                        obj = val.EqualsIgnoreCase(TRUE_STRING) ? "1" : "0";
                    }
                }

                if (destType == typeof(Int32))
                {
                    if (val.IsEmpty())
                    {
                        return 0;
                    }
                    bool b = int.TryParse(val, out var r);
                    if (b)
                    {
                        return r;
                    }
                    return 0;
                }
                if (destType.IsEnum)
                {
                    return Enum.ToObject(destType, int.Parse(val));
                }
            }
            else
            {
                if (obj is JToken)
                {
                    return (obj as JToken).ParseFromToken(destType);
                }
                else if (obj is JObject)
                {
                    return (obj as JObject).ParseFromJObject(destType);
                }

                if (destType.IsEnum)
                {
                    return Enum.ToObject(destType, obj);
                }
            }
            // còn lại cứ gọi hàm chuyển đổi của hệ thống
            return Convert.ChangeType(obj, destType);
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsList(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static bool IsNumeric(this Type type)
        {
            return type == typeof(decimal) || type == typeof(double) || type == typeof(double) || type == typeof(float)
                || type == typeof(int) || type == typeof(Int64) || type == typeof(Int16);
        }

        public static bool IsList(this Type type, out Type itemType)
        {
            itemType = null;
            var b = type.IsList();
            if (!b)
            {
                return false;
            }

            itemType = type.GenericTypeArguments[0];
            return true;
        }

        public static Type GetNullableUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }

        public static TypeInfo GetInfo(this Type t)
        {
            return _cacheType.TypeCache[t];
        }

        public static TypeInfo GetTypeInfo(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is TypeInfo)
            {
                return obj.To<TypeInfo>();
            }

            if (obj is Type)
            {
                return obj.To<Type>().GetInfo();
            }

            return _cacheType.TypeCache[obj.GetType()];
        }

        public static TypePropertyCollection GetProperties(TypeInfo t)
        {
            return t.Properties;
        }

        public static TypePropertyCollection GetProperties(Type t)
        {
            return GetProperties(t.GetInfo());
        }

        /// <summary>
        /// Get Properties of Object, If Obj is TypeInfo Or Type then return Property Of Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TypePropertyCollection GetProperties(this object obj)
        {
            var typeInfo = obj.GetTypeInfo();
            return GetProperties(typeInfo);
        }

        /// <summary>
        /// Get Properties of Object, If Obj is TypeInfo Or Type then return Property Of Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool HasProperty(this object obj, string propName)
        {
            return obj.GetProperty(propName) != null;
        }

        /// <summary>
        /// Get Properties of Object, If Obj is TypeInfo Or Type then return Property Of Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool HasProperty(this object obj, string propName, out ITypeProperty prop)
        {
            prop = obj.GetProperty(propName);
            return prop != null;
        }


        public static ITypeProperty GetProperty(TypeInfo t, string propName)
        {
            if (!IsNestProp(propName))
            {
                return t.Properties.ContainsKey(propName) ? t.Properties[propName] : null;
            }

            var paths = propName.Split(StringConst.PROPERTY_SEPARATE_TOKEN);

            Type currentType = t.Type;
            ITypeProperty currentProp = null;
            for (int i = 0; i < paths.Length; i++)
            {
                var fieldName = paths[i];
                currentProp = GetProperty(currentType, fieldName);
                if (currentProp == null)
                {
                    return null;
                }

                currentType = currentProp.Property.PropertyType;
            }

            return currentProp;
        }

        public static ITypeProperty GetProperty(Type t, string propName)
        {
            return GetProperty(t.GetInfo(), propName);
        }

        public static ITypeProperty GetProperty<T, TProperty>(this Expression<Func<T, TProperty>> exp)
        {
            if (exp != null)
            {
                var memberExpression = (MemberExpression)exp.Body;
                var property = (PropertyInfo)memberExpression.Member;
                return new TypeProperty(property);
            }

            return null;
        }

        public static ITypeProperty GetProperty<T>(this Expression<Func<T, object>> exp)
        {
            if (exp == null)
            {
                return null;
            }

            MemberExpression member = null;
            if (exp.Body is UnaryExpression)
            {
                UnaryExpression temp = exp.Body as UnaryExpression;
                if (temp.Operand is MemberExpression)
                {
                    member = temp.Operand as MemberExpression;
                }
            }

            if (exp.Body is MemberExpression)
            {
                member = exp.Body as MemberExpression;
            }

            if (member != null)
            {
                return new TypeProperty((PropertyInfo)member.Member);
            }

            return null;
        }

        /// <summary>
        /// Get Properties of Object, If Obj is TypeInfo Or Type then return Property Of Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ITypeProperty GetProperty(this object obj, string propName)
        {
            var typeInfo = obj.GetTypeInfo();
            return GetProperty(typeInfo, propName);
        }

        /// <summary>
        /// Get Parent Object of Full PropName xxx.yyy.zzz
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fullPropName"></param>
        /// <returns></returns>
        public static object GetParent(object target, string fullPropName)
        {
            if (!fullPropName.IsNestProp())
                return target;

            var paths = fullPropName.Split(StringConst.PROPERTY_SEPARATE_TOKEN);
            fullPropName = fullPropName.Substring(0, fullPropName.Length - paths.Last().Length - 1);
            return GetPropValue(target, fullPropName);
        }

        public static object GetPropValue(this object obj, string propName)
        {
            if (!propName.IsNestProp())
            {
                var prop = obj.GetProperty(propName);
                return prop == null ? null : prop.Get(obj);
            }

            var paths = propName.Split(StringConst.PROPERTY_SEPARATE_TOKEN);

            object currentObj = obj;
            for (int i = 0; i < paths.Length; i++)
            {
                var fieldName = paths[i];
                currentObj = GetPropValue(currentObj, fieldName);
                if (currentObj == null)
                {
                    return null;
                }
            }

            return currentObj;
        }

        public static TValue GetPropValue<TValue>(this object obj, string propName)
        {
            var value = obj.GetPropValue(propName);
            return value.To<TValue>();
        }

        public static Type GetPropertyType(this ITypeProperty prop)
        {
            return prop.Property.PropertyType;
        }

        public static bool IsType<TType>(this ITypeProperty prop)
        {
            return prop.GetPropertyType() == typeof(TType);
        }

        public static Type GetDeclareType(this ITypeProperty prop)
        {
            return prop.Property.DeclaringType;
        }

        public static Type GetReflectedType(this ITypeProperty prop)
        {
            return prop.Property.ReflectedType;
        }

        public static TValue GetPropValue<T, TValue>(this T obj, Expression<Func<T, TValue>> prop)
        {
            var fullName = prop.FullName();
            return obj.GetPropValue<TValue>(fullName);
        }

        public static void SetPropValue(this object obj, string propName, object value)
        {
            if (!propName.IsNestProp())
            {
                var prop = obj.GetProperty(propName);
                if (prop != null)
                {
                    prop.Set(obj, value);
                }
                return;
            }

            var parent = GetParent(obj, propName);
            if (parent != null)
            {
                var name = propName.GetNameOfClass();
                parent.SetPropValue(name, value);
            }
        }

        public static void SetPropValue(ITypeProperty prop, object source, object value)
        {
            if (prop != null && prop.Property.CanWrite)
            {
                prop.Set(source, value);
            }
        }


        #region Attributes

        public static T GetAttribute<T>(Type t) where T : Attribute
        {
            var attr = GetAttribute(t, typeof(T)) as T;
            return attr;
        }

        public static bool HasAttribute<T>(Type t) where T : Attribute
        {
            return GetAttribute<T>(t) != null;
        }

        public static List<Attribute> GetAttributes(Type t, Type tAttribute)
        {
            if (t == null)
            {
                return new List<Attribute>();
            }

            MemberInfo info = t;
            var attributes = info.GetCustomAttributes(true);
            var attrs = attributes.Where(x => IsEqualOrDerived(tAttribute, x.GetType())).Select(o => o as Attribute).ToList();
            return attrs;
        }

        public static List<T> GetAttributes<T>(Type t) where T : Attribute
        {
            var attrs = GetAttributes(t, typeof(T)).Select(x => x as T).ToList();
            return attrs;
        }

        public static Attribute GetAttribute(Type t, Type tAttribute)
        {
            return GetAttributes(t, tAttribute).FirstOrDefault();
        }

        public static T GetAttribute<T>(PropertyInfo prop) where T : Attribute
        {
            var attr = GetAttribute(prop, typeof(T)) as T;
            return attr;
        }

        public static bool HasAttribute<T>(PropertyInfo t) where T : Attribute
        {
            return GetAttribute<T>(t) != null;
        }

        public static Attribute GetAttribute(PropertyInfo prop, Type tAttribute)
        {
            return GetAttributes(prop, tAttribute).FirstOrDefault();
        }

        public static List<T> GetAttributes<T>(PropertyInfo prop) where T : Attribute
        {
            var attr = GetAttributes(prop, typeof(T)).Select(x => x as T).ToList();
            return attr;
        }

        public static List<Attribute> GetAttributes(PropertyInfo prop, Type tAttribute)
        {
            var attrs = prop.GetCustomAttributes(true)
                            .Where(x => IsEqualOrDerived(tAttribute, x.GetType()))
                            .Select(o => o as Attribute)
                            .ToList();

            return attrs;
        }

        public static bool HasAttribute<T>(MethodInfo t) where T : Attribute
        {
            return GetAttribute<T>(t) != null;
        }

        public static T GetAttribute<T>(MethodInfo m) where T : Attribute
        {
            var attr = GetAttribute(m, typeof(T)) as T;
            return attr;
        }

        public static Attribute GetAttribute(MethodInfo m, Type tAttribute)
        {
            return GetAttributes(m, tAttribute).FirstOrDefault();
        }

        public static List<T> GetAttributes<T>(MethodInfo m) where T : Attribute
        {
            var attr = GetAttributes(m, typeof(T)).Select(x => x as T).ToList();
            return attr;
        }

        public static List<Attribute> GetAttributes(MethodInfo m, Type tAttribute)
        {
            var attrs = m.GetCustomAttributes(true)
                            .Where(x => IsEqualOrDerived(tAttribute, x.GetType()))
                            .Select(o => o as Attribute)
                            .ToList();

            return attrs;
        }

        #endregion


        #region Instance Methods

        public static TType CreateInstance<TType>(params object[] args)
        {
            return (TType)CreateInstance(typeof(TType), args);
        }

        public static object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }

        public static object CreateInstance(string type, params object[] args)
        {
            var typeInfo = GetType(type);
            if (typeInfo == null)
            {
                throw new Exception("Type {0} isn't found!".Format(type));
            }

            return CreateInstance(typeInfo.Type, args);
        }

        public static IList CreateList(string itemType, params object[] args)
        {
            var typeInfo = GetType(itemType);
            if (typeInfo == null)
            {
                throw new Exception("Item type {0} isn't found!".Format(itemType));
            }
            return CreateList(typeInfo.Type, args);
        }

        public static IList CreateList(Type itemType, params object[] args)
        {
            var listType = GetListType(itemType);
            return CreateInstance(listType, args) as IList;
        }


        public static object RunGenericStaticMethod(Type typeofClass, string methodName, Type genericType, params object[] pars)
        {
            // Grabbing the specific static method
            var methodInfo = typeofClass.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
            methodInfo = methodInfo.MakeGenericMethod(genericType);
            // Simply invoking the method and passing parameters
            // The null parameter is the object to call the method from. Since the method is static, pass null.
            object returnValue = methodInfo.Invoke(null, pars);
            return returnValue;
        }

        public static object RunStaticMethod(Type typeofClass, string methodName, params object[] pars)
        {
            // Grabbing the specific static method
            var methodInfo = typeofClass.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

            // Simply invoking the method and passing parameters
            // The null parameter is the object to call the method from. Since the method is static, pass null.
            object returnValue = methodInfo.Invoke(null, pars);
            return returnValue;
        }

        #endregion

        #region Type Methods

        public static List<TypeInfo> FindDeriveds(Type baseType, bool excludeAbstract = true)
        {
            return _cacheType.FindDeriveds(baseType, excludeAbstract);
        }

        public static List<TypeInfo> FindDeriveds(string baseType, bool excludeAbstract = true)
        {
            var type = GetType(baseType);
            if (type != null)
            {
                return FindDeriveds(type.Type, excludeAbstract);
            }

            return new List<TypeInfo>();
        }

        public static List<TypeInfo> FindDeriveds<TBaseType>(bool excludeAbstract = true)
        {
            return FindDeriveds(typeof(TBaseType), excludeAbstract);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="pTypes">Danh sách các loại tham số bình thường, ngoài kiểu generic</param>
        /// <returns></returns>
        private static bool CheckParameterTypes(MethodInfo method, Type[] pTypes)
        {
            var pCount = pTypes == null ? 0 : pTypes.Length;
            var ps = method.GetParameters();
            if (ps == null || ps.Length == 0)
            {
                return pCount == 0;
            }
            int genParamCount = 0;
            foreach (var p in ps)
            {
                if (p.ParameterType.IsGenericParameter)
                {
                    genParamCount++;
                }
            }
            if (ps.Length - genParamCount != pCount)
            {
                return false;
            }
            if (pCount == 0)
            {
                return true;
            }
            foreach (var pT in pTypes)
            {
                var f = ps.FirstOrDefault(x => x.ParameterType.Name == pT.Name);
                if (f == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static MethodInfo GetGenericMethod(Type t, string methodName, Type[] pTypes, string keyword = null)
        {
            var list = GetMethods(t, methodName).Where(m => m.IsGenericMethod);
            foreach (var m in list)
            {
                var check = CheckParameterTypes(m, pTypes);
                if (check)
                {
                    if (string.IsNullOrEmpty(keyword))
                    {
                        return m;
                    }
                    var fullText = m.ToString();
                    if (fullText.Contains(keyword))
                    {
                        return m;
                    }
                }
            }
            return null;
        }

        public static MethodInfo GetGenericMethodWithType(Type t, string methodName, Type[] paramGenericTypes, Type[] pTypes)
        {
            var list = GetMethods(t, methodName).Where(m => m.IsGenericMethod).Select(x => x.MakeGenericMethod(paramGenericTypes));
            foreach (var m in list)
            {
                var check = CheckParameterTypes(m, pTypes);
                if (check)
                {
                    return m;
                }
            }

            return null;
        }

        public static Type GetGenericType(Type type, params Type[] paramTypes)
        {
            return type.MakeGenericType(paramTypes);
        }

        public static Type GetListType(Type itemType)
        {
            return GetGenericType(typeof(List<>), itemType);
        }

        public static Type GetListType(string itemType)
        {
            var type = GetType(itemType);
            if (type == null)
            {
                throw new Exception("Type {0} isn't found!".Format(type));
            }

            return GetListType(type.Type);
        }

        public static bool HasNonParamConstructor(Type t)
        {
            var lstConstructor = t.GetConstructors();
            if (lstConstructor.IsNotEmpty() && lstConstructor.Any(x => x.GetParameters().IsEmpty()))
            {
                return true;
            }

            return false;
        }

        public static List<MethodInfo> GetMethods(Type t, string methodName)
        {
            return t.GetMember(methodName).Where(x => x is MethodInfo).Select(x => x as MethodInfo).ToList();
        }

        public static MethodInfo GetNonGenericMethod(Type t, string methodName, params Type[] paramTypes)
        {
            var lstMethod = GetMethods(t, methodName);
            if (lstMethod.IsEmpty())
            {
                return null;
            }

            return lstMethod.FirstOrDefault(x => !x.IsGenericMethod && !x.IsGenericMethodDefinition);
        }

        public static TypeInfo GetType(string name)
        {
            return _cacheType.GetType(name);
        }

        public static List<TypeInfo> GetTypes(List<string> names)
        {
            if (names.IsEmpty())
            {
                return new List<TypeInfo>();
            }

            return names.Distinct().Select(x => GetType(x)).Where(x => x != null).ToList();
        }

        public static bool IsDerived(Type baseType, Type testType)
        {
            if (testType == null)
            {
                return false;
            }

            return baseType.IsAssignableFrom(testType);
        }

        public static bool IsDerived(string baseType, string testType)
        {
            var tBase = GetType(baseType);
            var tTest = GetType(testType);
            if (tBase == null || tTest == null)
            {
                return false;
            }

            return IsDerived(tBase.Type, tTest.Type);
        }

        public static bool IsDerived<TType>(Type testType)
        {
            return IsDerived(typeof(TType), testType);
        }

        public static bool IsDerived<TType>(string testType)
        {
            var tTest = GetType(testType);
            if (tTest == null)
            {
                return false;
            }

            return IsDerived(typeof(TType), tTest.Type);
        }

        public static bool IsEqualOrDerived(Type baseType, Type testType)
        {
            if (baseType == testType)
            {
                return true;
            }

            return IsDerived(baseType, testType);
        }

        public static bool IsEqualOrDerived(string baseType, string testType)
        {
            var tBase = GetType(baseType);
            var tTest = GetType(testType);
            if (tBase == null || tTest == null)
            {
                return false;
            }

            return IsEqualOrDerived(tBase.Type, tTest.Type);
        }

        public static bool IsEqualOrDerived<TType>(Type testType)
        {
            return IsEqualOrDerived(typeof(TType), testType);
        }

        public static bool IsEqualOrDerived<TType>(string testType)
        {
            var tTest = GetType(testType);
            if (tTest == null)
            {
                return false;
            }

            return IsEqualOrDerived(typeof(TType), tTest.Type);
        }

        #endregion
    }
}
