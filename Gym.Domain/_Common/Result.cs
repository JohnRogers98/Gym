namespace Gym.Domain._Common
{
    public record Result<T>
    {
        public Boolean Success { get; init; }
        public T? Data { get; init; }
        public DomainError? Error { get; init; }

        private Result() { }

        public static Result<T> Ok(T data) => new() { Success = true, Data = data };

        public static Result<T> Fail(DomainError error) => new() { Success = false, Error = error };
    }

    public record Result
    {
        public Boolean Success { get; init; }
        public DomainError? Error { get; init; }

        private Result() { }

        public static Result Ok() => new() { Success = true };

        public static Result Fail(DomainError error) => new() { Success = false, Error = error };
    }
}
