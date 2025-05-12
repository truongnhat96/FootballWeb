using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballWeb.Repository
{
    [Table("Statistic")]
    public class Statistic
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public required string MatchId { get; set; }
        public int CornerKicks { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int Shots { get; set; }
        public int ShotsOnGoal { get; set; }
        public int BallPossession { get; set; }
        public int Fouls { get; set; }
        public int Offsides { get; set; }
        public int ShotsOffGoal { get; set; }
        public int FreeKicks { get; set; }
        public int Saves { get; set; }
        public int ThrowIns { get; set; }
    }
}
