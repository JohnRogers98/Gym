using Gym.Application.Extensions;
using Gym.Application.Services.DomainEventPublisher;
using Gym.Domain;
using Gym.Domain.UserAggregate;
using Gym.Domain.UserAggregate.Authentication;
using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    internal class AuthenticateUserHandler(
        ITelegramSignatureVerifier _telegramSignatureVerifier,
        IUserRepository _userRepository,
        IUserQueryService _userQueryService,
        IDomainEventPublisher _domainEventPublisher) : IRequestHandler<AuthenticateUserCommand, UserDetails>
    {
        public async Task<UserDetails> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            Result<ValidatedTelegramUserInfo> verificationResult = _telegramSignatureVerifier.Verify(request.escapedInitData);

            if (!verificationResult.Success)
                throw new ArgumentException(verificationResult.Error);

            User? user = await _userQueryService.GetByTelegramIdAsync(verificationResult.Data!.Id, cancellationToken);

            if(user is null)
            {
                UserId userId = _userRepository.NextIdentity();
                User newUser = User.Create(userId, UserRole.Client, verificationResult.Data.Id);
                await _userRepository.SaveAsync(newUser, cancellationToken);
                
                await _domainEventPublisher.PublishAsync(newUser!.DomainEvents, cancellationToken);

                user = await _userQueryService.GetByIdAsync(userId, cancellationToken);
            }

            return user!.ToDetails();
        }

    }
}