using Gym.WebApplication.Operations;
using System.Net.Http.Json;

namespace Gym.WebApplication.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static Boolean IsContentTypeProblemDetails(this HttpResponseMessage response)
        {
            return response.Content?.Headers?.ContentType?.MediaType?.Equals("application/problem+json", StringComparison.OrdinalIgnoreCase) == true;
        }

        public static async Task<AsyncOperation<T>> GetFailedOperationFromProblemDetailsAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: cancellationToken);
            if(problemDetails is null)
                throw new InvalidDataException("The response content could not be deserialized into ProblemDetails.");

            return response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => AsyncOperation<T>.Failure($"{problemDetails.Detail}", ErrorType.Validation, problemDetails.Status),

                System.Net.HttpStatusCode.NotFound => AsyncOperation<T>.Failure($"{problemDetails.Detail}", ErrorType.NotFound, problemDetails.Status),

                System.Net.HttpStatusCode.Conflict => AsyncOperation<T>.Failure($"{problemDetails.Detail}", ErrorType.Conflict, problemDetails.Status),

                System.Net.HttpStatusCode.InternalServerError => AsyncOperation<T>.Failure($"{problemDetails.Detail}", ErrorType.ServerError, problemDetails.Status),

                _ => AsyncOperation<T>.Failure($"{problemDetails.Detail}", ErrorType.Unknown, problemDetails.Status)
            };
        }
    }
}
