using VSW.Core.Caching;
using VSW.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VSW.Core
{
    public class CachedTypes
    {
        private readonly object _lock = new object();

        private readonly GenericCache<string, TypeInfo> _mapType = new GenericCache<string, TypeInfo>();
        private readonly List<TypeInfo> _cachedType = new List<TypeInfo>();
        private readonly Dictionary<string, List<TypeInfo>> _cachedDirectDerivedTypes = new Dictionary<string, List<TypeInfo>>();

        private Lazy<AssemblySetting> setting;

        public CachedTypes(Func<AssemblySetting> _setting)
        {
            setting = new Lazy<AssemblySetting>(_setting, true);
        }

        #region Loader

        public void Load()
        {
            ensureAllTypesLoaded();
        }


        private bool bAlreadyLoadType = false;
        private void ensureAllTypesLoaded()
        {
            lock (_lock)
            {
                if (!bAlreadyLoadType)
                {
                    var settingValue = setting.Value;

                    if(settingValue == null)
                    {
                        return;
                    }

                    AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

                    var assemblyPaths = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => !x.IsDynamic)
                        .Select(x => x.Location);

                    List<string> lstAllDllPath = new List<string>();

                    if (setting.Value.Patterns.IsNotEmpty())
                    {
                        var pattern = settingValue.Patterns.Join("|");

                        var exclude = "";
                        if (settingValue.ExcludePatterns.IsNotEmpty())
                        {
                            exclude = settingValue.ExcludePatterns.Join("|");
                        }

                        lstAllDllPath = FileHelper.GetFiles(AppDomain.CurrentDomain.BaseDirectory, pattern: pattern, excludePattern: exclude);
                    }


                    var unloadedAssemblies = lstAllDllPath.Except(assemblyPaths).ToList();

                    foreach (var idx in unloadedAssemblies)
                    {
                        AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(idx));
                    }
                    var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var types = new List<Type>();
                    foreach (var assembly in loadedAssemblies)
                    {
                        if (assembly.IsDynamic || !lstAllDllPath.Contains(assembly.Location))
                        {
                            continue;
                        }

                        var lstTemp = assembly.GetTypes();
                        if (lstTemp.IsNotEmpty())
                        {
                            types.AddRange(lstTemp);
                        }
                    }

                    AddTypesToCache(types);

                    bAlreadyLoadType = true;
                }
            }
        }

        private void AddTypesToCache(IEnumerable<Type> ts)
        {
            foreach (var t in ts)
            {
                var key = t.FullName;
                if (_mapType.HasKey(key))
                {
                    continue;
                }

                var type = new TypeInfo(t);

                _mapType.Add(key, type);
                _cachedType.Add(type);

                AddToCacheDirectDerivedType(type);

                //Đối với name thì add Type đầu tiên
                key = t.Name;
                if (_mapType.HasKey(key))
                {
                    var existType = _mapType[key];
                    continue;
                }
                _mapType.Add(key, type);
            }
        }

        private void AddToCacheDirectDerivedType(TypeInfo t)
        {
            if (t == null)
            {
                return;
            }

            var lstInterface = t.Type.GetInterfaces();
            foreach (var i in lstInterface)
            {
                var ifName = i.FullName;
                if (string.IsNullOrEmpty(ifName))
                {
                    continue;
                }
                List<TypeInfo> list;
                if (!_cachedDirectDerivedTypes.ContainsKey(ifName))
                {
                    list = new List<TypeInfo>();
                    _cachedDirectDerivedTypes.Add(ifName, list);
                }
                else
                {
                    list = _cachedDirectDerivedTypes[ifName];
                }
                if (!list.Any(x => x.FullName == t.FullName))
                {
                    list.Add(t);
                }
            }
            Type baseType = t.Type.BaseType;
            if (baseType != null && !string.IsNullOrEmpty(baseType.FullName))
            {
                List<TypeInfo> list;
                if (!_cachedDirectDerivedTypes.ContainsKey(baseType.FullName))
                {
                    list = new List<TypeInfo>();
                    _cachedDirectDerivedTypes.Add(baseType.FullName, list);
                }
                else
                {
                    list = _cachedDirectDerivedTypes[baseType.FullName];
                }

                if (!list.Any(x => x.FullName == t.FullName))
                {
                    list.Add(t);
                }

                if (baseType.IsGenericType && !baseType.IsGenericTypeDefinition)
                {
                    AddToCacheDirectDerivedType(new TypeInfo(baseType));
                }
            }
        }

        /// <summary>
        /// Resolve dynamically loaded assemblies.
        /// </summary>
        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assyName = args.Name;
            if (assyName.EndsWith(".XmlSerializers") || assyName.Contains(".XmlSerializers,") ||
                assyName.Contains(".resources,") || assyName.Equals("NLog") || assyName.Equals("Serilog"))
            {
                return null;
            }

            var settingValue = setting.Value;
            TypeInfo t = GetType(assyName);
            if (t == null)
            {
                if (args.RequestingAssembly != null)
                {
                    return OnAssemplyDependentResolve(args.Name, args.RequestingAssembly);
                }
                else
                {
                    if (settingValue.ResolvePaths.IsEmpty())
                    {
                        return null;
                    }
                    // Logger.Debug(string.Format("Trying to resolver {0} on additional paths", assyName));
                    string[] names = assyName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var path in settingValue.ResolvePaths)
                    {
                        string fileName = Path.Combine(path, names[0] + ".dll");
                        if (File.Exists(fileName))
                        {
                            // Logger.Debug("Found " + fileName);
                            return Assembly.LoadFrom(fileName);
                        }
                        // thử tiếp với file exe
                        fileName = Path.Combine(path, names[0] + ".exe");
                        if (File.Exists(fileName))
                        {
                            // Logger.Debug("Found " + fileName);
                            return Assembly.LoadFrom(fileName);
                        }
                    }
                }
                return null;
            }
            return t.Type.Assembly;
        }

        private Assembly OnAssemplyDependentResolve(string name, Assembly parent)
        {
            var file = new FileInfo(parent.Location);
            if (file.Exists)
            {
                string[] names = name.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (names.Length > 0)
                {
                    if (file.DirectoryName == null)
                    {
                        return null;
                    }
                    string childPath = Path.Combine(file.DirectoryName, names[0] + ".dll");
                    if (FileHelper.FileExists(childPath))
                    {
                        return Assembly.LoadFrom(childPath);
                    }
                }
            }

            return null;
        }

        public TypeInfo GetType(string typeName)
        {
            return _mapType.HasKey(typeName) ? _mapType[typeName] : null;
        }


        public List<TypeInfo> FindDeriveds(Type baseType, bool excludeAbstract = true)
        {
            var list = new List<TypeInfo>();
            if (baseType.FullName.IsNotEmpty() && _cachedDirectDerivedTypes.ContainsKey(baseType.FullName))
            {
                var lst = _cachedDirectDerivedTypes[baseType.FullName];
                foreach (var t in lst)
                {
                    var lstChild = FindDeriveds(t.Type, excludeAbstract);
                    if (lstChild.Count > 0)
                    {
                        lstChild.ForEach(x =>
                        {
                            if (x.FullName.IsNotEmpty() && _mapType.HasKey(x.FullName) && !list.Contains(x))
                            {
                                list.Add(x);
                            }
                        });
                    }

                    if (excludeAbstract && (t.Type.IsAbstract || t.Type.IsInterface))
                    {
                        continue;
                    }

                    if (!t.FullName.IsEmpty() && _mapType.HasKey(t.FullName) && !list.Contains(t))
                    {
                        list.Add(t);
                    }
                }
            }


            return list;
        }

        #endregion

        #region Cached Types

        private readonly GenericCache<Type, TypePropertyCollection> _cacheProperty = new GenericCache<Type, TypePropertyCollection>(InitPropertyOfType);

        private readonly GenericCache<Type, TypeInfo> _cacheType = new GenericCache<Type, TypeInfo>(InitTypeInfo);

        private static TypeInfo InitTypeInfo(Type t)
        {
            return new TypeInfo(t);
        }

        private static TypePropertyCollection InitPropertyOfType(Type t)
        {
            var lst = new List<ITypeProperty>();
            foreach (var prop in t.GetProperties())
            {
                if (!prop.CanRead && prop.CanWrite)
                {
                    continue;
                }

                //Bỏ qua thuộc tính static
                if (prop.CanRead && (prop.GetMethod.IsStatic || !prop.GetMethod.IsPublic))
                {
                    continue;
                }

                if (prop.CanWrite && (prop.SetMethod.IsStatic || !prop.SetMethod.IsPublic))
                {
                    continue;
                }

                //Bỏ qua Thuộc tính Index
                var indexParam = prop.GetIndexParameters();
                if (indexParam != null && indexParam.Length > 0)
                {
                    continue;
                }

                var objProp = new TypeProperty(prop);

                lst.Add(objProp);
            }

            var collection = new TypePropertyCollection(lst);
            return collection;
        }

        internal GenericCache<Type, TypePropertyCollection> PropertyCache { get { return _cacheProperty; } }

        internal GenericCache<Type, TypeInfo> TypeCache { get { return _cacheType; } }

        #endregion

    }
}
