using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PersonalTrainingContext.Events;
using Gym.Domain.PersonalTrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.PersonalTrainings.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(PersonalTrainingCreatedDto dto)
        {
            return PersonalTrainingCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                PersonalTrainingId.From(dto.PersonalTrainingId).Unwrap(),
                InstructorId.From(dto.InstructorId).Unwrap(),
                ClientId.From(dto.ClientId).Unwrap()
            );
        }

        private DomainEvent ToDomainEvent(PersonalTrainingCancelledDto dto)
        {
            return PersonalTrainingCancelledDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                PersonalTrainingId.From(dto.PersonalTrainingId).Unwrap(),
                InstructorId.From(dto.InstructorId).Unwrap(),
                ClientId.From(dto.ClientId).Unwrap()
            );
        }
    }
}
