using Gym.AuthorizationServer.Admin.Extensions;
using Gym.AuthorizationServer.Client;
using Gym.AuthorizationServer.Client.Options;
using Gym.AuthorizationServer.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestHeaders.Add("X-Static-Header");
});

builder.Services.AddProblemDetails();

builder.Services.AddMemoryCache();

builder.Services.AddMediatR(cfg =>
{
    cfg.Lifetime = ServiceLifetime.Scoped;
    cfg.RegisterServicesFromAssembly(typeof(Gym.AuthorizationServer.Admin.Application.DependencyInjection).Assembly);
});

builder.Services.AddAuthenticationSchemes(
    builder.Configuration.GetRequiredConfiguration("AccessTokenIssuer"),
    builder.Configuration.GetRequiredConfiguration("AudienceUri")
    );
builder.Services.AddAuthorizationPolicies();

var mongoOptions = new MongoOptions();
builder.Configuration.GetRequiredSection("MongoDb").Bind(mongoOptions);
builder.Services.AddMongoInfrastructure(mongoOptions);

builder.Services.AddRepositories();
builder.Services.AddPasswordHashingServices();

AuthorizationServerOptions authorizationServerOptions = new();
builder.Configuration.GetRequiredSection("AuthorizationServer").Bind(authorizationServerOptions);

builder.Services.AddAuthorizationServerNamedClient(authorizationServerOptions.ClientName, authorizationServerOptions.BaseUrl);

builder.Services.SetupOAuthProtectedResourceConfiguration(authorizationServerOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
