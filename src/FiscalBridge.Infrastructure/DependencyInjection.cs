using FiscalBridge.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiscalBridge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<TokenOptions>()
            .Bind(configuration.GetSection(TokenOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.GenerationToken),
                $"{TokenOptions.SectionName}:GenerationToken é obrigatório.")
            .Validate(options => options.AccessTokenSigningKey?.Length >= 32,
                $"{TokenOptions.SectionName}:AccessTokenSigningKey deve ter pelo menos 32 caracteres.")
            .Validate(options => options.AccessTokenLifetimeMinutes is > 0 and <= 43_200,
                $"{TokenOptions.SectionName}:AccessTokenLifetimeMinutes deve estar entre 1 e 43200.")
            .ValidateOnStart();

        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<AccessTokenService>();

        return services;
    }
}
