using Gym.AuthorizationServer.Admin.Extensions;
using Gym.AuthorizationServer.Client.Options;
using Gym.AuthorizationServer.Infrastructure;
using Gym.AuthorizationServer.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();

builder.Services.AddAuthenticationSchemes(
    builder.Configuration.GetRequiredConfiguration("AccessTokenIssuer"),
    builder.Configuration.GetRequiredConfiguration("AudienceUri")
    );

var mongoOptions = new MongoOptions();
builder.Configuration.GetRequiredSection("MongoDb").Bind(mongoOptions);
builder.Services.AddMongoInfrastructure(mongoOptions);

builder.Services.AddRepositories();

AuthorizationServerOptions authorizationServerOptions = new();
builder.Configuration.GetRequiredSection("Urls:AuthorizationServer").Bind(authorizationServerOptions);

builder.Services.AddAuthorizationServerNamedClient(authorizationServerOptions.ClientName, authorizationServerOptions.BaseUrl);

builder.Services.SetupOAuthProtectedResourceConfiguration(authorizationServerOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
