using IdentityUyelik.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityUyelik.ClaimsProvder
{
    public class UserClaimProvders : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;


        public UserClaimProvders(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityUser = principal.Identity as ClaimsIdentity;

            var sehend = identityUser.FindFirst(x => x.Type == ClaimTypes.Email).ToString().Split("/")[8].ToString().Split("emailaddress: ")[1].ToString();

            var curretUser = await _userManager.FindByEmailAsync(sehend);

            if (string.IsNullOrEmpty(curretUser.City))
            {
                return principal;
            }

            if (principal.HasClaim(x => x.Type != "City"))
            {
                Claim cityClaim = new Claim("City", curretUser.City);

                identityUser.AddClaim(cityClaim);
            }

            return principal;
        }
    }
}
