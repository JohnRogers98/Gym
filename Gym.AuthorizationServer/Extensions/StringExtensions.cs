using MongoDB.Bson;

namespace Gym.AuthorizationServer.Extensions
{
    public static class StringExtensions
    {
        public static ObjectId ToObjectId(this String str) => ObjectId.Parse(str);
    }
}
