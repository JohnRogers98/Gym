using Gym.WebApplication.Features.Account.ViewModels;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Account;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Account.Services
{
    public interface IGetAllAccountHistoryItemsService
    {
        Task<IEnumerable<AccountHistoryItemViewModel>> ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public class GetAllAccountHistoryItemsService(HttpClient _httpClient, AccountHistoryViewModelMapper _accountHistoryViewModelMapper) : IGetAllAccountHistoryItemsService
    {
        public async Task<IEnumerable<AccountHistoryItemViewModel>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/actions/get-history", new GetAccountHistoryRequest(), cancellationToken);
            var listResponse = await response.Content.ReadFromJsonAsync<ListResponse<AccountHistoryDto>>();
            IEnumerable<AccountHistoryDto> dtos = listResponse!.Data;
            return dtos.Select(_accountHistoryViewModelMapper.ToViewModel).ToList();
        }
    }
}
