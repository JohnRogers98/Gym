using Gym.Abstractions.Query._CommonInfos;

namespace Gym.Abstractions.Query.PersonalTrainings
{
    public record PersonalTrainingProjection(
        String Id,
        InstructorInfo Instructor,
        ClientInfo Client,
        String Status,
        DateTime Start,
        DateTime? End,
        String PaymentStatus,
        String? InstructorComment,
        String? ClientComment);
}
