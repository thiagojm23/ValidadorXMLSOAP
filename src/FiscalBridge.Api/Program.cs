using FiscalBridge.Api.Endpoints;
using FiscalBridge.Api.Security;
using FiscalBridge.Application;
using FiscalBridge.Infrastructure;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddAuthentication(AccessTokenAuthenticationDefaults.Scheme)
    .AddScheme<AccessTokenAuthenticationOptions, AccessTokenAuthenticationHandler>(
        AccessTokenAuthenticationDefaults.Scheme,
        _ => { });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapTokenEndpoints();
app.MapFiscalDocumentEndpoints();

app.Run();