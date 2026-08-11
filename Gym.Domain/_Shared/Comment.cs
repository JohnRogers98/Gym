using Gym.Domain._Common;

namespace Gym.Domain._Shared
{
    public class Comment
    {
        public String Value { get; }

        private Comment(String value) => Value = value;

        public static Result<Comment> From(String value)
        {
            return Result<Comment>.Ok(new(value));
        }

        public override String ToString() => Value?.ToString() ?? String.Empty;
    }
}
