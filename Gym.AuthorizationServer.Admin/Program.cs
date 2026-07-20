using Gym.AuthorizationServer.Admin.Extensions;
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

builder.Services.AddDatabaseInfrastructure(builder.Configuration);
builder.Services.AddMessageBusInfstrastructure(builder.Configuration);

builder.Services.AddAuthorizationServerClient(builder.Configuration);

builder.Services.AddServices();


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

app.UseUnitOfWork();

app.MapControllers();

app.Run();
