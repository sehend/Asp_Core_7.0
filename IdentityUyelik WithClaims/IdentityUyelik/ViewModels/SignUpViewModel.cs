using System.ComponentModel.DataAnnotations;

namespace IdentityUyelik.ViewModels
{
    public class SignUpViewModel
    {

        public SignUpViewModel()
        {

        }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;

            Email = email;

            Phone = phone;

            Password = password;
        }


        [Required(ErrorMessage ="Kulanıcı Ad Alanı Boş Bırakılamaz")]
        [Display(Name = "Kulanıcı Adı :")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage ="Email Format Yanlış")]
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon Alanı Boş Bırakılamaz")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz")]
        [Display(Name = "Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz En az 6 Karakter Olabilir")]
        public string Password { get; set; }

        [Compare(nameof(Password),ErrorMessage ="Şifre Aynı Degildir")]
        [Required(ErrorMessage = "Şifre Tekrar Alanı Boş Bırakılamaz")]
        [Display(Name = "Şifre Tekrar :")]
        [MinLength(6, ErrorMessage = "Şifreniz En az 6 Karakter Olabilir")]
        public string PasswordConfirm { get; set; }
    }
}
