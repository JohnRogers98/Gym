using Gym.Chat.Shared;
using Microsoft.AspNetCore.SignalR.Client;

namespace Gym.Chat.Client
{
    public sealed class ChatMediator : IAsyncDisposable
    {
        private readonly HubConnection _connection;

        public event Func<String, Task>? ReceiveEchoMessage;

        public ChatMediator(Func<HubConnection> connectionFactory)
        {
            _connection = connectionFactory();
            this.SubscribeToHubEvents();
        }
        private void SubscribeToHubEvents()
        {
            _connection.On<String>(
                nameof(IChatClient.ReceiveEchoMessage),
                message => ReceiveEchoMessage?.Invoke(message) ?? Task.CompletedTask
            );
        }

        public Task StartAsync(CancellationToken cancellationToken = default) 
            => _connection.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken = default) 
            => _connection.StopAsync(cancellationToken);

        public Task SendEchoMessageAsync(String message, CancellationToken cancellationToken = default)
            => _connection.SendAsync(HubMethods.Chat.SendEchoMessage, message, cancellationToken);

        public ValueTask DisposeAsync() => _connection.DisposeAsync();
    }
}
