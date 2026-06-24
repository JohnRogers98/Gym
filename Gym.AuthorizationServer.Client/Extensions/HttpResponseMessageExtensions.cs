namespace System.Net.Http;

public static class HttpResponseMessageExtensions
{
    extension(HttpResponseMessage response)
    {
        public Boolean IsContentTypeJson()
        {
            return response.Content?.Headers?.ContentType?.MediaType?.Equals("application/json", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
   
}
