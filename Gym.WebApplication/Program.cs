using Gym.WebApplication;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features.Calendar.Services;
using Gym.WebApplication.Features.Login.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

builder.Services.AddMudServices();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IWebAppAuthService, WebAppAuthService>();

await builder.Build().RunAsync();