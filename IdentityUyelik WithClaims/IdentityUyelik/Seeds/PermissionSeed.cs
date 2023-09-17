using IdentityUyelik.Models;

using IdentityUyelik.PermissionsRoot;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityUyelik.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");

            var hasAdvencedRole = await roleManager.RoleExistsAsync("AdvencedRole");

            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");

            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "BasicRole" });

                var basicRole = roleManager.FindByNameAsync("BasicRole");

                await AddReadPermission(roleManager,await basicRole);

            }  
            
            if (!hasAdvencedRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdvencedRole" });

                var basicRole = roleManager.FindByNameAsync("AdvencedRole");

                await AddReadPermission(roleManager,await basicRole);

                await AddUpdateAndCreatePermission(roleManager,await basicRole);

            } 
            
            if (!hasAdminRole)
            {
                await roleManager.CreateAsync(new AppRole() { Name = "AdminRole" });

                var basicRole = roleManager.FindByNameAsync("AdminRole");

                await AddReadPermission(roleManager,await basicRole);

                await AddUpdateAndCreatePermission(roleManager,await basicRole);

                await AddDeletePermission(roleManager,await basicRole);

            }
        }

        public static async Task AddReadPermission(RoleManager<AppRole> roleManager, AppRole role)
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Stock.Read));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Order.Read));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Catalog.Read));
        }   
        
        public static async Task AddUpdateAndCreatePermission(RoleManager<AppRole> roleManager, AppRole role)
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Stock.Update));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Order.Update));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Catalog.Update));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Stock.Create));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Order.Create));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Catalog.Create));
        }

        public static async Task AddDeletePermission(RoleManager<AppRole> roleManager, AppRole role)
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Stock.Delete));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Order.Delete));

            await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Catalog.Delete));

        }
    }
}
