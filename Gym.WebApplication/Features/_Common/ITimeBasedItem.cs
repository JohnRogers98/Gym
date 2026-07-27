namespace Gym.WebApplication.Features._Common
{
    public interface ITimeBasedItem
    {
        DateTime Start { get; }
        DateTime? End { get; }

        public TimeSpan Duration => (End ?? Start) - Start;
        public String TimeRange => End.HasValue ? $"{Start:HH:mm} - {End:HH:mm}" : $"{Start:HH:mm}";
    }
}
