namespace System
{
    public static class StringExtensions
    {
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
