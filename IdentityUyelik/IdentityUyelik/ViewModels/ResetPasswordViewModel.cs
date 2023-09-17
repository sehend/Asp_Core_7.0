using System.ComponentModel.DataAnnotations;

namespace IdentityUyelik.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz")]
        [Display(Name = "Yeni Şifre :")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifre Aynı Degildir")]
        [Required(ErrorMessage = "Şifre Tekrar Alanı Boş Bırakılamaz")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        public string PasswordConfirm { get; set; }
    }
}
