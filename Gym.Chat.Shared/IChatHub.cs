namespace Gym.Chat.Shared
{
    public interface IChatHub
    {
        Task SendEchoMessage(String message);
    }
}
