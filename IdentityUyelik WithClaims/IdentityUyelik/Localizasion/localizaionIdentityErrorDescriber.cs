using Microsoft.AspNetCore.Identity;

namespace IdentityUyelik.Localizasion
{
    public class localizaionIdentityErrorDescriber : IdentityErrorDescriber
    {

        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"Bu {userName} Daha Önce Başka Bir Kulanıcı Tarafından Alınmıştır...." };
        }


        public override IdentityError DuplicateEmail(string email)
        {
            return new() { Code = "DuplicateUserName", Description = $"Bu {email} Daha Önce Başka Bir Kulanıcı Tarafından Alınmıştır...." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "DuplicateUserName", Description = $"Şifre En Az 6 Karakterli Olmalıdır...." }; ;
        }
    }
}
