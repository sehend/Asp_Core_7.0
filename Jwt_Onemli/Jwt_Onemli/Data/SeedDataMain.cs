using Microsoft.AspNetCore.Identity;
using System;

namespace Jwt_Onemli.Data
{
    public class SeedDataMain
    {

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<Jwt_Onemli.Data.DbContext>();

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var roleManeger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();




            //VeriTabanı Yok İse VeriTabanını Olusturur....

            dbContext.Database.EnsureCreated();

            AppUser appUser = new AppUser()
            {

                UserName = "sehend",

                Email = "sehendsina@gmail.com",

                SecurityStamp = Guid.NewGuid().ToString(),

            };


            if (!dbContext.Users.Any())
            {


                await userManager.CreateAsync(appUser, "Sehend0219.");







            }

            var hasAdmin = await roleManeger.FindByNameAsync("Admin");

            var hasEditör = await roleManeger.FindByNameAsync("Editör");

            if (hasAdmin == null)
            {
                await roleManeger.CreateAsync(new IdentityRole() { Name = "Admin" });

            }

            if (hasEditör == null)
            {
                await roleManeger.CreateAsync(new IdentityRole() { Name = "Editör" });

            }

            await userManager.AddToRoleAsync(appUser, "Admin");

        }

    }
}
