using AutoMapper;
using Gym.Abstractions.Query.EventStore;
using Gym.Application.Services.AccountApi.ChargeAccount;
using Gym.Application.Services.AccountApi.GetAccountHistory;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;

namespace Gym.WebApi.Controllers.Api.Accounts
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings() 
        {
            CreateMap<ChargeAccountRequest, ChargeAccount>()
                 .ConstructUsing((src, context) =>
                 {
                     return new ChargeAccount(context.GetTypedItem<String>(nameof(ChargeAccount.ClientId)), src.ByCount);
                 });

            CreateMap<ChargeAccountResult, ChargeAccountResponse>();

            CreateMap<GetAccountHistoryRequest, GetAccountHistory>()
                 .ConstructUsing((src, context) =>
                 {
                     return new GetAccountHistory(context.GetTypedItem<String>(nameof(GetAccountHistory.ClientId)));
                 });

            CreateMap<EventProjection, AccountHistoryDto>();
        }
    }
}
