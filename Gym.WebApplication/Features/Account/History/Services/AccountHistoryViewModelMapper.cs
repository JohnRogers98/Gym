using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features.Account.History.ViewModels;
using Gym.WebDto.Responses.Account;

namespace Gym.WebApplication.Features.Account.History.Services
{
    public class AccountHistoryViewModelMapper
    {
        public AccountHistoryItemViewModel ToViewModel(AccountHistoryDto dto)
        {
            return dto.Operation switch
            {
                "AccountCreatedDomainEvent" => new AccountCreatedViewModel { OccurredAt = dto.OccurredAt },

                "AccountChargedDomainEvent" => new AccountChargedViewModel
                {
                    OccurredAt = dto.OccurredAt,
                    ByCount = dto.Payload.GetRequiredValue<Int32>(nameof(AccountChargedViewModel.ByCount)),
                    Reason = dto.Payload.GetRequiredValue<String>(nameof(AccountChargedViewModel.Reason))
                },

                "TrainingBookedDomainEvent" => new TrainingBookedViewModel
                {
                    OccurredAt = dto.OccurredAt,
                    TrainingName = dto.Payload.GetRequiredValue<String>(nameof(TrainingBookedViewModel.TrainingName))
                },

                "TrainingCompletedDomainEvent" => new TrainingCompletedViewModel
                {
                    OccurredAt = dto.OccurredAt,
                    TrainingName = dto.Payload.GetRequiredValue<String>(nameof(TrainingBookedViewModel.TrainingName))
                },

                "TrainingCancelledDomainEvent" => new TrainingCancelledViewModel
                {
                    OccurredAt = dto.OccurredAt,
                    TrainingName = dto.Payload.GetRequiredValue<String>(nameof(TrainingCancelledViewModel.TrainingName))
                },

                _ => throw new NotImplementedException()
            };
        }
    }
}
