namespace Gym.Domain
{
    public record Result<T>
    {
        public Boolean Success { get; init; }
        public T? Data { get; init; }
        public String? Error { get; init; }

        private Result() { }

        public static Result<T> Ok(T data) => new() { Success = true, Data = data };

        public static Result<T> Fail(String error) => new() { Success = false, Error = error };
    }
}
