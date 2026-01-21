using Gym.Domain._Common;

namespace Gym.Domain._Exceptions
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
