using VSW.Core.Modules;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSW.Core.Modules
{
    public static class ModuleContainer
    {
        private static List<IModule> _modules = new List<IModule>();

        private static List<INeedStart> _needStarts = new List<INeedStart>();

        private static List<INeedShutdown> _needShutdowns = new List<INeedShutdown>();

        private static void InitStartAndShutdown()
        {
            if (_needStarts.IsEmpty() && _needShutdowns.IsEmpty())
            {
                var lst = GetInstanceByType<INeedStart>(CoreService.ServiceProvider);

                if (lst.IsNotEmpty())
                    _needStarts.AddRange(lst.ToArray());

                var lstShutdown = GetInstanceByType<INeedShutdown>(CoreService.ServiceProvider);

                if (lstShutdown.IsNotEmpty())
                    _needShutdowns.AddRange(lstShutdown.ToArray());
            }
        }

        public static List<INeedStart> GetNeedStarts()
        {
            InitStartAndShutdown();
            if (_needStarts.IsNotEmpty())
            {
                var lst = _needStarts.ToList();
                lst.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                return lst;
            }

            return new List<INeedStart>();
        }

        private static void InitModules()
        {
            if (_modules.IsEmpty())
            {
                var lst = GetInstanceByType<IModule>();
                foreach (var m in lst)
                {
                    m.Configuration = CoreService.Configuration;
                }

                _modules.AddRange(lst.ToArray());
            }
        }

        public static List<IModule> GetModules()
        {
            InitModules();
            if (_modules.IsNotEmpty())
            {
                var lst = _modules.ToList();
                lst.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                return lst;
            }

            return new List<IModule>();
        }

        public static TModule GetModule<TModule>() where TModule : class, IModule
        {
            InitModules();
            var key = typeof(TModule).FullName;
            var module = _modules.FirstOrDefault(x => x.GetType().FullName == key);
            if (module != null)
            {
                return module as TModule;
            }

            var inst = TypeManager.CreateInstance(typeof(TModule)) as TModule;
            _modules.Add(inst);
            return inst;
        }

        public static List<INeedShutdown> GetNeedShutdowns()
        {
            InitStartAndShutdown();
            if (_needShutdowns.IsNotEmpty())
            {
                var lst = _needShutdowns.ToList();
                lst.Sort((x, y) => x.Priority.CompareTo(y.Priority));
                return lst;
            }

            return new List<INeedShutdown>();
        }

        private static List<T> GetInstanceByType<T>(IServiceProvider serviceProvider = null) where T : class
        {
            var rs = new List<T>();
            var ts = TypeManager.FindDeriveds<T>(excludeAbstract: true);
            foreach (var t in ts)
            {
                T inst = default;
                if (serviceProvider != null)
                {
                    inst = serviceProvider.GetService(t.Type) as T;
                    if (inst != null)
                    {
                        rs.Add(inst);
                        continue;
                    }
                }

                inst = TypeManager.CreateInstance(t.Type) as T;
                rs.Add(inst);
            }

            return rs;
        }

    }
}
