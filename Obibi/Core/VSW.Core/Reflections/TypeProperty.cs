using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace VSW.Core
{
    public class TypeProperty : ITypeProperty
    {
        public PropertyInfo Property { get; private set; }
        public bool UseDefaultProperty { get; private set; }
        public Func<object, object> OnGet { get; private set; }
        public Action<object, object> OnSet { get; private set; }

        public string Name { get; private set; }

        public TypeProperty(PropertyInfo prop)
        {
            Property = prop;
            Name = Property.Name;
            UseDefaultProperty = prop.ReflectedType.IsGenericType;
            if (!UseDefaultProperty)
            {
                InitializeGet();
                InitializeSet();
            }
        }

        private void InitializeSet()
        {
            if (!Property.CanWrite)
                return;

            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");

            // value as T is slightly faster than (T)value, so if it's not a value type, use that
            UnaryExpression instanceCast = (!this.Property.DeclaringType.IsValueType) ? Expression.TypeAs(instance, this.Property.DeclaringType) : Expression.Convert(instance, this.Property.DeclaringType);
            UnaryExpression valueCast = (!this.Property.PropertyType.IsValueType) ? Expression.TypeAs(value, this.Property.PropertyType) : Expression.Convert(value, this.Property.PropertyType);
            OnSet = Expression.Lambda<Action<object, object>>(Expression.Call(instanceCast, Property.GetSetMethod(), valueCast), new ParameterExpression[] { instance, value }).Compile();
        }

        private void InitializeGet()
        {
            if (!Property.CanRead)
                return;

            var instance = Expression.Parameter(typeof(object), "instance");
            UnaryExpression instanceCast = null;
            if (this.Property.DeclaringType.IsValueType)
            {
                instanceCast = Expression.Convert(instance, this.Property.DeclaringType);
            }
            else
            {
                instanceCast = Expression.TypeAs(instance, this.Property.DeclaringType);
            }

            OnGet = Expression.Lambda<Func<object, object>>(Expression.TypeAs(Expression.Call(instanceCast, Property.GetGetMethod()), typeof(object)), instance).Compile();
        }

        public object Get(object instance)
        {
            if (OnGet != null)
            {
                return OnGet(instance);
            }

            if (UseDefaultProperty && Property.CanRead)
            {
                return Property.GetValue(instance);
            }

            return null;
        }

        public void Set(object instance, object value)
        {
            if (OnSet != null)
            {
                OnSet(instance, value);
            }

            if (UseDefaultProperty && Property.CanWrite)
            {
                Property.SetValue(instance, value);
            }
        }


        public void Dispose()
        {

        }
    }
}
