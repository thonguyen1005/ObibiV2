using LinqToDB.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace VSW.Core.Services
{
    public class SqlConvertHandleData : ISqlDataHandle
    {
        private AppsSetting _setting;
        public SqlConvertHandleData(IOptions<AppsSetting> options)
        {
            _setting = options.Value;
        }

        public void HandleReadValue(object obj)
        {
        }

        public void HandleUpdateValue(object obj)
        {
            var props = obj.GetProperties().Where(x => x.Value.GetPropertyType() == typeof(DateTime)).ToList();
            if (props.IsNotEmpty())
            {
                foreach (var prop in props)
                {
                    var type = prop.Value.GetPropertyType();
                    var value = obj.GetPropValue(prop.Key);
                    value = ProcesssUpdateSingleValue(value, type);
                    obj.SetPropValue(prop.Key, value);
                }
            }
        }

        private object ProcesssUpdateSingleValue(object singleValue, Type type)
        {
            if (type == typeof(DateTime))
            {
                var value = (DateTime)singleValue;
                var newValue = value.Refine();
                return newValue;
            }

            return singleValue;
        }
    }
}
