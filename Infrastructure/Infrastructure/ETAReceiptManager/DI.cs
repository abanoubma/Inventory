using Application.Common.Services.ETAReceiptManager;
using ETA.eReceipt.IntegrationToolkit.Application.Services;
using ETA.eReceipt.IntegrationToolkit.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ETAReceiptManager;

public static class DI
{
    public static IServiceCollection RegisterETAReceiptManager(this IServiceCollection services, IConfiguration configuration)
    {
        // Register ETA Toolkit services
        services.AddSingleton<IToolkitHandler, ToolkitHandler>();
        services.AddSingleton<IJsonHelper, JsonHelper>();
        
        // Register our application service
        services.AddScoped<IETAReceiptService, ETAReceiptService>();

        return services;
    }
} 