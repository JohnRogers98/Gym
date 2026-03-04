namespace Gym.Domain.UserContext.Authentication
{
    public record ValidatedTelegramUserInfo
    {
        public TelegramId Id { get; init; }
        public TelegramUsername? Username { get; init; }
        public FirstName? FirstName { get; init; }
        public LastName? LastName { get; init; }
        
        private ValidatedTelegramUserInfo(TelegramId id, TelegramUsername? username, FirstName? firstName, LastName? lastName)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
        }

        public static ValidatedTelegramUserInfo From(TelegramId id, TelegramUsername? username = null, FirstName? firstName = null, LastName? lastName = null) 
            => new (id, username, firstName, lastName);
    }
}
