using Gym.Domain._Common;
using Microsoft.CSharp.RuntimeBinder;
using System.Text.Json;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer : IEventSerializer
    {
        public String Serialize(DomainEvent domainEvent)
        {
            try
            {
                Object dto = this.ToDto((dynamic)domainEvent);
                return JsonSerializer.Serialize(dto, dto.GetType());
            }
            catch (RuntimeBinderException ex)
            {
                throw new InvalidOperationException(
                    $"No ToDto method found for {domainEvent.GetType().Name} in {this.GetType().Name}.", ex);
            }
        }
    }
}
