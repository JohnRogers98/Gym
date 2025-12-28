using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Extensions
{
    internal static class StringExtensions
    {
        public static ObjectId ToObjectId(this String str) => ObjectId.Parse(str);
    }
}
