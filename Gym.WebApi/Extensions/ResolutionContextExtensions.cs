using AutoMapper;

namespace Gym.WebApi.Extensions
{
    public static class ResolutionContextExtensions
    {
        public static T GetTypedItem<T>(this ResolutionContext resolutionContext, String key)
        {
            return (T)resolutionContext.Items[key];
        }
    }
}
