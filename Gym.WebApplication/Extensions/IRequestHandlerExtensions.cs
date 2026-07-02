using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Gym.WebApplication.Features._Common.Services;

public static class IRequestHandlerExtensions
{
    extension<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> requestHandler)
    {
        public HttpRequestMessage CreatePostRequestWithJson<T>(String url, T bodyObject, Boolean setCookie = true)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            if(setCookie)
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var bodyJson = JsonSerializer.Serialize(bodyObject);
            request.Content = new StringContent(bodyJson, Encoding.UTF8, MediaTypeNames.Application.Json);

            return request;
        }

        public HttpRequestMessage CreateGetRequest(String url, Boolean setCookie = true)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            if (setCookie)
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return request;
        }
    }
}
