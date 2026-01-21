namespace Gym.Domain._Common
{
    public abstract class DomainError
    {
        public String ErrorTag { get; }

        protected DomainError(String errorTag) 
        {
            ErrorTag = errorTag;
        }

        public virtual String GetErrorMessage()
        {
            return ErrorTag;
        }

        public override String ToString() => GetErrorMessage();
    }
}
