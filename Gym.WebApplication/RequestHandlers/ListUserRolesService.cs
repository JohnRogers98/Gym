using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Roles;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers;

public class ListUserRolesService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
    : IRequestHandler<ListUserRoles, ListUserRolesResult>
{
    public async Task<AsyncOperation<ListUserRolesResult>> HandleAsync(ListUserRoles request, CancellationToken cancellationToken = default)
    {
        using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

        using var listRolesRequest = this.CreateGetRequest(_bffOptions.Value.ListRolesEndpoint);

        HttpResponseMessage listRolesResponse = await httpClient.SendAsync(listRolesRequest, cancellationToken);
        if (listRolesResponse.IsSuccessStatusCode)
        {
            var deserializedListRolesResponse = await listRolesResponse.Content.ReadFromJsonAsync<ListResponse<UserRoleDto>>();
            if(deserializedListRolesResponse is null)
                return AsyncOperation<ListUserRolesResult>.EmptyResponseBody();

            var userRoles = deserializedListRolesResponse.Data.Select(dtoRole => new UserRole() { Id = dtoRole.Id, Name = dtoRole.Name });
            return AsyncOperation<ListUserRolesResult>.Success(new ListUserRolesResult(userRoles));
        }

        if (listRolesResponse.IsContentTypeProblemDetails())
        {
            return await listRolesResponse.GetFailedOperationFromProblemDetailsAsync<ListUserRolesResult>(cancellationToken);
        }

        return AsyncOperation<ListUserRolesResult>.UnknownResponseType((Int32)listRolesResponse.StatusCode);
    }
}

public record ListUserRoles;

public record ListUserRolesResult(IEnumerable<UserRole> Roles);

public record UserRole
{
    public required String Id { get; init; }
    public required String Name { get; init; }
}
