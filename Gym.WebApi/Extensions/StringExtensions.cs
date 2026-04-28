using System.Text.RegularExpressions;

namespace Gym.WebApi.Extensions
{
    public static class StringExtensions
    {
        public static String ToKebabLower(this String str) 
            => Regex.Replace(str, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
