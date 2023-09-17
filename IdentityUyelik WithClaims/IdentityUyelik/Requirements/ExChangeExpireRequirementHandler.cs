using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityUyelik.Requirements
{
    public class ExChangeExpireRequirementHandler : AuthorizationHandler<ExChangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExChangeExpireRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "ExChangeExpireDate"))
            {
                context.Fail();

                return Task.CompletedTask;
            }

            Claim exChangeExpireDate = context.User.FindFirst("ExChangeExpireDate");

            if (DateTime.Now > Convert.ToDateTime(exChangeExpireDate.Value))
            {
                context.Fail();

                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;

        }
    }
}
