using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VSW.Core.Modules
{
    public class ModuleCollection<TModule> : Dictionary<string, TModule>
    {
        public ModuleCollection() : base()
        {

        }

        public TModuleImplement Get<TModuleImplement>() where TModuleImplement : class, TModule
        {
            return this[typeof(TModuleImplement).FullName] as TModuleImplement;
        }
    }
}
