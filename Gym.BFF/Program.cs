using Gym.AuthorizationServer.Client;
using Gym.AuthorizationServer.Client.Options;
using Gym.BFF.Extensions;
using Gym.BFF.Middlewares;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddBffAuthentication();

builder.Services.AddServerSideSession();

builder.Services.AddCorsPolicy("BffCorsPolicy", builder.Configuration.GetRequiredConfiguration("Urls:Spa:BaseUrl"));

builder.Services.AddOptionsFromConfiguration(builder.Configuration);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestHeaders.Add("X-Static-Header");
});

builder.Services.AddDelegatingHandlers();

ClientCredentialsOptions clientCredentialsOptions = new();
builder.Configuration.GetRequiredSection("ClientCredentials").Bind(clientCredentialsOptions);

AuthorizationServerOptions authorizationServerOptions = new();
builder.Configuration.GetRequiredSection("Urls:AuthorizationServer").Bind(authorizationServerOptions);

builder.Services.AddAuthorizationServerNamedClient(authorizationServerOptions.ClientName, authorizationServerOptions.BaseUrl);

builder.Services.AddAuthorizationServerAdminApiNamedClient(
    builder.Configuration.GetRequiredConfiguration("Urls:AuthorizationServerAdminApi:ClientName"),
    builder.Configuration.GetRequiredConfiguration("Urls:AuthorizationServerAdminApi:BaseUrl")
);

builder.Services.SetupOAuthClientConfiguration(clientCredentialsOptions, authorizationServerOptions);

builder.Services.AddServices();

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseCors("BffCorsPolicy");

app.UseSession();

app.UseAuthentication();
app.UseMiddleware<StaticHeaderCheckForCorsImposing>();
app.UseAuthorization();

app.MapControllers();

app.Run();
