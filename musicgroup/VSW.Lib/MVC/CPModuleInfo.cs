using System;

namespace VSW.Lib.MVC
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CPModuleInfo : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public int Access { get; set; }

        public int Order { get; set; }

        public int State { get; set; }

        public bool ShowInMenu { get; set; }

        public string CssClass { get; set; }
    }
}