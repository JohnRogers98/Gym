namespace Gym.Infrastructure.Configurations
{
    public sealed record MongoDbOptions(String ConnectionString, String DatabaseName, CollectionOptions CollectionOptions)
    {
        public static MongoDbOptions Default => new MongoDbOptions(String.Empty, "test", CollectionOptions.Default);
    }

    public sealed record CollectionOptions(String Instructors, String Trainings, String CalendarEvents, String Users)
    {
        public static CollectionOptions Default => new CollectionOptions("instructors", "trainings", "calendar-events", "users");
    }
}
