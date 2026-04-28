using Gym.WebApplication;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.States;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Polly;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

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

builder.Services.AddScoped<IAppSnackbarNotifier, AppSnackbarNotifier>();

builder.Services.AddResiliencePipelines();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient(builder.Configuration);

builder.Services.AddAuthenticationServices();

builder.Services.AddLocalStorage();

builder.Services.AddCalendarEventServices();

builder.Services.AddAccountServices();

builder.Services.AddInstructorServices();

builder.Services.AddTrainingServices();

builder.Services.AddClientServices();

builder.Services.AddPersonalTrainingServices();

await builder.Build().RunAsync();