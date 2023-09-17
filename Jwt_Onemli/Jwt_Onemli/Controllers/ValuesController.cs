using Jwt_Onemli.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jwt_Onemli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public ValuesController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            var sehend = await _userManager.Users.ToListAsync();

            return Ok(sehend);
        }
    }
}
