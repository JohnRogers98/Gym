using Gym.Domain.InstructorContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.Instructors.EventsDto
{
    [EventSerializationForm<InstructorCreatedDomainEvent>]
    internal record InstructorCreatedDto(String Id, DateTime occurredOn, String InstructorId);
}
