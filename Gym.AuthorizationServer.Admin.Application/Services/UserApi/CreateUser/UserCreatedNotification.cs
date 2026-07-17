using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.CreateUser
{
    public record UserCreatedNotification(String UserId, String? FirstName, String? LastName, String Role) : INotification;
}
