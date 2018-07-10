using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Utilities
{
    public static class MappingUtil
    {
        public static void Map<TSource, TDest>(TSource source, TDest destination)
            where TSource : class
            where TDest : class
        {
            AutoMapper.Mapper.Map(source, destination);
        }

        public static TDest Map<TSource, TDest>(TSource source)
            where TSource : class
            where TDest : class
        {
            return AutoMapper.Mapper.Map<TSource, TDest>(source);
        }

        public static void MapToNew<TSource, TDest>(this TSource source, out TDest dest)
            where TSource : class
            where TDest : class
        {
            dest = Activator.CreateInstance<TDest>();
            AutoMapper.Mapper.Map(source, dest);
        }
    }
}