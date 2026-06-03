using Gym.BFF.Extensions;
using Gym.BFF.Middlewares;
using Gym.BFF.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddBffAuthentication();

builder.Services.AddServerSideSession();

builder.Services.AddBffCors();

builder.Services.AddAuthorizationServerNamedClient(HttpClientNames.AuthorizationServer, builder.Configuration);

builder.Services.AddServices();
builder.Services.AddOptionsFromCongiguration(builder.Configuration);

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BffCorsPolicy");

app.UseSession();

app.UseAuthentication();
app.UseMiddleware<StaticHeaderCheckForCorsImposing>();
app.UseAuthorization();

app.MapControllers();

app.Run();
