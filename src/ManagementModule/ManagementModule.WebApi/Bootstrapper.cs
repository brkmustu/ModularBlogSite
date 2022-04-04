using ManagementModule.System;
using CoreModule.Web;
using Serilog;
using SimpleInjector;

namespace ManagementModule
{
    public static class Bootstrapper
    {
        public static IEnumerable<Type> GetKnownCommandTypes() => ApplicationLayerBootstrapper.GetCommandTypes();

        public static IEnumerable<(Type QueryType, Type ResultType)> GetKnownQueryTypes() =>
            ApplicationLayerBootstrapper.GetQueryTypes();

        public static void Bootstrap(Container container)
        {
            container.AddApplication();
        }

        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            /// web api layer registrations
            /// 

            services.AddWebCore();

            services.AddHttpContextAccessor();

            services.Configure<SystemOptions>(configuration.GetSection(SystemOptions.Name));

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }
    }
}

