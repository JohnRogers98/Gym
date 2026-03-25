namespace Gym.Domain.PollContext.ValueObjects
{
    public record Choice
    {
        public ChoiceId Id { get; }

        public ChoiceText Text { get; }

        private Choice(ChoiceId id, ChoiceText text) 
            => (Id, Text) = (id, text);

        public static Choice From(ChoiceId id, ChoiceText text)
        {
            return new(id, text);
        }
    }
}
