using Application.DI;
using Application.Options;
using Azure.Identity;
using ChatApi.Extentions;
using ChatApi.Hubs;
using ChatApi.Hubs.Interfaces;
using Infrastructure.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Azure.SignalR;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(
    Environment.GetEnvironmentVariable("SchoolChatSecretsUri")
    ?? builder.Configuration["SchoolChatSecretsUri"]
    ?? throw new InvalidConfigurationException("Environment\\configuration variable is missing: SchoolChatSecretsUri"));

DotNetEnv.Env.Load(
    Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));
builder.Configuration
    .AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential())
    .AddEnvironmentVariables();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        var jwtOptions = builder.Services.BuildServiceProvider().GetRequiredService<JwtOptions>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };
    });

builder.WebHost.ConfigureKestrel(options =>
{
    int httpPort = 0;

    if (int.TryParse(builder.Configuration["ASPNETCORE_HTTP_PORT"], out int port1))
    {
        httpPort = port1;
    }
    else if (int.TryParse(Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORT"), out int port2))
    {
        httpPort = port2;
    }
    else
    {
        throw new InvalidConfigurationException("Http ports isn't configured");
    }

    options.ListenAnyIP(httpPort);
}); // http only


builder.Services.AddControllers(conf =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    conf.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddPagination();

var azureSignalRConnectionString = builder.Configuration["SignalR-SchoolChat-PrimaryConnectionString"]
    ?? Environment.GetEnvironmentVariable("SignalR-SchoolChat-PrimaryConnectionString")
    ?? throw new InvalidConfigurationException("Environment\\configuration variable is missing: SignalR-SchoolChat-PrimaryConnectionString");
//azure key vault

builder.Services.AddSignalR().AddAzureSignalR(azureSignalRConnectionString);
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

//Presentation Layer Dependencies
builder.Services.AddSingleton<IChatsHub, ChatsHub>();

//Application Layer Dependencies
builder.Services.AddApplicationServices();

//Infrastructure Layer Dependencies
builder.Services.AddDBDependencies();

var app = builder.Build();

app.ExecuteMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatsHub>("/api/chat-hub");


app.MapControllers().RequireAuthorization();

app.Run();
