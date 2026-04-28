using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.CreatePersonalTraining
{
    internal class CreatePersonalTrainingHandler(IPersonalTrainingRepository _personalTrainingRepository) : IRequestHandler<CreatePersonalTraining, Result<CreatePersonalTrainingResult>>
    {
        public async Task<Result<CreatePersonalTrainingResult>> Handle(CreatePersonalTraining request, CancellationToken cancellationToken)
        {
            var instructorIdResult = InstructorId.From(request.InstructorId);
            if(instructorIdResult.Success is false)
                return Result<CreatePersonalTrainingResult>.Fail(instructorIdResult.Error!);

            var clientIdResult = ClientId.From(request.ClientId);
            if (clientIdResult.Success is false)
                return Result<CreatePersonalTrainingResult>.Fail(clientIdResult.Error!);

            var startsAtResult = StartsAt.From(request.Start);
            if (startsAtResult.Success is false)
                return Result<CreatePersonalTrainingResult>.Fail(startsAtResult.Error!);

            EndsAt? endsAt = null;
            if (request.End.HasValue)
            {
                var endsAtResult = EndsAt.From(request.End.Value);
                if (endsAtResult.Success is false)
                    return Result<CreatePersonalTrainingResult>.Fail(endsAtResult.Error!);
                endsAt = endsAtResult.Data;
            }

            var periodResult = TrainingPeriod.From(startsAtResult.Data!, endsAt);
            if (periodResult.Success is false)
                return Result<CreatePersonalTrainingResult>.Fail(periodResult.Error!);

            Comment? instructorComment = null;
            if(request.InstructorComment is not null)
            {
                var instructorCommentResult = Comment.From(request.InstructorComment);
                if (instructorCommentResult.Success is false)
                    return Result<CreatePersonalTrainingResult>.Fail(instructorCommentResult.Error!);

                instructorComment = instructorCommentResult.Data!;
            }

            var paymentStatus = request.IsPaid ? PaymentStatus.Paid : PaymentStatus.Unpaid;

            PersonalTraining personalTraining = PersonalTraining.Create(
                _personalTrainingRepository.NextIdentity(),
                instructorIdResult.Data!,
                clientIdResult.Data!,
                periodResult.Data!,
                paymentStatus,
                instructorComment: instructorComment
            );

            await _personalTrainingRepository.SaveAsync(personalTraining, cancellationToken);

            return Result<CreatePersonalTrainingResult>.Ok(new CreatePersonalTrainingResult(personalTraining.Id.Value));
        }
    }
}
