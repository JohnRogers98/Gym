namespace Gym.AuthorizationServer.Abstractions;


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

#nullable restore warnings

