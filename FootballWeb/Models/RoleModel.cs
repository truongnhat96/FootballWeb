using System.ComponentModel.DataAnnotations;

namespace FootballWeb.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Phải nhập tên role")]
        [Display(Name = "Tên của Role")]
        [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
        public bool IsUpdate { get; set; } = false;
    }
}
