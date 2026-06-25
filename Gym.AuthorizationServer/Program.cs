using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
    //options.KnownProxies.Clear();
    //options.KnownIPNetworks.Clear();
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestHeaders.Add("X-Static-Header");
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var corsOrigins = builder.Configuration.GetRequiredSection("CorsOrigins").Get<String[]>();
if (corsOrigins is null)
    throw new InvalidOperationException("No CorsOrigins presented");

builder.Services.AddCorsPolicy("AuthenticationServerCorsPolicy", corsOrigins);

builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddMongoOptions(builder.Configuration);

var mongoOptions = new MongoOptions();
builder.Configuration.GetRequiredSection("MongoDb").Bind(mongoOptions);
builder.Services.AddMongoInfrastructure(mongoOptions);

builder.Services.AddRepositories();
builder.Services.AddPasswordHashingServices();

builder.Services.AddTelegramBotToken(builder.Configuration);
builder.Services.AddRsaSigningService(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddAuthenticationSchemes();

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AuthenticationServerCorsPolicy");

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.UseCacheControlHeader();

app.UseUnitOfWork();

app.MapRazorPages()
   .WithStaticAssets();

app.MapControllers();

app.Run();
