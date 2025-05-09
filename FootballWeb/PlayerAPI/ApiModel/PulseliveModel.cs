using System.Text.Json.Serialization;

namespace FootballWeb.PlayerAPI.ApiModel
{
    public class PulseliveModel
    {
        public class AltIdsDto
        {
            [JsonPropertyName("opta")]
            public string opta { get; set; } = string.Empty;
        }

        public class Birth
        {
            [JsonPropertyName("date")]
            public Date? DateOfBirth { get; set; }
        }


        public class PulselivePlayer
        {
            [JsonPropertyName("altIds")]
            public required AltIdsDto AltIds { get; set; }

            [JsonPropertyName("age")]
            public string Age { get; set; } = string.Empty; // Thêm age nếu cần 
            [JsonPropertyName("nationalTeam")]
            public National? NationalTeam { get; set; } // Thêm nationalTeam nếu cần

            [JsonPropertyName("birth")]
            public Birth? Birth { get; set; }

            [JsonPropertyName("info")]
            public InformationPlayer? Infor { get; set; }

            // Thêm name nếu cần
            [JsonPropertyName("name")]
            public Name? Name { get; set; }
        }

        public class Name
        {
            [JsonPropertyName("first")]
            public string First { get; set; } = string.Empty;

            [JsonPropertyName("last")]
            public string Last { get; set; } = string.Empty;
        }

        public class PlayerListResponse
        {
            [JsonPropertyName("content")]
            public List<PulselivePlayer> Content { get; set; } = [];
            // pageInfo nếu cần pagination
            [JsonPropertyName("pageInfo")]
            public PageInfo? PageInfo { get; set; }
        }

        public class PageInfo
        {
            [JsonPropertyName("page")]
            public int Page { get; set; }

            [JsonPropertyName("pageSize")]
            public int PageSize { get; set; }

            [JsonPropertyName("numPages")]
            public int NumPages { get; set; }              // <-- tổng số trang :contentReference[oaicite:0]{index=0}
        }
    }

    public class National
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }

    public class Date
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
    }

    public class InformationPlayer
    {
        [JsonPropertyName("shirtNum")]
        public float? ShirtNum { get; set; }
        [JsonPropertyName("positionInfo")]
        public string Position { get; set; } = "Unknown";
    }
}
