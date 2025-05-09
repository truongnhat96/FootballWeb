using System.ComponentModel.DataAnnotations;

namespace FootballWeb.Models
{
    public class TeamModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên đội bóng")]
        public String Name { get; set; }
       
    }
}
