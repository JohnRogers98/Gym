using AutoMapper;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models.Forms;
using Gym.WebApplication.Features.Admin.Clients.TableView.Models.Results;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Clients.TableView.Services
{
    public interface IChargeClientService
    {
        Task<ChargeClientResult> ExecuteAsync(ChargeClientFormModel chargeClientFormModel, CancellationToken cancellationToken = default);
    }

    public class ChargeClientService(HttpClient _httpClient, IMapper _mapper) : IChargeClientService
    {
        public async Task<ChargeClientResult> ExecuteAsync(ChargeClientFormModel chargeClientFormModel, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"/api/clients/{chargeClientFormModel.ClientId}/account/actions/charge",
                new ChargeAccountRequest() { ByCount = chargeClientFormModel.ByCount },
                cancellationToken: cancellationToken
            );

            var chargeAccountResponse = await response.Content.ReadFromJsonAsync<ChargeAccountResponse>();

            return _mapper.Map<ChargeClientResult>(chargeAccountResponse);
        }
    }
}
