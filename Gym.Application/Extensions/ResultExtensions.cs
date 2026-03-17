using Gym.Domain._Common;

namespace Gym.Application.Extensions
{
    internal static class ResultExtensions
    {
        public static Result<V> SelectMany<T, U, V>(
            this Result<T> result,
            Func<T, Result<U>> bind,
            Func<T, U, V> project)
        {
            if (result.Success is false)
                return Result<V>.Fail(result.Error!);

            var nextResult = bind(result.Data!);

            if (nextResult.Success is false)
                return Result<V>.Fail(nextResult.Error!);

            var combinedValue = project(result.Data!, nextResult.Data!);

            return Result<V>.Ok(combinedValue);
        }

        public static Result<Unit> AsUnit(this Result result)
        {
            return result.Success
                ? Result<Unit>.Ok(Unit.Instance)
                : Result<Unit>.Fail(result.Error!);
        }

        public record Unit
        {
            public static Unit Instance => new Unit();
        }
    }
}
