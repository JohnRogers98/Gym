namespace Gym.Infrastructure.Configurations
{
    public sealed record MongoDbOptions(String ConnectionString, String DatabaseName, CollectionOptions CollectionOptions)
    {
        public static MongoDbOptions Default => new MongoDbOptions(String.Empty, "test", CollectionOptions.Default);

        public override string ToString()
        {
            return $"MongoDbOptions: ConnectionString={ConnectionString}, DatabaseName={DatabaseName}";
        }
    }

    public sealed record CollectionOptions(
        String Instructors,
        String InstructorProjections,
        String Trainings,
        String TrainingProjections,
        String CalendarEvents,
        String CalendarEventProjections,
        String Users,
        String TelegramAuths,
        String FormAuths,
        String Clients,
        String ClientProjections,
        String Events,
        String EventProjections,
        String Messages,
        String OutboxChangeStreams,
        String Polls,
        String PollResponses)
    {
        public static CollectionOptions Default => new CollectionOptions(
            "instructors",
            "instructor-projections",
            "trainings",
            "training-projections",
            "calendar-events",
            "calendar-event-projections",
            "users",              
            "telegram-auths",
            "form-auths",
            "clients",
            "client-projections",
            "events",
            "event-projections",
            "messages",
            "outbox-changes-streams",
            "polls",
            "poll-responses");
    }
}
