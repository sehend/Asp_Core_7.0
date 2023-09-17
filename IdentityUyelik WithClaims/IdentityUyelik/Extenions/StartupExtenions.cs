using IdentityUyelik.CustemValidatoins;
using IdentityUyelik.Localizasion;
using IdentityUyelik.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityUyelik.Extenions
{
    public static class StartupExtenions
    {

        public static void AddIDentityWithExtenions(this IServiceCollection services)
        {

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {

                options.User.RequireUniqueEmail = true;

                options.User.AllowedUserNameCharacters = "abcçdefgğhiıjklmnoöprsştuüvyz1234567890_";

                options.Password.RequiredLength = 6;

                options.Password.RequireNonAlphanumeric = false;

                options.Password.RequireLowercase = true;

                options.Password.RequireUppercase = false;

                options.Password.RequireDigit = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

                options.Lockout.MaxFailedAccessAttempts = 3;


            }).AddPasswordValidator<PasswordValidator>()
            .AddUserValidator<UserValidator>()
            .AddErrorDescriber<localizaionIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();
        }

    }
}
