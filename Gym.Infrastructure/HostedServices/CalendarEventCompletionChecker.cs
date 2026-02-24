using Gym.Domain._Shared;
using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gym.Infrastructure.HostedServices
{
    internal class CalendarEventCompletionChecker(
        [FromKeyedServices(nameof(CalendarEventCompletionChecker))]PeriodicTimer _periodicTimer,
        IServiceScopeFactory _serviceLocator) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer periodicTimer = _periodicTimer;

            while (await periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                await using var scope = _serviceLocator.CreateAsyncScope();
                IServiceProvider serviceProvider = scope.ServiceProvider;

                var calendarEventRepository = serviceProvider.GetRequiredService<ICalendarEventRepository>();
                var accountRepository = serviceProvider.GetRequiredService<IAccountRepository>();
                var pastCalendarEventsFinder = serviceProvider.GetRequiredService<IPastCalendarEventsFinder>();
                var completeCalendarEventService = serviceProvider.GetRequiredService<ICompleteCalendarEventService>();
                var mongoUnitOfWork = serviceProvider.GetRequiredService<MongoUnitOfWork>();

                try
                {
                    await mongoUnitOfWork.BeginTransactionAsync(stoppingToken);
                    
                    IEnumerable<CalendarEvent> pastCalendarEvents = await pastCalendarEventsFinder.GetPastCalendarEventsAsync(DateTime.UtcNow, stoppingToken);

                    if (pastCalendarEvents.Any() is false)
                        continue;

                    foreach (var aPastCalendarEvent in pastCalendarEvents)
                    {
                        List<Account> accounts = new();
                        foreach (UserId bookingUser in aPastCalendarEvent.Bookings)
                        {
                            Account anAccount = await accountRepository.GetByIdAsync(AccountId.From(bookingUser), stoppingToken);
                            accounts.Add(anAccount);
                        }

                        completeCalendarEventService.Complete(aPastCalendarEvent, accounts);
                       
                        await calendarEventRepository.SaveAsync(aPastCalendarEvent, stoppingToken);

                        foreach (Account anAccount in accounts)
                        {
                            await accountRepository.SaveAsync(anAccount, stoppingToken);
                        }
                    }

                    await mongoUnitOfWork.CommitAsync(stoppingToken);
                }
                catch (Exception)
                {
                    await mongoUnitOfWork.RollbackAsync(stoppingToken);
                    throw;
                }
            }
        }

    }
}
