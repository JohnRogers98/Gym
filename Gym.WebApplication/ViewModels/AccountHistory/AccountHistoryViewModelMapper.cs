using Gym.WebApplication.Extensions;
using Gym.WebDto.Responses.Account;

namespace Gym.WebApplication.ViewModels.AccountHistory
{
    public class AccountHistoryViewModelMapper
    {
        public AccountHistoryItemViewModel ToViewModel(AccountHistoryDto dto)
        {
            return dto.Operation switch
            {
                "AccountCreatedDomainEvent" => new AccountCreatedViewModel { UtcOccurredAt = dto.OccurredAt },

                "AccountChargedDomainEvent" => new AccountChargedViewModel
                {
                    UtcOccurredAt = dto.OccurredAt,
                    ByCount = dto.Payload.GetRequiredValue<Int32>(nameof(AccountChargedViewModel.ByCount)),
                    Reason = dto.Payload.GetRequiredValue<String>(nameof(AccountChargedViewModel.Reason))
                },

                "TrainingBookedDomainEvent" => new TrainingBookedViewModel
                {
                    UtcOccurredAt = dto.OccurredAt,
                    TrainingName = dto.Payload.GetRequiredValue<String>(nameof(TrainingBookedViewModel.TrainingName))
                },

                "TrainingCompletedDomainEvent" => new TrainingCompletedViewModel
                {
                    UtcOccurredAt = dto.OccurredAt,
                    TrainingName = dto.Payload.GetRequiredValue<String>(nameof(TrainingBookedViewModel.TrainingName))
                },

                "TrainingCancelledDomainEvent" => new TrainingCancelledViewModel
                {
                    UtcOccurredAt = dto.OccurredAt,
                    TrainingName = dto.Payload.GetRequiredValue<String>(nameof(TrainingCancelledViewModel.TrainingName))
                },

                _ => throw new NotImplementedException()
            };
        }
    }
}
