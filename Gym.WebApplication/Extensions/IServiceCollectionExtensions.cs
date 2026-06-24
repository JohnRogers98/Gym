using Gym.WebApplication.Authentication;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Account.ChangePassword.Models.Forms;
using Gym.WebApplication.Features.Account.ChangePassword.Services;
using Gym.WebApplication.Features.Account.Details.Servises;
using Gym.WebApplication.Features.Account.History.Services;
using Gym.WebApplication.Features.Account.History.ViewModels;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Results;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Services;
using Gym.WebApplication.Features.Admin.CalendarEvents.TableView.Models;
using Gym.WebApplication.Features.Admin.CalendarEvents.TableView.Services;
using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Clients.Creation.Models.Results;
using Gym.WebApplication.Features.Admin.Clients.Creation.Services;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models.Forms;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models.Results;
using Gym.WebApplication.Features.Admin.Clients.TableView.Services;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Forms;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Results;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Services;
using Gym.WebApplication.Features.Admin.Shared.Models;
using Gym.WebApplication.Features.Admin.Shared.Services;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Models.Results;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Services;
using Gym.WebApplication.Features.Calendar.Services;
using Gym.WebApplication.Features.Client.Account.ChangePassword.Models.Results;
using Gym.WebApplication.Features.Client.Account.Details.Models;
using Gym.WebApplication.Features.Client.Account.History.Models;
using Gym.WebApplication.Features.Client.Calendar.Models;
using Gym.WebApplication.Features.Client.Schedule.Services;
using Gym.WebApplication.Features.Instructor.Calendar.Services;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Forms;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Results;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Services;
using Gym.WebApplication.JSAdapters;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Gym.WebApplication.Extensions
{
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
            services.AddScoped<IUserInfoService, UserInfoService>();

            services.AddScoped<ILogoutService, LogoutService>();

            return services;
        }

        public static IServiceCollection AddLocalStorage(this IServiceCollection services)
        {
            services.AddScoped<LocalStorageAdapter>();
            return services;
        }

        public static IServiceCollection AddCalendarEventServices(this IServiceCollection services)
        {
            RequestHandlerRegistration
               .WithRequest<GetAllCalendarItems>
               .WithResponse<IEnumerable<CalendarEventForClientViewModel>>
               .For<GetAllCalendarItemsService>
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
               .DecorateWithFailSnackbar();

            RequestHandlerRegistration
               .WithRequest<GetAllCalendarEventsForAdmin>
               .WithResponse<IEnumerable<CalendarEventForAdminViewModel>>
               .For<GetAllCalendarEventsForAdminService>
               .In(services)
               .DecorateWithResilience()
               .DecorateWithHttpExceptionCatcher()
               .DecorateWithFailSnackbar();

            RequestHandlerRegistration
               .WithRequest<GetCalendarEventByIdForAdmin>
               .WithResponse<CalendarEventForAdminViewModel>
               .For<GetCalendarEventByIdForAdminService>
               .In(services)
               .DecorateWithResilience()
               .DecorateWithHttpExceptionCatcher()
               .DecorateWithFailSnackbar();

            services.AddOperationStateNotifier<CreateCalendarEventFormModel, CreateCalendarEventResult>();
            RequestHandlerRegistration
             .WithRequest<CreateCalendarEventFormModel>
             .WithResponse<CreateCalendarEventResult>
             .For<CreateCalendarEventService>
             .In(services)
             .DecorateWithResilience()
             .DecorateWithHttpExceptionCatcher()
             .DecorateWithFailSnackbar()
             .DecorateWithNotifier();

            services.AddOperationStateNotifier<CancelCalendarEvent, CancelCalendarEventResult>();
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
               .WithRequest<GetInstructorCalendarEvents>
               .WithResponse<IEnumerable<CalendarEventForAdminViewModel>>
               .For<GetInstructorCalendarEventsService>
               .In(services)
               .DecorateWithResilience()
               .DecorateWithHttpExceptionCatcher()
               .DecorateWithFailSnackbar();

            RequestHandlerRegistration
               .WithRequest<GetClientCalendarEvents>
               .WithResponse<IEnumerable<CalendarEventForClientViewModel>>
               .For<GetClientCalendarEventsService>
               .In(services)
               .DecorateWithResilience()
               .DecorateWithHttpExceptionCatcher()
               .DecorateWithFailSnackbar();

            return services;
        }

        public static IServiceCollection AddAccountServices(this IServiceCollection services)
        {
            services.AddScoped<AccountHistoryViewModelMapper>();

            RequestHandlerRegistration
                .WithRequest<GetAllAccountHistoryItems>
                .WithResponse<IEnumerable<AccountHistoryItemViewModel>>
                .For<GetAllAccountHistoryItemsService>
                .In(services)
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar();

            RequestHandlerRegistration
               .WithRequest<ChangePasswordFormModel>
               .WithResponse<ChangePasswordResult>
               .For<ChangePasswordService>
               .In(services)
               .DecorateWithHttpExceptionCatcher()
               .DecorateWithFailSnackbar();

            return services;
        }

        public static IServiceCollection AddInstructorServices(this IServiceCollection services)
        {
            RequestHandlerRegistration
             .WithRequest<GetAllInstructors>
             .WithResponse<IEnumerable<InstructorViewModel>>
             .For<GetAllInstructorsService>
             .In(services)
             .DecorateWithResilience()
             .DecorateWithHttpExceptionCatcher()
             .DecorateWithFailSnackbar();

            RequestHandlerRegistration
             .WithRequest<GetInstructorById>
             .WithResponse<InstructorViewModel>
             .For<GetInstructorByIdService>
             .In(services)
             .DecorateWithResilience()
             .DecorateWithHttpExceptionCatcher()
             .DecorateWithFailSnackbar();

            services.AddOperationStateNotifier<InstructorRegistrationFormModel, CreateInstructorResult>();
            RequestHandlerRegistration
             .WithRequest<InstructorRegistrationFormModel>
             .WithResponse<CreateInstructorResult>
             .For<CreateInstructorService>
             .In(services)
             .DecorateWithResilience()
             .DecorateWithHttpExceptionCatcher()
             .DecorateWithFailSnackbar()
             .DecorateWithNotifier();

            return services;
        }

        public static IServiceCollection AddTrainingServices(this IServiceCollection services)
        {
            services.AddOperationStateNotifier<CreateTrainingFormModel, CreateTrainingResult>();
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
                .WithRequest<GetAllTrainings>
                .WithResponse<IEnumerable<TrainingViewModel>>
                .For<GetAllTrainingsService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar();

            RequestHandlerRegistration
                .WithRequest<GetTrainingById>
                .WithResponse<TrainingViewModel>
                .For<GetTrainingByIdService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher();

            return services;
        }

        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            RequestHandlerRegistration
                .WithRequest<GetAllClientsForAdmin>
                .WithResponse<IEnumerable<ClientViewModel>>
                .For<GetAllClientsForAdminService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar();

            RequestHandlerRegistration
                .WithRequest<GetAllClientsForInstructor>
                .WithResponse<IEnumerable<ClientViewModel>>
                .For<GetAllClientsForInstructorService>
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

            services.AddOperationStateNotifier<ChargeClientFormModel, ChargeClientResult>();
            RequestHandlerRegistration
                .WithRequest<ChargeClientFormModel>
                .WithResponse<ChargeClientResult>
                .For<ChargeClientService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar()
                .DecorateWithNotifier();

            services.AddOperationStateNotifier<CreateClientFormModel, CreateClientResult>();
            RequestHandlerRegistration
                .WithRequest<CreateClientFormModel>
                .WithResponse<CreateClientResult>
                .For<CreateClientService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar()
                .DecorateWithNotifier();

            return services;
        }

        public static IServiceCollection AddPersonalTrainingServices(this IServiceCollection services)
        {
            RequestHandlerRegistration
                .WithRequest<CreatePersonalTrainingFormModel>
                .WithResponse<CreatePersonalTrainingResult>
                .For<CreatePersonalTrainingService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar();

            RequestHandlerRegistration
                .WithRequest<GetInstructorPersonalTrainings>
                .WithResponse<IEnumerable<PersonalTrainingViewModel>>
                .For<GetInstructorPersonalTrainingsService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar();

            RequestHandlerRegistration
               .WithRequest<GetClientPersonalTrainings>
               .WithResponse<IEnumerable<PersonalTrainingViewModel>>
               .For<GetClientPersonalTrainingsService>
               .In(services)
               .DecorateWithResilience()
               .DecorateWithHttpExceptionCatcher()
               .DecorateWithFailSnackbar();

            services.AddOperationStateNotifier<CancelPersonalTraining, CancelPersonalTrainingResult>();
            RequestHandlerRegistration
                .WithRequest<CancelPersonalTraining>
                .WithResponse<CancelPersonalTrainingResult>
                .For<CancelPersonalTrainingService>
                .In(services)
                .DecorateWithResilience()
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar()
                .DecorateWithNotifier();

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
}
