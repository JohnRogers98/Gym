namespace Gym.Chat.Domain._Common
{
    public class DomainException : Exception
    {
        public DomainError Error { get; }

        public DomainException(DomainError error) : base(error.GetErrorMessage())
        {
            Error = error;  
        }
    }
}
