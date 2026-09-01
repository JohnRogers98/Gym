namespace Gym.BFF.Helpers
{
    public static class UrlHelper
    {
        public static String Combine(String baseUrl, String endpoint)
        {
            if (String.IsNullOrEmpty(baseUrl)) return endpoint;
            if (String.IsNullOrEmpty(endpoint)) return baseUrl;

            baseUrl = baseUrl.TrimEnd('/');
            endpoint = endpoint.TrimStart('/');

            return $"{baseUrl}/{endpoint}";
        }
    }
}
