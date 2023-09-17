using Jwt_Onemli.Data;
using Jwt_Onemli.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jwt_Onemli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuntController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AuntController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);

            if (user != null == await _userManager.CheckPasswordAsync(user!, loginModel.Password))
            {
                var userRole = await _userManager.GetRolesAsync(user!);

                var newClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user!.UserName!),

                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),

                };

                foreach (var item in userRole)
                {
                    newClaims.Add(new Claim(ClaimTypes.Role, item));
                }

                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SehendSinaJwtToken"));

                var token = new JwtSecurityToken(

                    issuer: "https://localhost:7004",

                    audience: "https://localhost:7004",

                    expires: DateTime.UtcNow.AddDays(20),

                    claims: newClaims,

                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)

                    );

                var clearToken = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(clearToken);


            }
            else
            {
                return BadRequest();
            }
        }
    }

}

