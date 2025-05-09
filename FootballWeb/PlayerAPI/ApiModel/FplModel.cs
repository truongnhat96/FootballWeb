using System.Text.Json.Serialization;

namespace FootballWeb.PlayerAPI.ApiModel
{
    public class FplBootstrap
    {
        [JsonPropertyName("current_event")]
        public int? CurrentEvent { get; set; }
        [JsonPropertyName("elements")]
        public List<FplPlayer> elements { get; set; } = [];

        [JsonPropertyName("element_types")]
        public List<FplElementType> element_types { get; set; } = [];

        // Nếu cần teams
        [JsonPropertyName("teams")]
        public List<FplTeam> teams { get; set; } = [];
    }

    public class FplPlayer
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("first_name")]
        public string first_name { get; set; } = string.Empty;

        [JsonPropertyName("second_name")]
        public string second_name { get; set; } = string.Empty;

        [JsonPropertyName("photo")]
        public string photo { get; set; } = string.Empty;

        [JsonPropertyName("team")]
        public int team { get; set; }

        [JsonPropertyName("element_type")]
        public int element_type { get; set; }

        // squad_number hiện không dùng (null)
        [JsonPropertyName("squad_number")]
        public int? squad_number { get; set; }
    }

    public class FplElementType
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("singular_name")]
        public string singular_name { get; set; } = string.Empty;

        [JsonPropertyName("plural_name")]
        public string plural_name { get; set; } = string.Empty;
    }

    public class FplTeam
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; } = string.Empty;

        [JsonPropertyName("short_name")]
        public string short_name { get; set; } = string.Empty;
    }
}
