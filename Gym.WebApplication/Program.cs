using Gym.WebApplication;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features.Account.Services;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Services;
using Gym.WebApplication.Features.Admin.Instructors.States;
using Gym.WebApplication.Features.Admin.Shared.Services;
using Gym.WebApplication.Features.Calendar.Services;
using Gym.WebApplication.Features.Login.Services;
using Gym.WebApplication.JSAdapters;
using Microsoft.AspNetCore.Components.Authorization;
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

builder.Services.AddResiliencePipelines();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient(builder.Configuration);

builder.Services.AddScoped<IWebAppAuthService, WebAppAuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, WebAppAuthStateProvider>();

builder.Services.AddScoped<LocalStorageAdapter>();

builder.Services.AddScoped<IGetAllCalendarItemsService, GetAllCalendarItemsService>();
builder.Services.AddScoped<IBookCalendarItemService, BookCalendarItemService>();

builder.Services.AddScoped<AccountHistoryViewModelMapper>();
builder.Services.AddScoped<IGetAllAccountHistoryItemsService, GetAllAccountHistoryItemsService>();

builder.Services.AddScoped<IGetAllInstructorsService, GetAllInstructorsService>();

builder.Services.AddScoped<IGetInstructorByIdService, GetInstructorByIdService>();
builder.Services.Decorate<IGetInstructorByIdService, RetryableGetInstructorByIdService>();

builder.Services.AddScoped<ICreateInstructorService, CreateInstructorService>();
builder.Services.AddScoped<IInstructorRegisteredSharedState, InstructorRegistrationSharedState>();

await builder.Build().RunAsync();