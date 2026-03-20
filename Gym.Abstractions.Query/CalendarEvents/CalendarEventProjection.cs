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
        IEnumerable<InstructorInfo>? Instructors,
        PollInfo? PollInfo = null
        );

    public record TrainingInfo(String Id, String Name, String? Description);
    
    public record InstructorInfo(String Id, String FullName);

    public record PollInfo(String Id, String Title, List<ChoiceInfo> Choices);

    public record ChoiceInfo(Int32 Id, String Text, Int32 VoteCount = 0);

    public record BookingUserInfo(String Id);
}
