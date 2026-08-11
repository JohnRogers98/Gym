using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.PersonalTrainings;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class PersonalTrainingExtensions
    {
        public static PersonalTraining ToDomain(this PersonalTrainingEntity entity)
        {
            return PersonalTraining.Restore(
                PersonalTrainingId.From(entity.Id.ToString()).Unwrap(),
                InstructorId.From(entity.InstructorId.ToString()).Unwrap(),
                ClientId.From(entity.ClientId.ToString()).Unwrap(),
                Enum.Parse<PersonalTrainingStatus>(entity.Status),
                TrainingPeriod.From(
                    StartsAt.From(entity.Start).Unwrap(),
                    entity.End.HasValue ? EndsAt.From(entity.End.Value).Unwrap() : null
                ).Unwrap(),
                Enum.Parse<PaymentStatus>(entity.PaymentStatus),
                entity.InstructorComment is null ? null : Comment.From(entity.InstructorComment).Unwrap(),
                entity.ClientComment is null ? null : Comment.From(entity.ClientComment).Unwrap()
            );
        }

        public static PersonalTrainingEntity ToEntity(this PersonalTraining personalTraining)
        {
            return new()
            {
                Id = personalTraining.Id.Value.ToObjectId(),
                InstructorId = personalTraining.InstructorId.Value.ToObjectId(),
                ClientId = personalTraining.ClientId.Value.ToObjectId(),
                Status = personalTraining.Status.ToString(),
                Start = personalTraining.TrainingPeriod.StartsAt.Value,
                End = personalTraining.TrainingPeriod.EndsAt?.Value,
                PaymentStatus = personalTraining.PaymentStatus.ToString(),
                InstructorComment = personalTraining.InstructorComment?.Value,
                ClientComment = personalTraining.ClientComment?.Value
            };
        }
        
    }
}
