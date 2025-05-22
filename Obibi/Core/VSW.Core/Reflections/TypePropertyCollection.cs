using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public class TypePropertyCollection : Dictionary<string, ITypeProperty>
    {
        public TypePropertyCollection(ICollection<ITypeProperty> properties) : base()
        {
            foreach(var item in properties)
            {
                Add(item.Name, item);
            }    
        }
    }
}
