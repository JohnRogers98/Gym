using Gym.Domain.InstructorContext;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Instructors
{
    internal class InstructorRepository(IMongoCollection<InstructorEntity> _instructorCollection, MongoUnitOfWork _mongoUnitOfWork) : IInstructorRepository
    {
        public InstructorId NextIdentity() => InstructorId.From(ObjectId.GenerateNewId().ToString());

        public async Task SaveAsync(Instructor instructor, CancellationToken cancellationToken)
        {
            InstructorEntity instructorEntity = instructor.ToEntity();
            
            await _instructorCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eInstructor => eInstructor.Id == instructorEntity.Id,
                instructorEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<Instructor?> GetByIdAsync(InstructorId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _instructorCollection.Find(_mongoUnitOfWork.Session, eInstructor => eInstructor.Id == id.Value.ToObjectId())
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(InstructorId id, CancellationToken cancellationToken) 
            => await _instructorCollection.Find(_mongoUnitOfWork.Session, eInstructor => eInstructor.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);
    }
}
