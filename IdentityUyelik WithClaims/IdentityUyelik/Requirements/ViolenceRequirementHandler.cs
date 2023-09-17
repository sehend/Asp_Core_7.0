using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityUyelik.Requirements
{
    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "BirthDay"))
            {
                context.Fail();

                return Task.CompletedTask;
            }

            Claim birthDayDateCalim = context.User.FindFirst("BirthDay");

            var toDay = DateTime.Now;

            var birthDayDate = Convert.ToDateTime(birthDayDateCalim.Value);

            var age = toDay.Year - birthDayDate.Year;

            if (birthDayDate > toDay.AddYears(-age))
            {
                age--;
            }

            if (toDay.Year == (age + 1))
            {
                context.Fail();

                return Task.CompletedTask;
            }

            if (requirement.ThresholdAge > age)
            {
                context.Fail();

                return Task.CompletedTask;
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
