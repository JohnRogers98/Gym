using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    public record BookTrainingEventCommand(String userId, String calendarEventId) : IRequest<BookingDetails>;
}
