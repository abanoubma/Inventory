using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ETAReceiptManager;

public static class DI
{
    public static IServiceCollection RegisterETAReceiptManager(this IServiceCollection services, IConfiguration configuration)
    {
        // Official method - automatically registers all required services including mappers
            services.AddToolkit(configuration, ServiceLifetime.Transient);
        
        // Register your custom service wrapper
        services.AddScoped<IETAReceiptService, ETAReceiptService>();
        
        return services;
    }
} 