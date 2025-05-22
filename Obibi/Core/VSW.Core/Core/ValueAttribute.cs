using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    public class ValueAttribute : Attribute
    {
        public string Value { get; set; }

        public ValueAttribute(string value)
        {
            Value = value;
        }
    }
}