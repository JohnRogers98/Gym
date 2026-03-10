namespace Gym.Infrastructure.Entities.EventStores.DtoDeserializers
{
    internal interface IEventDtoDeserializer
    {
        Object Deserialize(EventEntity eventEntity);
        TDto Deserialize<TDto>(EventEntity eventEntity) where TDto: class;
    }
}
