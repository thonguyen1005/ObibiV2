using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace VSW.Core
{
    public enum MapperDirection
    {
        Oneway,
        Twoway
    }

    public class MapperInfo
    {
        public Type SourceType { get; set; }

        public Type DestinationType { get; set; }

        internal IMappingExpression InnerMappingToDestination { get; set; }

        internal IMappingExpression InnerMappingToSource { get; set; }

        public MapperDirection Direction { get; set; }


        internal MapperInfo(Type source, Type destination, MapperDirection direction = MapperDirection.Twoway)
        {
            SourceType = source;
            DestinationType = destination;
            Direction = direction;
        }
    }

    public class MapperInfo<TSource, TDestination> : MapperInfo where TSource : class
                                                                where TDestination : class
    {
        internal new IMappingExpression<TSource, TDestination> InnerMappingToDestination { get; set; }

        internal new IMappingExpression<TDestination, TSource> InnerMappingToSource { get; set; }


        internal MapperInfo(MapperDirection direction = MapperDirection.Twoway) : base(typeof(TSource), typeof(TDestination), direction)
        {
        }

    }

    public interface IMapperConfig
    {
        void Register();
    }

    public abstract class MapperConfig : IMapperConfig
    {
        public MapperConfig()
        {
        }

        public abstract void Register();
    }

    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            AutoRegister();
        }

        private void AutoRegister()
        {
            MapperConfigExtension.Profile = this;

            var lstConfig = TypeManager.FindDeriveds<IMapperConfig>();
            if (lstConfig.IsNotEmpty())
            {
                foreach (var config in lstConfig)
                {
                    var instance = TypeManager.CreateInstance(config.Type).To<IMapperConfig>();
                    instance.Register();
                }
            }
        }
    }
}
