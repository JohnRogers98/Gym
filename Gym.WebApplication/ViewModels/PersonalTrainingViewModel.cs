using Gym.WebApplication.Features._Common;
using Gym.WebDto.Responses.PersonalTraining;

namespace Gym.WebApplication.ViewModels
{
    public record PersonalTrainingViewModel : ITimeBasedItem
    {
        public required String Id { get; init; }
        public required InstructorInfo Instructor { get; init; }
        public required ClientInfo Client { get; init; }
        public required String Status { get; init; }
        public required DateTime UtcStart { get; init; }
        public DateTime? UtcEnd { get; init; }
        public required String PaymentStatus { get; init; }
        public String? InstructorComment { get; init; }
        public String? ClientComment { get; init; }

        public DateTime Start => UtcStart.ToLocalTime();

        public DateTime? End => UtcEnd?.ToLocalTime();

        public Boolean IsPaid => PaymentStatus == "Paid";

        public Boolean IsCancelled => Status == "Cancelled";
    }
}
