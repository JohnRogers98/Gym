using AutoMapper;
using Gym.WebApplication.Features.Admin.Instructors.States;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Instructor;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public interface IGetAllInstructorsService
    {
        Task<IEnumerable<InstructorViewModel>> HandleAsync(CancellationToken cancellationToken = default);
    }

    public class CachableGetAllInstructosSetvice : IGetAllInstructorsService
    {
        private readonly IGetAllInstructorsService _decoratee;
        private readonly IInstructorRegisteredSharedState _instructorRegisteredSharedState;

        private IEnumerable<InstructorViewModel>? _cache;

        public CachableGetAllInstructosSetvice(IGetAllInstructorsService decoratee, IInstructorRegisteredSharedState instructorRegisteredSharedState)
        {
            _decoratee = decoratee;
            _instructorRegisteredSharedState = instructorRegisteredSharedState;

            _instructorRegisteredSharedState.InstructorCreated += _ => _cache = null;
        }

        public async Task<IEnumerable<InstructorViewModel>> HandleAsync(CancellationToken cancellationToken = default)
        {
            if (_cache is not null)
            {
                return _cache;
            }

            _cache = [.. await _decoratee.HandleAsync(cancellationToken)];
            return _cache;
        }
    }

    public class GetAllInstructorsService(HttpClient _httpClient, IMapper _mapper) : IGetAllInstructorsService
    {
        public async Task<IEnumerable<InstructorViewModel>> HandleAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<InstructorDto>>("api/instructors", cancellationToken: cancellationToken);
            IEnumerable<InstructorDto> dtos = response!.Data;
            return dtos.Select(_mapper.Map<InstructorViewModel>).ToList();
        }
    }
}
