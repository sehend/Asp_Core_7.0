using System.ComponentModel.DataAnnotations;

namespace IdentityUyelik.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz")]
        [Display(Name = "Yeni Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz En az 6 Karakter Olabilir")]
        public string PasswordOld { get; set; }

        [Required(ErrorMessage = "Yeni Şifre Alanı Boş Bırakılamaz")]
        [Display(Name = "Yeni Şifre :")]
        public string PasswordNew { get; set; }

        [Compare(nameof(PasswordNew), ErrorMessage = "Şifre Aynı Degildir")]
        [Required(ErrorMessage = "Yeni Şifre Tekrar Alanı Boş Bırakılamaz")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        public string PasswordNewConfirm { get; set; }
    }
}
