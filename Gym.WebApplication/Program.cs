using Gym.WebApplication;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features.Calendar.Services;
using Gym.WebApplication.Features.Login.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient(builder.Configuration);

builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IWebAppAuthService, WebAppAuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, WebAppAuthStateProvider>();

await builder.Build().RunAsync();