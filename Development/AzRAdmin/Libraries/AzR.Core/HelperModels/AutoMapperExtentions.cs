using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;

namespace AzR.Core.HelperModels
{
    public static class AutoMapperExtentions
    {

        // var result = model.Map<Category, CategoryModel>();
        public static TDest ToMap<TSource, TDest>(this TSource source)
        {
            //Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDest>());
            //var result = Mapper.Map<TDest>(source);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDest>()).CreateMapper();
            var result = mapper.Map<TDest>(source);
            return result;
        }

        //var result= listModel.Map<FCTRForeignCashInOut, FCTRForeignCashInOutViewModel>();
        public static List<TDest> ToMap<TSource, TDest>(this List<TSource> source)
        {
            //Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDest>());
            //var result = Mapper.Map<List<TDest>>(source);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDest>()).CreateMapper();
            var result = mapper.Map<List<TDest>>(source);
            return result;
        }

        public static IQueryable<TDest> ToMap<TSource, TDest>(this IQueryable<TSource> sources)
        {
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDest>()).CreateMapper();
            //Mapper.CreateMap<TSource, TDest>();
            return sources.ProjectTo<TDest>();
        }

    }

}
