namespace Gym.Infrastructure.Configurations
{
    public sealed record MongoDbOptions(String ConnectionString, String DatabaseName, CollectionOptions CollectionOptions)
    {
        public static MongoDbOptions Default => new MongoDbOptions(String.Empty, "test", CollectionOptions.Default);
    }

    public sealed record CollectionOptions(
        String Instructors,
        String InstructorProjections,
        String Trainings,
        String CalendarEvents,
        String CalendarEventProjections,
        String Users,
        String Clients,
        String Events,
        String EventProjections,
        String Messages,
        String OutboxChangeStreams)
    {
        public static CollectionOptions Default => new CollectionOptions(
            "instructors",
            "instructor-projections",
            "trainings",
            "calendar-events",
            "calendar-event-projections",
            "users",
            "clients",
            "events",
            "event-projections",
            "messages",
            "outbox-changes-streams");
    }
}
