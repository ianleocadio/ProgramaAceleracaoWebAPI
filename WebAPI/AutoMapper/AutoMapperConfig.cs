using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Configure(IServiceProvider provider, IMapperConfigurationExpression config)
        {
            config.AddProfile<MappingToDomainProfile>();
            config.AddProfile<MappingToViewModelProfile>();
            config.ConstructServicesUsing(type => ActivatorUtilities.CreateInstance(provider, type));
        }
    }
}
