using IdentityUyelik.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityUyelik.CustemValidatoins
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        Task<IdentityResult> IPasswordValidator<AppUser>.ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {

            //! mutlaka null olmıcak anlamında

            var errors = new List<IdentityError>();

            if (password!.ToLower().Contains(user.UserName!.ToLower())){

                errors.Add(new() { Code = "PasswordContainsUserName", Description = "Şifre Alanı Kulanıcı Adı İçeremez...." });

            }

            if (password!.ToLower().StartsWith("1234"))
            {

                errors.Add(new() { Code = "PasswordContains1234", Description = "Şifre Alanı Ardışık Sayı İçeremez...." });

            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
           
                return Task.FromResult(IdentityResult.Success);
          
        }
    }
}
