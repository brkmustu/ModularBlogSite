using AutoMapper;
using CoreModule.Application.Common.Contracts;
using Microsoft.Extensions.Options;
using SimpleInjector;
using System;
using System.Linq;

namespace UserPortalModule;

public class TestBase
{
    protected Container Container = new Container();
    protected IMapper Mapper;

    public TestBase()
    {
        var profiles = typeof(ApplicationLayerBootstrapper).Assembly
            .GetTypes()
            .Where(x => typeof(Profile).IsAssignableFrom(x));

        var config = new MapperConfiguration(cfg =>
        {
            foreach (var profile in profiles)
            {
                cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        });

        Container.RegisterInstance(config);
        Container.Register(() => config.CreateMapper(Container.GetInstance));

        Mapper = Container.GetInstance<IMapper>();
    }
}

