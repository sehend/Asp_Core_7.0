using System.ComponentModel.DataAnnotations;

namespace IdentityUyelik.ViewModels
{
    public class ForgetPasswordViewModel
    {

        [EmailAddress(ErrorMessage = "Email Format Yanlış")]
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz")]
        [Display(Name = "Şifre :")]
        public string Password { get; set; }

    }
}
