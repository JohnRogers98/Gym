using Gym.CompositionRoot.Extensions;
using Gym.WebApi.Converters;
using Gym.WebApi.Extensions;
using Gym.Abstractions.MessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter()));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddCorsPolicies(builder.Configuration.GetRequiredConfiguration("BffUrl"));

builder.Services.AddMemoryCache();

builder.Services.AddAuthenticationSchemes(
    builder.Configuration.GetRequiredConfiguration("AccessTokenIssuer"),
    builder.Configuration.GetRequiredConfiguration("AudienceUri")
);

builder.Services.AddAuthorizationPolicies();

builder.Services.AddAuthorizationServerClient(builder.Configuration);

builder.Services.AddMessageBus(builder.Configuration);

builder.Services.AddProviders();

builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

builder.Services.AddCompositionRoot(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(nameof(CorsPolicy.AllowBff));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
