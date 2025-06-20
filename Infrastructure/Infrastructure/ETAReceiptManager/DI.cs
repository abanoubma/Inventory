using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit;
using ETA.eReceipt.IntegrationToolkit.Application.Services;
using ETA.eReceipt.IntegrationToolkit.Infrastructure.Services;
using Infrastructure.ETAReceiptManager.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ETAReceiptManager;

public static class DI
{
    public static IServiceCollection RegisterETAReceiptManager(this IServiceCollection services, IConfiguration configuration)
    {
        // Use ONLY the toolkit's registration - it handles everything
        services.AddToolkit(configuration, ServiceLifetime.Scoped);

        // Register our application service only
        services.AddScoped<IETAReceiptService, ETAReceiptService>();

        return services;
    }
} 