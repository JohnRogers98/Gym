using Gym.WebApplication.Features.Account.ChangePassword.Services;
using Gym.WebApplication.Features.Account.Details.Servises;
using Gym.WebApplication.Features.Account.History.Services;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Services;
using Gym.WebApplication.Features.Admin.CalendarEvents.States;
using Gym.WebApplication.Features.Admin.CalendarEvents.TableView.Services;
using Gym.WebApplication.Features.Admin.Clients.Creation.Services;
using Gym.WebApplication.Features.Admin.Clients.TableView.Services;
using Gym.WebApplication.Features.Admin.Instructors.Registration.Services;
using Gym.WebApplication.Features.Admin.Instructors.States;
using Gym.WebApplication.Features.Admin.Shared.Services;
using Gym.WebApplication.Features.Admin.Trainings.Creation.Services;
using Gym.WebApplication.Features.Admin.Trainings.States;
using Gym.WebApplication.Features.Calendar.Services;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Forms;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Models.Results;
using Gym.WebApplication.Features.Instructor.CreatePersonalTrainingPage.Services;
using Gym.WebApplication.Features.Login.Services;
using Gym.WebApplication.JSAdapters;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Providers;
using Gym.WebApplication.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Polly;
using Polly.Fallback;

namespace Gym.WebApplication.Extensions
{
    public static class ServiceCollectionExtensions
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

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IWebAppAuthService, WebAppAuthService>();
            services.AddScoped<IBasicAuthService, BasicAuthService>();
            services.AddScoped<IMockedAdminAuthService, MockedAdminAuthService>();
            services.AddScoped<UserAuthState>();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            return services;
        }

        public static IServiceCollection AddLocalStorage(this IServiceCollection services)
        {
            services.AddScoped<LocalStorageAdapter>();

            return services;
        }

        public static IServiceCollection AddCalendarEventServices(this IServiceCollection services)
        {
            services.AddScoped<IGetAllCalendarItemsService, GetAllCalendarItemsService>();
            services.AddScoped<IBookCalendarItemService, BookCalendarItemService>();

            services.AddScoped<IGetAllAdminCalendarEventsService, GetAllAdminCalendarEventsService>();
            services.Decorate<IGetAllAdminCalendarEventsService, CachableGetAllAdminCalendarEventsSetvice>();

            services.AddScoped<IGetAdminCalendarEventByIdService, GetAdminCalendarEventByIdService>();
            services.Decorate<IGetAdminCalendarEventByIdService, RetryableGetAdminCalendarEventByIdService>();

            services.AddScoped<ICreateCalendarEventService, CreateCalendarEventService>();
            services.AddScoped<ICalendarEventCreationState, CalendarEventCreationState>();

            services.AddScoped<ICancelCalendarEventService, CancelCalendarEventService>();
            services.AddScoped<ICalendarEventCancellationState, CalendarEventCancellationState>();

            return services;
        }

        public static IServiceCollection AddAccountServices(this IServiceCollection services)
        {
            services.AddScoped<AccountHistoryViewModelMapper>();
            services.AddScoped<IGetAllAccountHistoryItemsService, GetAllAccountHistoryItemsService>();
            services.AddScoped<IChangePasswordService, ChangePasswordService>();

            return services;
        }

        public static IServiceCollection AddInstructorServices(this IServiceCollection services)
        {
            services.AddScoped<IGetAllInstructorsService, GetAllInstructorsService>();
            services.Decorate<IGetAllInstructorsService, CachableGetAllInstructosSetvice>();

            services.AddScoped<IGetInstructorByIdService, GetInstructorByIdService>();
            services.Decorate<IGetInstructorByIdService, RetryableGetInstructorByIdService>();

            services.AddScoped<ICreateInstructorService, CreateInstructorService>();
            services.AddScoped<IInstructorRegisteredSharedState, InstructorRegistrationSharedState>();

            return services;
        }

        public static IServiceCollection AddTrainingServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateTrainingService, CreateTrainingService>();
            services.AddScoped<ITrainingCreationSharedState, TrainingCreationSharedState>();

            services.AddScoped<IGetAllTrainingsService, GetAllTrainingsService>();
            services.Decorate<IGetAllTrainingsService, CachableGetAllTrainingsSetvice>();

            services.AddScoped<IGetTrainingByIdService, GetTrainingByIdService>();
            services.Decorate<IGetTrainingByIdService, RetryableGetTrainingByIdService>();

            return services;
        }

        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddScoped<Features.Admin.Clients.TableView.Services.IGetAllClientsService, Features.Admin.Clients.TableView.Services.GetAllClientsService>();
            services.AddScoped<Features.Instructor.CreatePersonalTrainingPage.Services.IGetAllClientsService, Features.Instructor.CreatePersonalTrainingPage.Services.GetAllClientsService>();
            services.AddScoped<IGetClientDetailsService, GetClientDetailsService>();
            services.AddScoped<IChargeClientService, ChargeClientService>();
            services.AddScoped<ICreateClientService, CreateClientService>();

            return services;
        }

        public static IServiceCollection AddPersonalTrainingServices(this IServiceCollection services)
        {
            RequestHandlerRegistration
                .WithRequest<CreatePersonalTrainingFormModel>
                .WithResponse<CreatePersonalTrainingResult>
                .For<CreatePersonalTrainingService>
                .In(services)
                .DecorateWithHttpExceptionCatcher()
                .DecorateWithFailSnackbar();

            return services;
        }

        public static IServiceCollection AddResiliencePipelines(this IServiceCollection services)
        {
            services.AddResiliencePipeline<String, InstructorViewModel?>(nameof(GetInstructorByIdService), builder =>
            {
                builder
                    .AddFallback(new FallbackStrategyOptions<InstructorViewModel?>
                    {
                        FallbackAction = args => Outcome.FromResultAsValueTask((InstructorViewModel?)null)
                    })
                    .AddTimeout(TimeSpan.FromSeconds(5))
                    .AddRetry(new()
                    {
                        MaxRetryAttempts = 3,
                        ShouldHandle = new PredicateBuilder<InstructorViewModel?>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response => response is null)
                    });
            });

            services.AddResiliencePipeline<String, TrainingViewModel?>(nameof(GetTrainingByIdService), builder =>
            {
                builder
                    .AddFallback(new FallbackStrategyOptions<TrainingViewModel?>
                    {
                        FallbackAction = args => Outcome.FromResultAsValueTask((TrainingViewModel?)null)
                    })
                    .AddTimeout(TimeSpan.FromSeconds(5))
                    .AddRetry(new()
                    {
                        MaxRetryAttempts = 3,
                        ShouldHandle = new PredicateBuilder<TrainingViewModel?>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response => response is null)
                    });
            });

            services.AddResiliencePipeline<String, AdminCalendarItemViewModel?>(nameof(GetAdminCalendarEventByIdService), builder =>
            {
                builder
                    .AddFallback(new FallbackStrategyOptions<AdminCalendarItemViewModel?>
                    {
                        FallbackAction = args => Outcome.FromResultAsValueTask((AdminCalendarItemViewModel?)null)
                    })
                    .AddTimeout(TimeSpan.FromSeconds(5))
                    .AddRetry(new()
                    {
                        MaxRetryAttempts = 3,
                        ShouldHandle = new PredicateBuilder<AdminCalendarItemViewModel?>()
                            .Handle<HttpRequestException>()
                            .HandleResult(response => response is null)
                    });
            });

            services.AddScoped<IPipelineProvider, PipelineProvider>();

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
