namespace Gym.OAuth.Extensions
{
#nullable disable warnings
    public class HttpResult<T>
    {
        public Boolean IsSuccess { get; }
        public Boolean IsFailure => !IsSuccess;
        public T Value { get; }
        public OAuthError Error { get; }

        private HttpResult(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private HttpResult(OAuthError error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static HttpResult<T> Success(T value) => new HttpResult<T>(value);

        public static HttpResult<T> Failure(OAuthError error)
            => new HttpResult<T>(error);

        public static HttpResult<T> Failure(String error, String errorDescription = null)
            => Failure(new OAuthError() { Error = error, ErrorDescription = errorDescription});
    }
#nullable restore warnings

    public class OAuthError
    {
        public String? Error { get; set; }
        public String? ErrorDescription { get; set; }
        public String? ErrorUri { get; set; }
    }
}
