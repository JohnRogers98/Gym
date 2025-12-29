using System.ComponentModel;

namespace Gym.WebApi.Extensions
{
    public enum SecurityPolicy
    {
        [Description("Requires authenticated user")]
        RequireAuthenticated,

        [Description("Requires Admin role")]
        AdminOnly,

        [Description("Requires Client role")]
        ClientOnly
    }
}
