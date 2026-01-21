using Gym.Application.Extensions;
using Gym.Application.Services.DomainEventPublisher;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.ClientAggregate;
using Gym.Domain.UserAggregate;
using Gym.Domain.UserAggregate.Authentication;
using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    internal class AuthenticateUserHandler(
        ITelegramSignatureVerifier _telegramSignatureVerifier,
        IUserRepository _userRepository,
        IUserQueryService _userQueryService,
        IClientRepository _clientRepository,
        IUnitOfWork _unitOfWork,
        IDomainEventPublisher _domainEventPublisher) : IRequestHandler<AuthenticateUserCommand, UserDetails>
    {
        public async Task<UserDetails> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            Result<ValidatedTelegramUserInfo> verificationResult = _telegramSignatureVerifier.Verify(request.escapedInitData);

            if (!verificationResult.Success)
                throw new ArgumentException(verificationResult.Error!.GetErrorMessage());

            User? user = await _userQueryService.GetByTelegramIdAsync(verificationResult.Data!.Id, cancellationToken);

            if(user is null)
            {
                UserId registeredUserId = await this.RegisterUser(verificationResult.Data.Id, cancellationToken);
                user = await _userQueryService.GetByIdAsync(registeredUserId, cancellationToken);
            }

            return user!.ToDetails();
        }

        private async Task<UserId> RegisterUser(TelegramId telegramId, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();
            try{
                UserId userId = _userRepository.NextIdentity();
                User newUser = User.Create(userId, UserRole.Client, telegramId);
                await _userRepository.SaveAsync(newUser, cancellationToken);

                await this.CreateClientFromRegisteredUser(userId, cancellationToken);

                await _unitOfWork.CommitAsync();
                return userId;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private async Task CreateClientFromRegisteredUser(UserId registeredUserId, CancellationToken cancellationToken)
        {
            Client client = Client.Create(_clientRepository.NextIdentity(), registeredUserId);
            await _clientRepository.SaveAsync(client, cancellationToken);
            await _domainEventPublisher.PublishAsync(client!.DomainEvents, cancellationToken);
        }

    }
}