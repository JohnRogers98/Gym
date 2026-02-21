using Gym.WebApplication.Features.Account.ViewModels;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Account;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Account.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountHistoryItemViewModel>> GetAllAccountHistoryItemsAsync();
    }

    public class AccountService(HttpClient _httpClient, AccountHistoryViewModelMapper _accountHistoryViewModelMapper) : IAccountService
    {
        public async Task<IEnumerable<AccountHistoryItemViewModel>> GetAllAccountHistoryItemsAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/actions/get-history", new GetAccountHistoryRequest());
            Console.WriteLine("2");
            var listResponse = await response.Content.ReadFromJsonAsync<ListResponse<AccountHistoryDto>>();
            IEnumerable<AccountHistoryDto> dtos = listResponse!.Data;
            Console.WriteLine("1");
            foreach(var a in dtos)
            {
                foreach (var item in a.Payload)
                {
                    Console.WriteLine(item.Key +  "         "  +  item.Value.GetType());
                }
                Console.WriteLine(a.ToString()); 
            }
            return dtos.Select(_accountHistoryViewModelMapper.ToViewModel).ToList();
        }
    }

}
