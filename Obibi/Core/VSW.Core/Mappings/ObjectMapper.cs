using AutoMapper;
using VSW.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace VSW.Core
{
    public class InjectOptions
    {
        public int Level { get; set; }

        public List<string> Fields { get; }

        private List<string> _inculdeFields;
        private List<string> _excludeFields;

        public InjectOptions()
        {

        }

        public static InjectOptions WithLevel(int level = -1)
        {
            return new InjectOptions { Level = level };
        }

        public static InjectOptions WithMaxLevel()
        {
            return new InjectOptions { Level = int.MaxValue };
        }

        public static InjectOptions WithLevel(InjectOptions option, int level = -1)
        {
            option.Level = level;
            return option;
        }

        public static InjectOptions WithMaxLevel(InjectOptions option)
        {
            option.Level = int.MaxValue;
            return option;
        }

        public static InjectOptions WithFields(params string[] fields)
        {
            return new InjectOptions { _inculdeFields = fields.ToList() };
        }

        public static InjectOptions WithFields(InjectOptions option, params string[] fields)
        {
            option._inculdeFields = fields.ToList();
            return option;
        }

        public static InjectOptions ExcludeFields(params string[] fields)
        {
            return new InjectOptions { _excludeFields = fields.ToList() };
        }

        public static InjectOptions ExcludeFields(InjectOptions option, params string[] fields)
        {
            option._excludeFields = fields.ToList();
            return option;
        }


        public static InjectOptions<TObject> WithLevel<TObject>(int level = -1) where TObject : class
        {
            return new InjectOptions<TObject> { Level = level };
        }

        public static InjectOptions<TObject> WithLevel<TObject>(InjectOptions<TObject> option, int level = -1) where TObject : class
        {
            option.Level = level;
            return option;
        }

        public static InjectOptions<TObject> WithFields<TObject>(params Expression<Func<TObject, object>>[] fields) where TObject : class
        {
            return new InjectOptions<TObject> { _inculdeFields = fields.Select(x => x.Name()).ToList() };
        }

        public static InjectOptions<TObject> WithFields<TObject>(InjectOptions<TObject> option, params Expression<Func<TObject, object>>[] fields) where TObject : class
        {
            option._inculdeFields = fields.Select(x => x.Name()).ToList();
            return option;
        }

        public static InjectOptions<TObject> ExcludeFields<TObject>(params Expression<Func<TObject, object>>[] fields) where TObject : class
        {
            return new InjectOptions<TObject> { _excludeFields = fields.Select(x => x.Name()).ToList() };
        }

        public static InjectOptions<TObject> ExcludeFields<TObject>(InjectOptions<TObject> option, params Expression<Func<TObject, object>>[] fields) where TObject : class
        {
            option._excludeFields = fields.Select(x => x.Name()).ToList();
            return option;
        }
    }


    public class InjectOptions<TObject> : InjectOptions where TObject : class
    {
        public InjectOptions()
        {

        }
    }

    public static class ObjectMapper
    {
        private static IMapper GetMapper()
        {
            var mapper = CoreService.Mapper;
            if (mapper == null)
            {
                throw new Exception("Auto Mapper isn't init config");
            }

            return mapper;
        }

        public static object MapTo(this object source, Type destType)
        {
            return GetMapper().Map(source, source.GetType(), destType);
        }

        public static TDestination MapTo<TDestination>(this object source)
        {
            return GetMapper().Map<TDestination>(source);
        }

        public static TDestination MapTo<TDestination>(this object source, TDestination destination, Type destType = null) where TDestination:class
        {
            if (destType == null)
            {
                destType = destination.GetType();
            }

            return GetMapper().Map(source, destination, source.GetType(), destType) as TDestination;
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return GetMapper().Map<TSource, TDestination>(source);
        }

        public static List<TDestination> MapToList<TDestination>(this IEnumerable sources)
        {
            var rs = new List<TDestination>();

            var mapper = GetMapper();

            foreach (var obj in sources)
            {
                var objDest = mapper.Map<TDestination>(obj);
                rs.Add(objDest);
            }

            return rs;
        }

        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> sources)
        {
            var rs = new List<TDestination>();

            var mapper = GetMapper();

            foreach (var obj in sources)
            {
                var objDest = mapper.Map<TSource, TDestination>(obj);
                rs.Add(objDest);
            }

            return rs;
        }

        public static IList MapToList<TSource>(this IEnumerable<TSource> sources, Type destType)
        {
            var rs = TypeManager.CreateList(destType);

            var mapper = GetMapper();

            var srcType = typeof(TSource);

            foreach (var obj in sources)
            {
                var objDest = mapper.Map(obj, srcType, destType);
                rs.Add(objDest);
            }

            return rs;
        }

        public static void InjectTo(this object source, object destination, InjectOptions options = null)
        {
            if (source == null || destination == null)
            {
                return;
            }

            var level = options == null ? 1 : options.Level;
            if (level <= 0)
            {
                return;
            }

            var sourceType = source.GetType();
            var targetType = destination.GetType();
            var isSameType = sourceType == targetType;
            var propsSource = TypeManager.GetProperties(sourceType);
            var specField = options != null ? options.Fields : null;
            var hasSpecField = specField.IsNotEmpty();

            foreach (var prop in propsSource.Values)
            {
                if (isSameType && !prop.Property.CanWrite)
                {
                    continue;
                }

                string name = prop.Property.Name;
                if (hasSpecField && !specField.Contains(name))
                {
                    continue;
                }

                var val = prop.Get(source);
                bool isAssign = true;
                if (level > 1 && val != null && !TypeManager.IsSystemType(prop.Property.PropertyType))
                {
                    var valTarget = destination.GetPropValue(name);
                    isAssign = false;//Mac dinh la false
                    if (valTarget == null)
                    {
                        val = Clone(val, level - 1);
                        isAssign = true;
                    }
                    else if (prop.Property.PropertyType.IsList())
                    {
                        var lstTemp = val as IList;
                        var lstTarget = valTarget as IList;
                        lstTarget.Clear();
                        foreach (var objTemp in lstTemp)
                        {
                            var newObject = level > 2 ? Clone(objTemp, level - 1) : objTemp;
                            lstTarget.Add(newObject);
                        }
                    }
                    else
                    {
                        InjectTo(val, valTarget, InjectOptions.WithLevel(level - 1));
                    }
                }

                if (!isAssign)
                {
                    continue;
                }

                if (isSameType)
                {
                    TypeManager.SetPropValue(prop, destination, val);
                }
                else
                {
                    var propTarget = destination.GetProperty(name);
                    TypeManager.SetPropValue(propTarget, destination, val);
                }
            }
        }

        public static T CloneObject<T>(this T source, int level = int.MaxValue) where T : class
        {
            return (T)Clone(source, level);
        }

        public static object Clone(object source, int level = int.MaxValue)
        {
            if (source == null || level <= 0)
            {
                return null;
            }

            var type = source.GetType();
            var result = TypeManager.CreateInstance(type);
            if (type.IsList())
            {
                var sourceList = source as IList;
                if (sourceList.Count == 0)
                {
                    return result;
                }

                var newList = result as IList;

                foreach (var obj in sourceList)
                {
                    if (obj == null || TypeManager.IsSystemType(obj.GetType()))
                    {
                        newList.Add(obj);
                        continue;
                    }

                    var newObj = Clone(obj, level == int.MaxValue ? level : (level - 1));
                    newList.Add(newObj);
                }

                return newList;
            }

            var propsSource = TypeManager.GetProperties(type);
            foreach (var prop in propsSource.Values)
            {
                if (prop.Property.CanWrite)
                {
                    string name = prop.Property.Name;
                    var val = prop.Get(source);

                    if (val != null && !TypeManager.IsSystemType(prop.Property.PropertyType))
                    {
                        val = Clone(val, level == int.MaxValue ? level : (level - 1));
                    }

                    TypeManager.SetPropValue(prop, result, val);
                }
            }

            return result;
        }

    }
}
