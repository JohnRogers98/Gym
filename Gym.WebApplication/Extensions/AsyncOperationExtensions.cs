namespace Gym.WebApplication.Operations;

public static class AsyncOperationExtensions
{
    extension<T>(AsyncOperation<T> asyncOperation)
    {
        public static AsyncOperation<T> UnknownResponseType(Int32? statusCode = null) 
            => AsyncOperation<T>.Failure($"Unknown response type.", ErrorType.Unknown, statusCode);

        public static AsyncOperation<T> EmptyResponseBody()
            => AsyncOperation<T>.Failure($"Response body is empty.", ErrorType.Deserialization);
    }
}
