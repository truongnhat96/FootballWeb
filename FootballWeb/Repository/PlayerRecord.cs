using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballWeb.Repository
{
    [Table("PlayerRecord")]
    public class PlayerRecord
    {
        [Key]
        public int Id { get; set; }
        public int Appearances { get; set; }
        public int TotalPoints { get; set; }
        public int Minutes { get; set; }
        public int GoalsScored { get; set; }
        public int Assists { get; set; }
        public int CleanSheets { get; set; }
        public int GoalsConceded { get; set; }
        public int OwnGoals { get; set; }
        public int PenaltiesSaved { get; set; }
        public int PenaltiesMissed { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int Saves { get; set; }
        public int Value { get; set; }
        public virtual Player? Player { get; set; }
    }
}
