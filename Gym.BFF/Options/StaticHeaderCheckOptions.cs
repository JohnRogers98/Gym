namespace Gym.BFF.Options
{
    public class StaticHeaderCheckOptions
    {
        public const String SectionName = "StaticHeaderCheck";

        public HashSet<String> ExcludedPaths { get; set; } = new();

        public Boolean Enabled { get; set; } = true;

        public HashSet<PathString> GetExcludedPathStrings()
        {
            return ExcludedPaths
                .Select(s => new PathString(s))
                .ToHashSet();
        }
    }
}
