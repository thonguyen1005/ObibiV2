using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class XmlMemberAttribute : Attribute
    {
        public Type Type { get; set; }

        public string TagName { get; set; }

        public string Format { get; set; }

        public string TagParent { get; set; }

        public XmlMemberAttribute()
        {
        }
    }
}
