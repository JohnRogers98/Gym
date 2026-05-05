using Gym.WebDto.Responses.PersonalTraining;

namespace Gym.WebApplication.ViewModels
{
    public record PersonalTrainingViewModel
    {
        public required String Id { get; init; }
        public required InstructorInfo Instructor { get; init; }
        public required ClientInfo Client { get; init; }
        public required String Status { get; init; }
        public required DateTime UtcStart { get; init; }
        public DateTime LocalStart => UtcStart.ToLocalTime();
        public DateTime? UtcEnd { get; init; }
        public DateTime? LocalEnd => UtcEnd?.ToLocalTime();
        public required String PaymentStatus { get; init; }
        public String? InstructorComment { get; init; }
        public String? ClientComment { get; init; }
    }
}
