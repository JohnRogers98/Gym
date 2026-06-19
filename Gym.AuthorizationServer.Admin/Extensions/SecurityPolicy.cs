using System.ComponentModel;

namespace Gym.AuthorizationServer.Admin.Extensions
{
    public enum SecurityPolicy
    {
        [Description("Requires authenticated user")]
        AuthenticatedOnly,

        [Description("Requires Admin role")]
        AdminOnly,

        [Description("Requires Client role")]
        ClientOnly,

        [Description("Requires Instructor role")]
        InstructorOnly
    }
}
