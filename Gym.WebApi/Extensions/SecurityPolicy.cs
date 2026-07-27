using System.ComponentModel;

namespace Gym.WebApi.Extensions
{
    public enum SecurityPolicy
    {
        [Description("Requires authenticated user")]
        Authenticated,

        [Description("Requires Admin role")]
        Admin,

        [Description("Requires Client role")]
        Client,

        [Description("Requires Instructor role")]
        Instructor
    }
}
