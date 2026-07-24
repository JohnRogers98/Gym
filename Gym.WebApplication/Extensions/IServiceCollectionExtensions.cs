using FluentValidation;
using Gym.WebApplication.Authentication;
using Gym.WebApplication.RequestHandlers;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.JSAdapters;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Gym.WebApplication.ViewModels.AccountHistory;

namespace Gym.WebApplication.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddScoped<CookieHandler>();
        services.AddScoped(sp =>
        {
            var cookieHandler = sp.GetRequiredService<CookieHandler>();

            if (cookieHandler.InnerHandler == null)
            {
                cookieHandler.InnerHandler = new HttpClientHandler();
            }

            return new HttpClient(cookieHandler)
            {
                BaseAddress = new Uri(configuration["WebApiBaseUrl"]!)
            };
        });

        return services;
    }

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
        RequestHandlerRegistration
            .WithRequest<ListUserRoles>
            .WithResponse<ListUserRolesResult>
            .For<ListUserRolesService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<CreateClientFormModel>
            .WithResponse<CreateClientResult>
            .For<CreateClientService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<CheckUsernameExistence>
            .WithResponse<CheckUsernameExistenceResult>
            .For<CheckUsernameExistenceService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<GetClientDetails>
            .WithResponse<ClientViewModel>
            .For<GetClientDetailsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        services.AddScoped<AccountHistoryViewModelMapper>();
        RequestHandlerRegistration
            .WithRequest<GetAccountHistoryItems>
            .WithResponse<IEnumerable<AccountHistoryItemViewModel>>
            .For<GetAccountHistoryItemsService>
            .In(services)
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<ChangePasswordFormModel>
            .WithResponse<ChangePasswordResult>
            .For<ChangePasswordService>
            .In(services)
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<ListAvailableClientCalendarItems>
            .WithResponse<IEnumerable<CalendarEventForClientViewModel>>
            .For<ListAvailableClientCalendarItemsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<BookCalendarItem>
            .WithResponse<BookCalendarItemResult>
            .For<BookCalendarItemService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<ListClientsForAdmin>
            .WithResponse<IEnumerable<ClientViewModel>>
            .For<ListClientsForAdminService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<ChargeClientFormModel>
            .WithResponse<ChargeClientResult>
            .For<ChargeClientService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<CreateCalendarEventFormModel>
            .WithResponse<CreateCalendarEventResult>
            .For<CreateCalendarEventService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<ListCalendarEventsForAdmin>
            .WithResponse<IEnumerable<CalendarEventForAdminViewModel>>
            .For<ListCalendarEventsForAdminService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<CancelCalendarEvent>
            .WithResponse<CancelCalendarEventResult>
            .For<CancelCalendarEventService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<ListInstructors>
            .WithResponse<IEnumerable<InstructorViewModel>>
            .For<ListInstructorsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<ListTrainings>
            .WithResponse<IEnumerable<TrainingViewModel>>
            .For<ListTrainingsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<CreateTrainingFormModel>
            .WithResponse<CreateTrainingResult>
            .For<CreateTrainingService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<CreateInstructorFormModel>
            .WithResponse<CreateInstructorResult>
            .For<CreateInstructorService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<ListSessionClientCalendarEvents>
            .WithResponse<IEnumerable<CalendarEventForClientViewModel>>
            .For<ListSessionClientCalendarEventsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<ListSessionClientPersonalTrainings>
            .WithResponse<IEnumerable<PersonalTrainingViewModel>>
            .For<ListSessionClientPersonalTrainingsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<ListClientsForInstructor>
            .WithResponse<IEnumerable<ClientViewModel>>
            .For<ListClientsForInstructorService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<CreatePersonalTrainingFormModel>
            .WithResponse<CreatePersonalTrainingResult>
            .For<CreatePersonalTrainingService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<CancelPersonalTraining>
            .WithResponse<CancelPersonalTrainingResult>
            .For<CancelPersonalTrainingService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();

        RequestHandlerRegistration
            .WithRequest<ListSessionInstructorCalendarEvents>
            .WithResponse<IEnumerable<CalendarEventForAdminViewModel>>
            .For<ListSessionInstructorCalendarEventsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        RequestHandlerRegistration
            .WithRequest<ListSessionInstructorPersonalTrainings>
            .WithResponse<IEnumerable<PersonalTrainingViewModel>>
            .For<ListSessionInstructorPersonalTrainingsService>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar();

        return services;
    }

    public static IServiceCollection AddOperationStateNotifier<TRequest, TResponse>(this IServiceCollection services)
    {
        services.AddScoped<AsyncOperationStateNotifier<TRequest, TResponse>>();
        return services;
    }

}

public class CookieHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}
