using System.Text.Json.Serialization;

namespace FootballWeb.PlayerAPI.ApiModel
{
    public class PlayerStatsResponse
    {
        [JsonPropertyName("history")]
        public List<PlayerRecord> Records { get; set; } = [];
    }

    public class PlayerRecord
    {
        [JsonPropertyName("element")]
        public int Id { get; set; }
        [JsonPropertyName("total_points")]
        public int TotalPoints { get; set; }
        [JsonPropertyName("minutes")]
        public int Minutes { get; set; }
        [JsonPropertyName("goals_scored")]
        public int GoalsScored { get; set; }
        [JsonPropertyName("assists")]
        public int Assists { get; set; }
        [JsonPropertyName("clean_sheets")]
        public int CleanSheets { get; set; }
        [JsonPropertyName("goals_conceded")]
        public int GoalsConceded { get; set; }
        [JsonPropertyName("own_goals")]
        public int OwnGoals { get; set; }
        [JsonPropertyName("penalties_saved")]
        public int PenaltiesSaved { get; set; }
        [JsonPropertyName("penalties_missed")]
        public int PenaltiesMissed { get; set; }
        [JsonPropertyName("yellow_cards")]
        public int YellowCards { get; set; }
        [JsonPropertyName("red_cards")]
        public int RedCards { get; set; }
        [JsonPropertyName("saves")]
        public int Saves { get; set; }
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}
