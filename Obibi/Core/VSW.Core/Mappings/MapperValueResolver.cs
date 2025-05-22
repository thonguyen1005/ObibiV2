using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public class MapperValueResolver<TSource, TDest, TResult> : IValueResolver<TSource, TDest, TResult>
    {
        protected Func<object, object, object> Func { get; set; }

        public MapperValueResolver(Func<object, object, object> func)
        {
            Func = func;
        }

        public TResult Resolve(TSource source, TDest destination, TResult destMember, ResolutionContext context)
        {
            var value = Func(source, destination);
            return value.To<TResult>();
        }
    }
}
