namespace Gym.AuthorizationServer.Admin.Application.Abstractions
{

#nullable disable warnings

    public class Result<T>
    {
        public Boolean IsSuccess { get; }
        public Boolean IsFailed => !IsSuccess;
        public T Value { get; }
        public String ErrorCode { get; }
        public String ErrorDescription { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(String errorCode, String errorDescription)
        {
            IsSuccess = false;
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }

        public static Result<T> Success(T value) => new Result<T>(value);

        public static Result<T> Failure(String errorCode, String errorDescription = null)
            => new Result<T>(errorCode, errorDescription);
    }

    public class Result
    {
        public Boolean IsSuccess { get; }
        public Boolean IsFailed => !IsSuccess;
        public String ErrorCode { get; }
        public String ErrorDescription { get; }

        private Result()
        {
            IsSuccess = true;
        }

        private Result(String errorCode, String errorDescription)
        {
            IsSuccess = false;
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }

        public static Result Success() => new Result();

        public static Result Failure(String errorCode, String errorDescription = null)
            => new Result(errorCode, errorDescription);
    }

#nullable restore warnings
}
