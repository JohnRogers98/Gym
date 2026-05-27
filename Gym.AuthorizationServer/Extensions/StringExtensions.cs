using MongoDB.Bson;

namespace System
{
    public static class StringExtensions
    {
        public static ObjectId ToObjectId(this String str) => ObjectId.Parse(str);

        public static String ToUrlSafe(this String str)
        {
            String urlSafeBase64 = str
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            return urlSafeBase64;
        }
    }
}
