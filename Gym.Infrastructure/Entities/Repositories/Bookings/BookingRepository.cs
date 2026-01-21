using Gym.Domain.BookingAggregate;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Bookings
{
    internal class BookingRepository(IMongoCollection<BookingEntity> _bookingCollection, MongoUnitOfWork _mongoUnitOfWork) 
        : IBookingRepository, IBookingQueryService
    {
        public BookingId NextIdentity() => BookingId.From(ObjectId.GenerateNewId().ToString());

        public async Task<Boolean> ExistsAsync(BookingId id, CancellationToken cancellationToken) 
            => await _bookingCollection.Find(_mongoUnitOfWork.Session, eBooking => eBooking.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);

        public async Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _bookingCollection.Find(_mongoUnitOfWork.Session, eBooking => eBooking.Id == id.Value.ToObjectId())
             .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task SaveAsync(Booking booking, CancellationToken cancellationToken)
        {
            BookingEntity bookingEntity = booking.ToEntity();

            await _bookingCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eBooking => eBooking.Id == bookingEntity.Id,
                bookingEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }
    }
}
