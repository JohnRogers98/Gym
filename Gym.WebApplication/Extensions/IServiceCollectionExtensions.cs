using FluentValidation;
using Gym.WebApplication.Authentication;
using Gym.WebApplication.JSAdapters;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.RequestHandlers;
using Gym.WebApplication.Scanners;
using Gym.WebApplication.ViewModels.AccountHistory;
using Microsoft.AspNetCore.Components.Authorization;

namespace Gym.WebApplication.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOptionsFromConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<BffOptions>()
            .Bind(configuration.GetRequiredSection(BffOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddBffNamedClient(this IServiceCollection services, String key, String baseUrl)
    {
        services.AddHttpClient(key, client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("X-Static-Header", "1");
        });

        return services;
    }

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<UserAuthState>();
        services.AddScoped<AuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateProvider>());

        services.AddScoped<ICheckSessionService, CheckSessionService>();
        services.AddScoped<ITelegramInitService, TelegramInitService>();
        services.AddScoped<ISessionInfoService, SessionInfoService>();

        services.AddScoped<ILogoutService, LogoutService>();

        return services;
    }

    public static IServiceCollection AddLocalStorage(this IServiceCollection services)
    {
        services.AddScoped<LocalStorageAdapter>();
        return services;
    }

    public static IServiceCollection AddFormValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateClientFormModel>, CreateClientFormModel.Validator>();
        services.AddScoped<IValidator<ChangePasswordFormModel>, ChangePasswordFormModel.Validator>();
        services.AddScoped<IValidator<ChargeClientFormModel>, ChargeClientFormModel.Validator>();
        services.AddScoped<IValidator<CreatePollFormModel>, CreatePollFormModel.Validator>();
        services.AddScoped<IValidator<CreateCalendarEventFormModel>, CreateCalendarEventFormModel.Validator>();
        services.AddScoped<IValidator<CreateTrainingFormModel>, CreateTrainingFormModel.Validator>();
        services.AddScoped<IValidator<CreateInstructorFormModel>, CreateInstructorFormModel.Validator>();
        services.AddScoped<IValidator<CreatePersonalTrainingFormModel>, CreatePersonalTrainingFormModel.Validator>();

        return services;
    }

    public static IServiceCollection AddBffServices(this IServiceCollection services)
    {
        services.AddScoped<AccountHistoryViewModelMapper>();
        RequestHandlersScanner.ScanAssembly(typeof(Program).Assembly, services);

        return services;
    }

    public static IServiceCollection AddOperationStateNotifier<TRequest, TResponse>(this IServiceCollection services)
    {
        services.AddScoped<AsyncOperationStateNotifier<TRequest, TResponse>>();
        return services;
    }

}