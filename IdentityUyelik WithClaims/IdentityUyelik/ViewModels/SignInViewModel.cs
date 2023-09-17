using System.ComponentModel.DataAnnotations;

namespace IdentityUyelik.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
                
        }

        [EmailAddress(ErrorMessage = "Email Format Yanlış")]
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz")]
        [Display(Name = "Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz En az 6 Karakter Olabilir")]
        public string Password { get; set; }


        [Display(Name = "Beni Hatırla")]
        public bool RemmemberMe { get; set; }

    }
}
