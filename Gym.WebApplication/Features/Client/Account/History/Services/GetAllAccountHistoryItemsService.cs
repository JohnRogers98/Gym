using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Account.History.ViewModels;
using Gym.WebApplication.Features.Client.Account.History.Models;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Account;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Account.History.Services
{
    public class GetAllAccountHistoryItemsService(HttpClient _httpClient, AccountHistoryViewModelMapper _accountHistoryViewModelMapper) 
        : IRequestHandler<GetAllAccountHistoryItems, IEnumerable<AccountHistoryItemViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<AccountHistoryItemViewModel>>> HandleAsync(GetAllAccountHistoryItems request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/actions/get-history", new GetAccountHistoryRequest(), cancellationToken);
            
            var listResponse = await response.Content.ReadFromJsonAsync<ListResponse<AccountHistoryDto>>();
            var items = listResponse!.Data.Select(_accountHistoryViewModelMapper.ToViewModel).ToList();
            
            return AsyncOperation<IEnumerable<AccountHistoryItemViewModel>>.Success(items);
        }
    }
}
