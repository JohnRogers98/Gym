using Gym.CompositionRoot.Extensions;
using Gym.WebApi.Controllers.Api.Users.Jwt;
using Gym.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

builder.Services.AddCompositionRoot(builder.Configuration);
builder.Services.AddSingleton<IAccessTokenGenerator, AccessTokenGenerator>();

builder.Services.AddCorsPolicies(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorizationPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(nameof(CorsPolicy.AllowWebApplication));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
