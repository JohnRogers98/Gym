using Gym.Chat.WSApi.Extensions;
using Gym.Chat.WSApi.Hubs;
using Gym.Chat.WSApi.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOptionsFromConfiguration(builder.Configuration);

builder.Services
    .AddAuthenticationSchemes(
        builder.Configuration.GetRequiredConfiguration("AccessTokenIssuer"),
        builder.Configuration.GetRequiredConfiguration("AudienceUri"))
    .AddAuthorizationPolicies()
    .AddCorsPolicy("CorsPolicy", builder.Configuration)
    .AddAuthorizationServerClient(builder.Configuration)
    .AddMemoryCache();

var authorizationCookieOptions = builder.Configuration
    .GetRequiredSection(AuthenticationCookieOptions.SectionName)
    .Get<AuthenticationCookieOptions>() ?? throw new InvalidOperationException("AuthenticationCookie section is missing.");

builder.Services
    .AddRedisClient(builder.Configuration)
    .AddRedisDataProtection(authorizationCookieOptions.DataProtectorPurpose);

builder.Services.AddServices();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<ChatHub>("/Chat", options =>
{
    options.AllowStatefulReconnects = true;
});

app.MapControllers();

app.Run();
