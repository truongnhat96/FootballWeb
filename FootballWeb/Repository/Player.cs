using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballWeb.Repository
{
    [Table("Player")]
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public required string Name { get; set; }
        [StringLength(250)]
        public required string Position { get; set; } 
        public int ShirtNo { get; set; }
        [StringLength(350)]
        public required string Nationality { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; } 
        public int Age { get; set; }
        [Url]
        public required string ImageUrl { get; set; }
        public int TeamId { get; set; }
        public virtual Team? Team { get; set; }
        public virtual PlayerRecord? PlayerRecord { get; set; }
    }
}
