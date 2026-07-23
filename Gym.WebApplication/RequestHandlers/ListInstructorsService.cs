using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Instructor;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class ListInstructorsService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<ListInstructors, IEnumerable<InstructorViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<InstructorViewModel>>> HandleAsync(ListInstructors request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var listInstructorsRequest = this.CreateGetRequest(_bffOptions.Value.ListInstructorsEndpoint);

            HttpResponseMessage listInstructorsResponse = await httpClient.SendAsync(listInstructorsRequest, cancellationToken);
            if (listInstructorsResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await listInstructorsResponse.Content.ReadFromJsonAsync<ListResponse<InstructorDto>>();
                if (deserializedResponse is null)
                    return AsyncOperation<IEnumerable<InstructorViewModel>>.EmptyResponseBody();

                var instructors = deserializedResponse.Data.Select(_mapper.Map<InstructorViewModel>);
                return AsyncOperation<IEnumerable<InstructorViewModel>>.Success(instructors);
            }

            if (listInstructorsResponse.IsContentTypeProblemDetails())
            {
                return await listInstructorsResponse.GetFailedOperationFromProblemDetailsAsync<IEnumerable<InstructorViewModel>>(cancellationToken);
            }

            return AsyncOperation<IEnumerable<InstructorViewModel>>.UnknownResponseType((Int32)listInstructorsResponse.StatusCode);
        }
    }

    public class ListInstructors;
}
