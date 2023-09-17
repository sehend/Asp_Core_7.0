using Jwt_Onemli.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jwt_Onemli.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public WeatherForecastController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

    
        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var sehend = await _userManager.Users.ToListAsync();

            return Ok(sehend);
        }
    }
}