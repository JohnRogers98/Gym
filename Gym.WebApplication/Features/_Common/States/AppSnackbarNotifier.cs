namespace Gym.WebApplication.Features._Common.States
{
    public interface IAppSnackbarNotifier
    {
        void ShowMessage(String message, MessageSeverity severity);
        
        event Action<String, MessageSeverity>? OnMessage;
    }

    public enum MessageSeverity
    {
        Error,
        Success
    }

    public class AppSnackbarNotifier : IAppSnackbarNotifier
    {
        public event Action<String, MessageSeverity>? OnMessage;

        public void ShowMessage(String message, MessageSeverity severity)
        {
            OnMessage?.Invoke(message, severity);
        }
    }
}
