using Gym.WebApplication.Features.Admin.Instructors.Registration.Models.Results;

namespace Gym.WebApplication.Features.Admin.Instructors.States
{
    public interface IInstructorRegisteredSharedState
    {
        event Action<CreateInstructorResult>? InstructorCreated;

        void Notify(CreateInstructorResult createdInstructorViewModel);
    }

    public class InstructorRegistrationSharedState : IInstructorRegisteredSharedState
    {
        public event Action<CreateInstructorResult>? InstructorCreated;

        public void Notify(CreateInstructorResult createdInstructorViewModel)
        {
            InstructorCreated?.Invoke(createdInstructorViewModel);
        }
    }
}
