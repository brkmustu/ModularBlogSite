using Microsoft.Extensions.DependencyInjection;
using ManagementModule.System.SeedSampleData;
using System.Reflection;
using CoreModule.Application.Common.Contracts;
using CoreModule.Application.Common.Interfaces;
using CoreModule.Application.Auditing;
using CoreModule.Application.Validation;
using SimpleInjector;
using CoreModule.Application.Authorization;
using CoreModule.Application;
using Microsoft.Extensions.Configuration;
using CoreModule.Application.CrossCuttingConcerns;
using ManagementModule.Common.Contracts;

namespace ManagementModule;

public static class ApplicationLayerBootstrapper
{
    private static readonly Assembly[] ApplicationLayerAssemblies = new[] { Assembly.GetExecutingAssembly() };

    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        /// application layer registrations
        /// 

        services.AddCoreApp(configuration);

        services.AddAutoMapper(typeof(ManagementModuleApplicationService).Assembly);

        services.AddTransient<SampleDataSeeder>();

        return services;
    }

    public static Container AddApplication(this Container container)
    {
        if (container == null)
        {
            throw new ArgumentNullException(nameof(container));
        }

        container.RegisterInstance<IValidator>(new DataAnnotationsValidator());

        container.Register(typeof(ICommandHandler<>), ApplicationLayerAssemblies);
        container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(AuditingCommandHandlerDecorator<>),
                x => x.ImplementationType.GetCustomAttributes(typeof(AuditingDecoratorAttribute)).Any()
            );
        container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(AuthorizationCommandHandlerDecorator<>),
                x => x.ImplementationType.GetCustomAttributes(typeof(AuthorizationDecoratorAttribute)).Any()
            );
        container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(ValidationCommandHandlerDecorator<>),
                x => x.ImplementationType.GetCustomAttributes(typeof(ValidationDecoratorAttribute)).Any()
            );

        container.Register(typeof(IQueryHandler<,>), ApplicationLayerAssemblies);
        container.RegisterDecorator(
                typeof(IQueryHandler<,>),
                typeof(AuditingQueryHandlerDecorator<,>),
                x => x.ImplementationType.GetCustomAttributes(typeof(AuditingDecoratorAttribute)).Any()
            );
        container.RegisterDecorator(
                typeof(IQueryHandler<,>),
                typeof(AuthorizationQueryHandlerDecorator<,>),
                x => x.ImplementationType.GetCustomAttributes(typeof(AuthorizationDecoratorAttribute)).Any()
            );
        container.RegisterDecorator(
                typeof(IQueryHandler<,>),
                typeof(ValidationQueryHandlerDecorator<,>),
                x => x.ImplementationType.GetCustomAttributes(typeof(ValidationDecoratorAttribute)).Any()
            );

        return container;
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

