using Gym.WebApplication;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.States;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using MudBlazor;
using MudBlazor.Services;
using MudExtensions.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

builder.Logging.AddDebug();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.SetMinimumLevel(LogLevel.Trace);

builder.Services.AddOptionsFromConfiguration(builder.Configuration);

builder.Services.AddBffNamedClient(
    builder.Configuration.GetRequiredConfiguration("Bff:ClientName"),
    builder.Configuration.GetRequiredConfiguration("Bff:BaseUrl")
);

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.RequireInteraction = false;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddMudExtensions();

builder.Services.AddScoped<IAppSnackbarNotifier, AppSnackbarNotifier>();

builder.Services.AddAuthorizationCore(options => 
{
    options.AddPolicy("HasBasicAuth", policy =>
      policy.RequireAssertion(context =>
      {
          var acr = context.User.FindFirst(JwtRegisteredClaimNames.Acr)?.Value;
          return acr == "1fa";
      }));
});
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthenticationServices();

builder.Services.AddLocalStorage();

builder.Services.AddBffServices();

builder.Services.AddFormValidators();

await builder.Build().RunAsync();