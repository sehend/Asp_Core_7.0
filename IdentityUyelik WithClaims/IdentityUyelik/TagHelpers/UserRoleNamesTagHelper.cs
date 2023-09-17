using IdentityUyelik.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace IdentityUyelik.TagHelpers
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        public string UserId { get; set; }

        private readonly UserManager<AppUser> _userManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            var userRoles = await _userManager.GetRolesAsync(user);

            var strinBuilder = new StringBuilder();

            userRoles.ToList().ForEach(x => { strinBuilder.Append(@$"<span class='badge bg-secondary mx-1'>{x.ToLower()}</span>"); });

            output.Content.SetHtmlContent(strinBuilder.ToString());
        }
    }
}
