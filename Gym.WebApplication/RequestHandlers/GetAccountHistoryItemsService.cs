using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels.AccountHistory;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Account;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class GetAccountHistoryItemsService(
        IHttpClientFactory _httpClientFactory,
        IOptions<BffOptions> _bffOptions,
        AccountHistoryViewModelMapper _accountHistoryViewModelMapper) : IRequestHandler<GetAccountHistoryItems, IEnumerable<AccountHistoryItemViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<AccountHistoryItemViewModel>>> HandleAsync(GetAccountHistoryItems request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using HttpRequestMessage getAccountHistoryRequest = this.CreatePostRequestWithJson(_bffOptions.Value.GetAccountHistoryEndpoint, new GetAccountHistoryRequest());

            var getAccountHistoryResponse = await httpClient.SendAsync(getAccountHistoryRequest, cancellationToken);
            if (getAccountHistoryResponse.IsSuccessStatusCode)
            {
                var deserializaedResponse = await getAccountHistoryResponse.Content
                    .ReadFromJsonAsync<ListResponse<AccountHistoryDto>>(cancellationToken: cancellationToken);
                
                if (deserializaedResponse is null)
                    return AsyncOperation<IEnumerable<AccountHistoryItemViewModel>>.EmptyResponseBody();

                var historyItems = deserializaedResponse!.Data.Select(_accountHistoryViewModelMapper.ToViewModel).ToList();
                return AsyncOperation<IEnumerable<AccountHistoryItemViewModel>>.Success(historyItems);
            }

            if (getAccountHistoryResponse.IsContentTypeProblemDetails())
            {
                return await getAccountHistoryResponse.GetFailedOperationFromProblemDetailsAsync<IEnumerable<AccountHistoryItemViewModel>>(cancellationToken);
            }

            return AsyncOperation<IEnumerable<AccountHistoryItemViewModel>>.UnknownResponseType((Int32)getAccountHistoryResponse.StatusCode);
        }
    }

    public class GetAccountHistoryItems;
}
