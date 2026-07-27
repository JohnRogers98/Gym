using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PersonalTrainingContext.Errors;
using Gym.Domain.PersonalTrainingContext.Events;
using Gym.Domain.PersonalTrainingContext.ValueObjects;

namespace Gym.Domain.PersonalTrainingContext
{
    public class PersonalTraining : AggregateRoot
    {
        public PersonalTrainingId Id { get; }
        public InstructorId InstructorId { get; private set; }
        public ClientId ClientId { get; private set; }

        public PersonalTrainingStatus Status { get; private set; }

        public TrainingPeriod TrainingPeriod { get; private set; }

        public PaymentStatus PaymentStatus { get; private set; }

        public Comment? ClientComment {  get; private set; }
        public Comment? InstructorComment {  get; private set; }

        private PersonalTraining(
            PersonalTrainingId id,
            InstructorId instructorId,
            ClientId clientId,
            PersonalTrainingStatus status,
            TrainingPeriod trainingPeriod,
            PaymentStatus payment,
            Comment? clientComment,
            Comment? instructorComment)
        {
            Id = id;
            InstructorId = instructorId;
            ClientId = clientId;
            Status = status;
            TrainingPeriod = trainingPeriod;
            PaymentStatus = payment;
            ClientComment = clientComment;
            InstructorComment = instructorComment;
        }

        public static PersonalTraining Create(
            PersonalTrainingId id,
            InstructorId instructorId,
            ClientId clientId,
            TrainingPeriod trainingPeriod,
            PaymentStatus payment,
            Comment? clientComment = null,
            Comment? instructorComment = null)
        {
            PersonalTraining personalTraining = new(id, instructorId, clientId, PersonalTrainingStatus.Upcoming, trainingPeriod, payment, clientComment, instructorComment);
            personalTraining.AddDomainEvent(PersonalTrainingCreatedDomainEvent.Create(personalTraining.Id, personalTraining.InstructorId, personalTraining.ClientId));

            return personalTraining;
        }

        public static PersonalTraining Restore(
            PersonalTrainingId id,
            InstructorId instructorId,
            ClientId clientId,
            PersonalTrainingStatus status,
            TrainingPeriod trainingPeriod,
            PaymentStatus payment, 
            Comment? clientComment = null,
            Comment? instructorComment = null)
            => new PersonalTraining(id, instructorId, clientId, status, trainingPeriod, payment, clientComment, instructorComment);

        public Result Cancel()
        {
            if (Status != PersonalTrainingStatus.Upcoming)
                return Result.Fail(CancelPersonalTrainingError.Create(Id));

            base.AddDomainEvent(PersonalTrainingCancelledDomainEvent.Create(Id, InstructorId, ClientId));
            Status = PersonalTrainingStatus.Cancelled;

            return Result.Ok();
        } 

        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(InstructorId)}: {InstructorId} \t {nameof(ClientId)}: {ClientId}";
    }
}
