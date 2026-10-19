using Gym.Chat.Shared;
using Gym.Chat.WSApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Gym.Chat.WSApi.Hubs
{
    [Authorize]
    public class ChatHub : Hub<IChatClient>, IChatHub
    {
        [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
        public async Task SendEchoMessage(String message)
            => await Clients.User(base.Context.UserIdentifier!).ReceiveEchoMessage(message);
    }
}
