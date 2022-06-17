using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mapping
{
    public static class ObjectMapper
    {
        //Lazy class'ının avantajı aşağıda Static olarak tanımlanan Mapper property'si çağırılıncaya kadar metodun çalışmayacak olması. Ne zaman lazy property'si çağırılır o zaman lazy içerisinde tanımlanan kod icra olur.
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
       {

           var config = new MapperConfiguration(cfg =>
           {
               cfg.AddProfile<CustomMapping>();
           });

           return config.CreateMapper();
       });

        public static IMapper Mapper => lazy.Value; 
    }
}
