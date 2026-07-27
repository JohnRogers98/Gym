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

        public static Byte[] Base64UrlDecode(this String str)
        {
            String base64 = str.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
