using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Extensions
{
    public static class StringExtensions
    {
        public static ObjectId ToObjectId(this String str) => ObjectId.Parse(str);
    }
}
