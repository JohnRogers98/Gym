using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models.Forms;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models.Results;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Clients.TableView.Services
{
    public class ChargeClientService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<ChargeClientFormModel, ChargeClientResult>
    {
        public async Task<AsyncOperation<ChargeClientResult>> HandleAsync(ChargeClientFormModel request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"/api/clients/{request.ClientId}/account/actions/charge",
                new ChargeAccountRequest() { ByCount = request.ByCount },
                cancellationToken: cancellationToken
            );

            if (response.IsSuccessStatusCode)
            {
                var chargeAccountResponse = await response.Content.ReadFromJsonAsync<ChargeAccountResponse>();

                return AsyncOperation<ChargeClientResult>
                    .Success(_mapper.Map<ChargeClientResult>(chargeAccountResponse));
            }

            if (response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<ChargeClientResult>();

            return AsyncOperation<ChargeClientResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
