using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Polls
{
    internal class PollEntity
    {
        public ObjectId Id { get; set; }

        public required String Title { get; set; }

        public Boolean IsRequired { get; set; }

        public Boolean CanAcceptManyChoices { get; set; }

        public required IEnumerable<ChoiceRecord> Choices { get; set; }
    }

    internal class ChoiceRecord
    {
        public required Int32 Id { get; set; }
        public required String Text { get; set; }
    }
}
