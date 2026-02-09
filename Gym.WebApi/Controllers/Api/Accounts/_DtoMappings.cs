using AutoMapper;
using Gym.Application.Services.AccountApi;
using Gym.Application.Services.AccountApi.ChargeAccount;
using Gym.WebDto.Requests.Account;
using Gym.WebDto.Responses.Account;

namespace Gym.WebApi.Controllers.Api.Accounts
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings() 
        {
            CreateMap<ChargeAccountRequest, ChargeAccount>();
            CreateMap<AccountDetails, ChargeAccountResponse>();
        }
    }
}
