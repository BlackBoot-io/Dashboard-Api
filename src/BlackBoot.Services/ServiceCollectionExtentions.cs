﻿using BlackBoot.Services.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BlackBoot.Services;

public static class ServiceCollectionExtentions
{
    public static void RegisterApplicatioinServices(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtentions).Assembly;
        services.RegisterScopedServices(assembly);
        services.RegisterSingletonServices(assembly);
        services.RegisterTransientServices(assembly);
    }
}

