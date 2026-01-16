using Gym.Domain.InstructorAggregate;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Instructors
{
    internal class InstructorRepository(IMongoCollection<InstructorEntity> _instructorCollection, MongoUnitOfWork _mongoUnitOfWork) : IInstructorRepository, IInstructorQueryService
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

        public async Task<IEnumerable<Instructor>> GetAllAsync(CancellationToken cancellationToken)
        {
            List<Instructor> allInstructors = new();

            await _instructorCollection.Find(_mongoUnitOfWork.Session, Builders<InstructorEntity>.Filter.Empty)
                .ForEachAsync(eInstructor => allInstructors.Add(eInstructor.ToDomain()));

            return allInstructors;
        }

        public async Task<Boolean> ExistsAsync(InstructorId id, CancellationToken cancellationToken) 
            => await _instructorCollection.Find(_mongoUnitOfWork.Session, eInstructor => eInstructor.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);
    }
}
