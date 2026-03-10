using System.Text.Json;

namespace Gym.WebApplication.Extensions
{
    public static class DictionaryExtensions
    {
        public static T GetRequiredValue<T>(this Dictionary<String, Object> dictionary, String key)
        {
            if (dictionary.TryGetValue(key, out Object? value) is false)
                throw new KeyNotFoundException($"Key '{key}' not found.");

            return value switch
            {
                T typedValue => typedValue,

                JsonElement element => element.Deserialize<T>(),

                _ => (T)Convert.ChangeType(value, typeof(T))
            } ?? throw new InvalidOperationException($"Cannot convert {key} to {typeof(T)}");
        }
    }
}
