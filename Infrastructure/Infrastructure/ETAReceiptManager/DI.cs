using Application.Common.Services.ETAReceiptManager;
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
        // Register configuration
        services.Configure<ETAToolkitConfiguration>(
            configuration.GetSection(ETAToolkitConfiguration.SectionName));

        // Register ETA Toolkit services as per official documentation
        services.AddSingleton<IToolkitHandler, ToolkitHandler>();
        services.AddSingleton<IJsonHelper, JsonHelper>();
        
        // Register our application service
        services.AddScoped<IETAReceiptService, ETAReceiptService>();

        return services;
    }
} 