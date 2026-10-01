namespace Gym.BFF.Services
{
    public interface IIdempotencyKeyValidator
    {
        Boolean IsValid(String idempotencyKey);
    }

    public class IdempotencyKeyValidator : IIdempotencyKeyValidator
    {
        public Boolean IsValid(String idempotencyKey) 
            => Guid.TryParse(idempotencyKey, out _);
    }
}
