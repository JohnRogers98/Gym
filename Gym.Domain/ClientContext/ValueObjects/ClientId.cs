using Gym.Domain._Common;
using Gym.Domain.ClientContext.Errors;

namespace Gym.Domain.ClientContext.ValueObjects
{
    public class ClientId
    {
        public String Value { get; }

        private ClientId(String value) => Value = value;

        public static Result<ClientId> From(String value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Result<ClientId>.Fail(ClientIdValidationError.Create());
            }  

            return Result<ClientId>.Ok(new(value));
        }

        public override String ToString() => Value.ToString();
    }
}
