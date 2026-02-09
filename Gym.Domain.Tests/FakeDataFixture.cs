using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.TrainingContext;

namespace Gym.Domain.Tests
{
    public class FakeDataFixture
    {
        public AccountId AccountId => field ??= AccountId.From(Guid.NewGuid().ToString());
        public UserId UserId => field ??= UserId.From(Guid.NewGuid().ToString());
        public CalendarEventId CalendarEventId => field ??= CalendarEventId.From(Guid.NewGuid().ToString());
        public BookingId BookingId => field ??= BookingId.From(Guid.NewGuid().ToString());

        public Account CreateAccount(Int32 availableTrainingsCount = default)
        {
            Account account = Account.Create(AccountId, UserId);

            if (availableTrainingsCount > 0)
            {
                account.Charge(availableTrainingsCount);
            }

            return account;
        }

        public CalendarEvent CreateCalendarEvent(Boolean isExpired = false, Int32? maxClientCount = null)
        {
            return CalendarEvent.Create(
                CalendarEventId.From(Guid.NewGuid().ToString()),
                isExpired ? DateTime.MinValue : DateTime.MaxValue,
                null,
                TrainingInfo.Create(TrainingId.From(Guid.NewGuid().ToString()), "kangoo", null),
                maxClientCount: maxClientCount
                );
        }
    }
}
