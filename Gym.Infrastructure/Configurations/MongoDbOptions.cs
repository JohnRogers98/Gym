using System.ComponentModel.DataAnnotations;

namespace Gym.Infrastructure.Configurations
{
    public sealed class MongoDbOptions
    {
        [Required]
        public String ConnectionString { get; set; } = default!;

        [Required]
        public String DatabaseName { get; set; } = default!;

        public CollectionOptions Collections { get; set; } = new();

        public override string ToString()
        {
            return $"MongoDbOptions: ConnectionString={ConnectionString}, DatabaseName={DatabaseName}";
        }
    }

    public sealed class CollectionOptions
    {
        public String Instructors { get; set; } = "instructors";
        public String InstructorProjections { get; set; } = "instructor-projections";
        public String Trainings { get; set; } = "trainings";
        public String TrainingProjections { get; set; } = "training-projections";
        public String CalendarEvents { get; set; } = "calendar-events";
        public String CalendarEventProjections { get; set; } = "calendar-event-projections";
        public String Users { get; set; } = "users";
        public String TelegramAuths { get; set; } = "telegram-auths";
        public String FormAuths { get; set; } = "form-auths";
        public String Clients { get; set; } = "clients";
        public String ClientProjections { get; set; } = "client-projections";
        public String Events { get; set; } = "events";
        public String EventProjections { get; set; } = "event-projections";
        public String Messages { get; set; } = "messages";
        public String OutboxChangeStreams { get; set; } = "outbox-changes-streams";
        public String Polls { get; set; } = "polls";
        public String PollResponses { get; set; } = "poll-responses";
        public String PersonalTrainings { get; set; } = "personal-trainings";
        public String PersonalTrainingProjections { get; set; } = "personal-training-projections";
    }
}
