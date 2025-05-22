using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace VSW.Core
{
    public interface ITypeProperty : IDisposable
    {
        PropertyInfo Property { get; }

        string Name { get; }

        bool UseDefaultProperty { get; }

        Func<object, object> OnGet { get; }

        Action<object, object> OnSet { get; }

        object Get(object instance);

        void Set(object instance, object value);
    }
}
