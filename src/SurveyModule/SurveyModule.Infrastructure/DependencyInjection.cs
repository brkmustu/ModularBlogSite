﻿using CoreModule.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace SurveyModule.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, MachineDateTime>();
        return services;
    }
}

