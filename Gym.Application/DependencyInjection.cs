using Gym.Application.Aspects;
using Gym.Domain._Shared.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Gym.Application.Tests")]

namespace Gym.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LogAspect<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(OperationLockAspect<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkAspect<,>));

            services.AddScoped<ITrainingBookingService, TrainingBookingService>();
            services.AddScoped<IChargeAccountService, ChargeAccountService>();
            services.AddScoped<ICompleteCalendarEventService, CompleteCalendarEventService>();
            services.AddScoped<ICancelCalendarEventService, CancelCalendarEventService>();
            services.AddScoped<ISubmitPollResponseService, SubmitPollResponseService>();

            return services;
        }

    }
}
