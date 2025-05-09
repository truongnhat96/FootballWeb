using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballWeb.Repository
{
    [Table("Team")]
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public required string Name { get; set; }
        [StringLength(50)]
        public required string ShortName { get; set; }
        public virtual ICollection<Player>? Players { get; set; }
    }
}
