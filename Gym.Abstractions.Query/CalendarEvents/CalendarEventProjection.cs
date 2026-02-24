namespace Gym.Abstractions.Query.CalendarEvents
{
    public record CalendarEventProjection(
        String Id,
        DateTime Start,
        DateTime? End,
        String Status,
        TrainingInfo Training,
        Int32? MaxClientCount,
        IEnumerable<BookingUserInfo>? BookingUsers,
        IEnumerable<InstructorInfo>? Instructors
        );

    public record TrainingInfo(String Id, String Name, String? Description);
    
    public record InstructorInfo(String Id, String FullName);

    public record BookingUserInfo(String Id);
}
