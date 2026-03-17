using Gym.Domain._Common;
using Gym.Domain._Exceptions;

namespace Gym.Application.Extensions
{
    public static class ResultExtensions
    {
        public static T Unwrap<T>(this Result<T> result)
        {
            if (result.Success is false)
                throw new DomainException(result.Error!);

            return result.Data!;
        }

        public static Result Bind(this Result result, Func<Result> onSuccess)
        {
            if (result.Success is false)
                return result;

            return onSuccess();
        }

        public static Result Bind<T>(this Result<T> result, Func<Result> onSuccess)
        {
            if (result.Success is false)
                return Result.Fail(result.Error!);

            return onSuccess();
        }

        public static Result<T> Bind<T>(this Result result, Func<Result<T>> onSuccess)
        {
            if (result.Success is false)
                return Result<T>.Fail(result.Error!);

            return onSuccess();
        }

        public static Result<T> Bind<T>(this Result<T> result, Func<Result<T>> onSuccess)
        {
            if (result.Success is false)
                return Result<T>.Fail(result.Error!);

            return onSuccess();
        }
    }
}
