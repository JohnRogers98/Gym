namespace Gym.WebApplication.Operations
{
    public class AsyncOperation<T>
    {
        public Boolean Succeeded { get; }
        public T? Data { get; }
        public String? ErrorMessage { get; }
        public ErrorType? ErrorType { get; }
        public Int32? HttpStatusCode { get; }

        private AsyncOperation(Boolean succeeded, T? data, String? errorMessage, ErrorType? errorType, Int32? httpStatusCode)
        {
            Succeeded = succeeded;
            Data = data;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
            HttpStatusCode = httpStatusCode;
        }

        public static AsyncOperation<T> Success(T data) => new(true, data, null, null, null);

        public static AsyncOperation<T> Failure(String message, ErrorType errorType, Int32? httpStatusCode = null)
            => new(false, default, message, errorType, httpStatusCode);
    }

    public enum ErrorType
    {
        Validation,
        Conflict,
        Unauthorized,
        Forbidden,
        NotFound,
        Timeout,
        Network,
        ServerError,
        Unknown
    }
}
