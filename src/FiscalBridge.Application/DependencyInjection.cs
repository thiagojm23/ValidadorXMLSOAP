using FiscalBridge.Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace FiscalBridge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IFiscalDocumentValidationStrategy, CTeValidationStrategy>();
        services.AddScoped<IFiscalDocumentValidationStrategy, NFeValidationStrategy>();
        services.AddScoped<IFiscalDocumentValidationStrategyResolver, FiscalDocumentValidationStrategyResolver>();
        services.AddScoped<ValidateFiscalDocument>();

        return services;
    }
}
