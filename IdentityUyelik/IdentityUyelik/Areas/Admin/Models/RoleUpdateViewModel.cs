using System.ComponentModel.DataAnnotations;

namespace IdentityUyelik.Areas.Admin.Models
{
    public class RoleUpdateViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role Ad Alanı Boş Bırakılamaz")]
        [Display(Name = "Role Adı :")]
        public string Name { get; set; }
    }
}
