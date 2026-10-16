using Gym.BFF.Extensions;
using Gym.BFF.Middlewares;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOptionsFromConfiguration(builder.Configuration);

var cookieOptions = builder.Configuration
    .GetRequiredSection(Gym.BFF.Options.CookieOptions.SectionName)
    .Get<Gym.BFF.Options.CookieOptions>() ?? throw new InvalidOperationException("Cookie section is missing.");

builder.Services.AddRedisClient(builder.Configuration)
    .AddRedisDataProtection(cookieOptions.DataProtectorPurpose);

builder.Services.AddBffAuthentication(cookieOptions.Name, cookieOptions.DataProtectorPurpose);

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddServerSideSession();

builder.Services.AddProblemDetails();

builder.Services.AddCorsPolicy("CorsPolicy", builder.Configuration);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestHeaders.Add("X-Static-Header");
    options.CombineLogs = true;
});

builder.Services.AddDelegatingHandlers();

builder.Services
    .AddAuthorizationServerClient(builder.Configuration)    
    .AddAuthorizationServerAdminApiNamedClient(builder.Configuration)
    .AddWebApiNamedClient(builder.Configuration);

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseMiddleware<StaticHeaderCheckForCorsImposingMiddleware>();
app.UseAuthorization();

app.UseMiddleware<IdempotencyMiddleware>();

app.MapControllers();

app.Run();
