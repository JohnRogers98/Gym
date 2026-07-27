namespace Gym.AuthorizationServer.Infrastructure.Entities.UserConsents
{
    public class ScopeInfoComparer : IEqualityComparer<ScopeInfo>
    {
        public static ScopeInfoComparer Instance { get; } = new();

        public Boolean Equals(ScopeInfo? x, ScopeInfo? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.Id == y.Id;
        }

        public Int32 GetHashCode(ScopeInfo obj) => obj.Id.GetHashCode();
    }
}
