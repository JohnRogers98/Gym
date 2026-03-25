using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.Infrastructure.Entities.Repositories.PollResponses
{
    internal class PollResponseEntity
    {
        [BsonId]
        public required String Id { get; set; }

        public ObjectId UserId { get; set; }

        public ObjectId PollId { get; set; }

        public required IEnumerable<Int32> ChoiceIds { get; set; }
    }
}
