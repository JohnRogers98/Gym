using Gym.Infrastructure.Entities.EventStores;

namespace Gym.Infrastructure.Entities.Extensions
{
    internal static class EventExtensions
    {

        public static Boolean IsVersionSequenceCorrect(this IEnumerable<EventEntity> eventEntities, Int32 lastKnownVersion)
        {
            var eventList = eventEntities.ToList();

            if (!eventList.Any())
                return true;
         
            for (Int32 i = 0; i < eventList.Count; i++)
            {
                Int32 expectedVersionForThisEvent = lastKnownVersion + 1 + i;
                if (eventList[i].Version != expectedVersionForThisEvent)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
