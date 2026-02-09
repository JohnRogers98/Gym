using MediatR;

namespace Gym.Application.Services.BookingApi.BookTrainingEvent
{
    public record BookTrainingEvent(String UserId, String CalendarEventId) : IRequest<BookingDetails>;
}
