using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.Entities;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.ValueObjects;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.TrainingContext.ValueObjects;

namespace Gym.Domain.Tests
{
    public class FakeDataFixture
    {
        public AccountId AccountId => field ??= AccountId.From(Guid.NewGuid().ToString()).Data!;
        public UserId UserId => field ??= UserId.From(Guid.NewGuid().ToString()).Data!;
        public PollId PollId => field ??= PollId.From(Guid.NewGuid().ToString()).Data!;
        public CalendarEventId CalendarEventId => field ??= CalendarEventId.From(Guid.NewGuid().ToString()).Data!;
        public BookingId BookingId => field ??= BookingId.From(Guid.NewGuid().ToString());
        public TrainingId TrainingId => field ??= TrainingId.From(Guid.NewGuid().ToString()).Data!;

        public Account CreateAccount(Int32 availableTrainingsCount = default)
        {
            Account account = Account.Create(AccountId, UserId);

            if (availableTrainingsCount > 0)
            {
                account.Charge(availableTrainingsCount);
            }

            return account;
        }

        public CalendarEvent CreateCalendarEvent(Boolean isExpired = false, Int32? capacity = null, PollId? pollId = null)
        {
            StartsAt startsAt = isExpired ? StartsAt.From(DateTime.MinValue).Data! : StartsAt.From(DateTime.MaxValue).Data!;
            TrainingPeriod trainingPeriod = TrainingPeriod.From(startsAt).Data!;   

            return CalendarEvent.Create(
                CalendarEventId.From(Guid.NewGuid().ToString()).Unwrap(),
                trainingPeriod,
                capacity is null ? Capacity.Unlimited() : Capacity.From(capacity.Value).Data!, 
                TrainingId,
                pollId: pollId ?? PollId
                );
        }

        public Poll CreatePoll(
            String pollId = "poll_id",
            String pollTitle = "Is that the test poll",
            Boolean isResponseRequired = false,
            Boolean canAcceptManyChoices = true,
            List<ChoiceText>? choices = null)
        {
            return Poll.Create(
                PollId.From(pollId).Unwrap(),
                PollTitle.From(pollTitle).Unwrap(),
                IsResponseRequired.From(isResponseRequired),
                CanAcceptManyChoices.From(canAcceptManyChoices),
                choices ?? [this.CreateChoiceText("Yes"), this.CreateChoiceText("No")]
            ).Unwrap();
        }

        public ChoiceText CreateChoiceText(String text) => ChoiceText.From(text).Unwrap();

        public ChoiceId CreateChoiceId(Int32 id) => ChoiceId.From(id).Unwrap();

        public UserId GenerateUserId() => UserId.From(Guid.NewGuid().ToString()).Data!;

        public PollId GeneratePollId() => PollId.From(Guid.NewGuid().ToString()).Data!;

        public PollTitle CreatePollTitle(String title) => PollTitle.From(title).Data!;

        public Booking GenerateBooking()
        {
            return Booking.Create(
                BookingId.From(Guid.NewGuid().ToString()),
                UserId.From(Guid.NewGuid().ToString()).Data!,
                CalendarEventId.From(Guid.NewGuid().ToString()).Data!
                );
        }

    }
}
