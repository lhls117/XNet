using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoMapperExtension
    {
        public static void AddAutoMapper(this ServiceCollection service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(a => a.GetName().Name != nameof(AutoMapper))
                  .SelectMany(a => a.DefinedTypes)
                  .ToArray();

                var profiles = allTypes
                    .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var profile in profiles.Select(t => t.AsType()))
                {
                    cfg.AddProfile(profile);
                }
            });

            Mapper mapper = new Mapper(config);
            service.AddScoped(typeof(IMapper), x => mapper);

            // service.AddScoped<IMapper,Mapper>(x=>mapper);
            // service.AddScoped<IMapper>(x=>mapper);
        }
    }
}
