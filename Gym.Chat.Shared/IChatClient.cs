namespace Gym.Chat.Shared
{
    public interface IChatClient
    {
        Task ReceiveEchoMessage(String message);
    }
}
