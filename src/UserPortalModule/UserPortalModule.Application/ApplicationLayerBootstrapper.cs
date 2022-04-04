using Microsoft.Extensions.DependencyInjection;
using UserPortalModule.System.SeedSampleData;
using System.Reflection;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Auditing;
using CoreModule.Application.Validation;
using SimpleInjector;
using CoreModule.Application.Authorization;

namespace UserPortalModule
{
    public static class ApplicationLayerBootstrapper
    {
        private static readonly Assembly[] ApplicationLayerAssemblies = new[] { Assembly.GetExecutingAssembly() };

        public static Container AddApplication(this Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.RegisterInstance<IValidator>(new DataAnnotationsValidator());

            container.Register(typeof(ICommandHandler<>), ApplicationLayerAssemblies);
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(AuthorizationCommandHandlerDecorator<>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(AuditingCommandHandlerDecorator<>));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));

            container.Register(typeof(IQueryHandler<,>), ApplicationLayerAssemblies);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(AuthorizationQueryHandlerDecorator<,>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(AuditingQueryHandlerDecorator<,>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>));

            return container;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            /// application layer registrations
            /// 

            services.AddAutoMapper(typeof(UserPortalModuleApplicationService).Assembly);

            services.AddTransient<SampleDataSeeder>();

            return services;
        }

        public static IEnumerable<Type> GetCommandTypes() =>
            from assembly in ApplicationLayerAssemblies
            from type in assembly.GetExportedTypes()
            where typeof(ICommand).IsAssignableFrom(type)
            where !type.IsAbstract
            select type;

        public static IEnumerable<(Type QueryType, Type ResultType)> GetQueryTypes() =>
            from assembly in ApplicationLayerAssemblies
            from type in assembly.GetExportedTypes()
            where IsQuery(type)
            select (type, DetermineResultTypes(type).Single());

        public static Type GetQueryResultType(Type queryType) => DetermineResultTypes(queryType).Single();

        private static bool IsQuery(Type type) => DetermineResultTypes(type).Any();

        private static IEnumerable<Type> DetermineResultTypes(Type type) =>
            from interfaceType in type.GetInterfaces()
            where interfaceType.IsGenericType
            where interfaceType.GetGenericTypeDefinition() == typeof(IQuery<>)
            select interfaceType.GetGenericArguments()[0];
    }
}

