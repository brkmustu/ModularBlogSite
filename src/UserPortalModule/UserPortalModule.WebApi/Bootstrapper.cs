using CoreModule.Web;
using Serilog;
using SimpleInjector;
using MassTransit;
using CoreModule.Application.Common.RabbitMqExtensions;
using CoreModule.Application.Common.Contracts;

namespace UserPortalModule
{
    public static class Bootstrapper
    {
        public static IEnumerable<Type> GetKnownCommandTypes() => ApplicationLayerBootstrapper.GetCommandTypes();

        public static IEnumerable<(Type QueryType, Type ResultType)> GetKnownQueryTypes() =>
            ApplicationLayerBootstrapper.GetQueryTypes();

        public static void Bootstrap(Container container, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection(RabbitMqOptions.SectionName);
            var options = appSettingsSection.Get<RabbitMqOptions>();

            container.AddApplication();

            container.RegisterSingleton<IPublishEndpoint>(() => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(options.HostName, options.VirtualHost, hst =>
                {
                    hst.Username(options.UserName);
                    hst.Password(options.Password);
                });
            }));
        }

        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            /// web api layer registrations
            /// 

            services.AddWebCore(configuration);

            services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));

            services.Configure<TokenOptions>(configuration.GetSection(TokenOptions.SectionName));

            services.AddHttpContextAccessor();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            services.AddQueueServices(configuration);

            services.AddConsuleClient(configuration);

            return services;
        }
    }
}

