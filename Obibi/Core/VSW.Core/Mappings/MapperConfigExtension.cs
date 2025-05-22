using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace VSW.Core
{
    public static class MapperConfigExtension
    {
        internal static MapperProfile Profile { get; set; }

        public static MapperInfo Register(Type source, Type destination, MapperDirection direction = MapperDirection.Twoway)
        {
            var info = new MapperInfo(source, destination, direction);
            var innerToDest = Profile.CreateMap(info.SourceType, info.DestinationType);

            info.InnerMappingToDestination = innerToDest;

            if (info.Direction == MapperDirection.Twoway)
            {
                var innerToSource = Profile.CreateMap(info.DestinationType, info.SourceType);
                info.InnerMappingToSource = innerToSource;
            }

            return info;
        }
        public static MapperInfo Register(this IMapperConfig config, Type source, Type destination, MapperDirection direction = MapperDirection.Twoway)
        {
            return Register(source, destination, direction);
        }


        public static MapperInfo<TSource, TDestination> Register<TSource, TDestination>(this IMapperConfig config, MapperDirection direction = MapperDirection.Twoway) where TSource : class
                                                                                                                                          where TDestination : class
        {
            return Register<TSource, TDestination>(direction);
        }

        public static MapperInfo<TSource, TDestination> Register<TSource, TDestination>(MapperDirection direction = MapperDirection.Twoway) where TSource : class
                                                                                                                                            where TDestination : class
        {
            var info = new MapperInfo<TSource, TDestination>(direction);
            var innerToDest = Profile.CreateMap<TSource, TDestination>();
            info.InnerMappingToDestination = innerToDest;
            info.InnerMappingToDestination.IncludeAllDerived();

            if (info.Direction == MapperDirection.Twoway)
            {
                var innerToSource = Profile.CreateMap<TDestination, TSource>();
                info.InnerMappingToSource = innerToSource;
                info.InnerMappingToSource.IncludeAllDerived();
            }

            return info;
        }

        public static MapperInfo<TSource, TDestination> WithAfterMapToDest<TSource, TDestination>(this MapperInfo<TSource, TDestination> map, Action<TSource, TDestination> afterToDest) where TSource : class
                                                                                                                                            where TDestination : class
        {
            map.InnerMappingToDestination = map.InnerMappingToDestination.AfterMap(afterToDest);
            return map;
        }

        public static MapperInfo<TSource, TDestination> WithAfterMapToSource<TSource, TDestination>(this MapperInfo<TSource, TDestination> map, Action<TDestination, TSource> afterToSource) where TSource : class
                                                                                                                                          where TDestination : class
        {
            map.InnerMappingToSource = map.InnerMappingToSource.AfterMap(afterToSource);
            return map;
        }

        public static MapperInfo<TSource, TDestination> ForMemberToDest<TSource, TDestination, TDestMember>(this MapperInfo<TSource, TDestination> map, Expression<Func<TDestination, TDestMember>> exp
                                                                                            , Func<TSource, TDestination, TDestMember> func) where TSource : class
                                                                                                                                           where TDestination : class
        {
            map.InnerMappingToDestination = map.InnerMappingToDestination.ForMember(exp, options =>
            {
                options.MapFrom(func);
            });

            return map;
        }

        public static MapperInfo<TSource, TDestination> ForMemberToSource<TSource, TDestination, TSourceMember>(this MapperInfo<TSource, TDestination> map, Expression<Func<TSource, TSourceMember>> exp
                                                                                            , Func<TDestination, TSource, TSourceMember> func) where TSource : class
                                                                                                                                           where TDestination : class
        {
            map.InnerMappingToSource = map.InnerMappingToSource.ForMember(exp, options =>
            {
                options.MapFrom(func);
            });

            return map;
        }

        public static MapperInfo<TSource, TDestination> ForMemberTwoway<TSource, TDestination, TMember>(this MapperInfo<TSource, TDestination> map, Expression<Func<TDestination, TMember>> expDest
                                                                                         , Expression<Func<TSource, TMember>> expSource
                                                                                        , Func<TSource, TDestination, TMember> funcToDest = null
                                                                                        , Func<TDestination, TSource, TMember> funcToSource = null
                                                                                        ) where TSource : class
                                                                                                                                        where TDestination : class
        {
            var funcToDestByField = expSource.Compile();
            var funcToSourceByField = expDest.Compile();

            map.InnerMappingToDestination = map.InnerMappingToDestination.ForMember(expDest, options =>
            {
                options.MapFrom((s, d) => funcToDest != null ? funcToDest(s, d) : funcToDestByField(s));
            });

            map.InnerMappingToSource = map.InnerMappingToSource.ForMember(expSource, options =>
            {
                options.MapFrom((d, s) => funcToSource != null ? funcToSource(d, s) : funcToSourceByField(d));
            });

            return map;
        }

        public static MapperInfo<TSource, TDestination> ForMemberTwoway<TSource, TDestination, TSourceMember, TDestMember>(this MapperInfo<TSource, TDestination> map, Expression<Func<TDestination, TDestMember>> expDest
                                                                                      , Expression<Func<TSource, TSourceMember>> expSource
                                                                                    , Func<TSource, TDestination, TDestMember> funcToDest
                                                                                     , Func<TDestination, TSource, TSourceMember> funcToSource
                                                                                     ) where TSource : class
                                                                                                                                     where TDestination : class
        {
            map.InnerMappingToDestination = map.InnerMappingToDestination.ForMember(expDest, options =>
            {
                options.MapFrom((s, d) => funcToDest(s, d));
            });

            map.InnerMappingToSource = map.InnerMappingToSource.ForMember(expSource, options =>
            {
                options.MapFrom((d, s) => funcToSource(d, s));
            });

            return map;
        }
    }
}
