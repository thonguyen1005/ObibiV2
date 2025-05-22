using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace VSW.Core
{
    public class Parameter<TValue>
    {
        public string Name { get; set; }
        public TValue Value { get; set; }

        public string Description { get; set; }

        public Parameter()
        {

        }

        public Parameter(string name, TValue value, string desc = null)
        {
            Name = name;
            Value = value;
            Description = desc;
        }
    }

    public class Parameter : Parameter<string>
    {
        public Parameter(string key, string value, string desc = null) : base(key, value, desc)
        {
        }

        public Parameter() : base()
        {
        }

        /// <summary>
        /// Conversion from string to Parameter.
        /// </summary>
        /// <param name="value">name:value</param>
        public static implicit operator Parameter(string value)
        {
            string[] arr = value.SplitWithTrim(":");
            var r = new Parameter { Name = arr[0].Trim() };
            if (arr.Length == 1)
            {
                r.Value = null;
            }
            else if (arr.Length == 2)
            {
                r.Value = arr[1].Trim();
            }
            else
            {
                r.Value = arr.ToList().GetRange(1, arr.Length - 1).Join(":");
            }

            return r;
        }
    }

    public class Parameters<TValue> : KeyValueList<string, Parameter<TValue>>
    {
        public Parameters() : base(x => x.Name)
        {

        }
    }

    public class Parameters : KeyValueList<string, Parameter>
    {
        public Parameters() : base(x => x.Name)
        {

        }

        /// <summary>
        /// Conversion from string to List of Parameters.
        /// </summary>
        /// <param name="value">name1:value1;name2:value2</param>
        public static implicit operator Parameters(string value)
        {
            var list = new Parameters();
            string[] arr = value.SplitWithTrim(";");
            foreach (var a in arr)
            {
                Parameter p = a;
                list.Add(p);
            }
            return list;
        }
    }
}
