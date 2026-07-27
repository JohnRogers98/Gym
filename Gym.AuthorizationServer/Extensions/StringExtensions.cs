using MongoDB.Bson;

namespace System
{
    public static class StringExtensions
    {
        public static ObjectId ToObjectId(this String str) => ObjectId.Parse(str);
    }
}
