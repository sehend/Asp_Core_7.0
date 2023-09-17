using Microsoft.AspNetCore.Authorization;

namespace IdentityUyelik.Requirements
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {

        public int ThresholdAge { get; set; }

    }
}
