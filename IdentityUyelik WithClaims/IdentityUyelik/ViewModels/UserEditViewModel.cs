using IdentityUyelik.Models;
using System.ComponentModel.DataAnnotations;

namespace IdentityUyelik.ViewModels
{
    public class UserEditViewModel
    {

        [Required(ErrorMessage = "Kulanıcı Ad Alanı Boş Bırakılamaz")]
        [Display(Name = "Kulanıcı Adı :")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email Format Yanlış")]
        [Required(ErrorMessage = "Email Alanı Boş Bırakılamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon Alanı Boş Bırakılamaz")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Dogum :")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "Şehir :")]
        public string City { get; set; }
        
        [Display(Name = "Profil Resim :")]
        public IFormFile Picture { get; set; }

        [Display(Name = "Cinsiyet :")]
        public Gender Gender { get; set; }

    }
}
