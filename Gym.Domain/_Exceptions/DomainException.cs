namespace Gym.Domain._Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(String message) : base(message) { }

        public DomainException(String message, Exception innerException) : base(message, innerException) { }
    }
}
