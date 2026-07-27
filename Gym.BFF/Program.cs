using Gym.BFF.Extensions;
using Gym.BFF.Middlewares;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddBffAuthentication();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddServerSideSession();

builder.Services.AddCorsPolicy("BffCorsPolicy", builder.Configuration.GetRequiredConfiguration("Urls:Spa:BaseUrl"));

builder.Services.AddOptionsFromConfiguration(builder.Configuration);

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

app.UseCors("BffCorsPolicy");

app.UseSession();

app.UseAuthentication();
app.UseMiddleware<StaticHeaderCheckForCorsImposing>();
app.UseAuthorization();

app.MapControllers();

app.Run();
