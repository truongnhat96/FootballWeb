using System.Text.Json.Serialization;
using System.Text.Json;

namespace FootballWeb.PlayerAPI.ApiModel
{
    public class Root
    {
        [JsonPropertyName("hits")]
        public Hits? Hits { get; set; }

        [JsonPropertyName("facets")]
        public JsonElement Facets { get; set; }
    }

    public class Hits
    {
        [JsonPropertyName("cursor")]
        public JsonElement Cursor { get; set; }

        [JsonPropertyName("found")]
        public int Found { get; set; }

        [JsonPropertyName("hit")]
        public List<Hit> Hit { get; set; } = [];
    }

    public class Hit
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; } = string.Empty;

        [JsonPropertyName("highlights")]
        public JsonElement Highlights { get; set; }

        [JsonPropertyName("response")]
        public Person? Response { get; set; }
    }

    public class Person
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("altIds")]
        public AltIds? AltIds { get; set; }

        [JsonPropertyName("metadata")]
        public JsonElement Metadata { get; set; }

        [JsonPropertyName("birth")]
        public Birth? Birth { get; set; }

        [JsonPropertyName("age")]
        public string Age { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public Name? Name { get; set; }

        [JsonPropertyName("playerId")]
        public int PlayerId { get; set; }

        [JsonPropertyName("info")]
        public Info? Info { get; set; }

        [JsonPropertyName("debut")]
        public DateLabel? Debut { get; set; }

        [JsonPropertyName("nationalTeam")]
        public Country? NationalTeam { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("currentTeam")]
        public Team? CurrentTeam { get; set; }

        [JsonPropertyName("previousTeam")]
        public Team? PreviousTeam { get; set; }

        [JsonPropertyName("weight")]
        public int? Weight { get; set; }

        [JsonPropertyName("latestPosition")]
        public string LatestPosition { get; set; } = string.Empty;

        [JsonPropertyName("appearances")]
        public int? Appearances { get; set; }

        [JsonPropertyName("goals")]
        public JsonElement Goals { get; set; }

        [JsonPropertyName("assists")]
        public JsonElement Assists { get; set; }

        [JsonPropertyName("tackles")]
        public JsonElement Tackles { get; set; }

        [JsonPropertyName("shots")]
        public JsonElement Shots { get; set; }

        [JsonPropertyName("keyPasses")]
        public JsonElement KeyPasses { get; set; }

        [JsonPropertyName("cleanSheets")]
        public int? CleanSheets { get; set; }

        [JsonPropertyName("saves")]
        public int? Saves { get; set; }

        [JsonPropertyName("goalsConceded")]
        public int? GoalsConceded { get; set; }

        [JsonPropertyName("awards")]
        public JsonElement Awards { get; set; }

        [JsonPropertyName("joinDate")]
        public DateLabel? JoinDate { get; set; }

        [JsonPropertyName("leaveDate")]
        public DateLabel? LeaveDate { get; set; }

        [JsonPropertyName("teamHistory")]
        public JsonElement TeamHistory { get; set; }

        [JsonPropertyName("active")]
        public bool? Active { get; set; }
    }

    public class AltIds
    {
        [JsonPropertyName("opta")]
        public string Opta { get; set; } = string.Empty;
    }

    public class Birth
    {
        [JsonPropertyName("date")]
        public DateLabel? Date { get; set; }

        [JsonPropertyName("country")]
        public Country? Country { get; set; }

        [JsonPropertyName("place")]
        public string Place { get; set; } = string.Empty;
    }

    public class DateLabel
    {
        [JsonPropertyName("millis")]
        public long Millis { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;
    }

    public class Country
    {
        [JsonPropertyName("isoCode")]
        public string IsoCode { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("demonym")]
        public string Demonym { get; set; } = string.Empty;
    }

    public class Name
    {
        [JsonPropertyName("display")]
        public string Display { get; set; } = string.Empty;

        [JsonPropertyName("first")]
        public string First { get; set; } = string.Empty;

        [JsonPropertyName("middle")]
        public string Middle { get; set; } = string.Empty;

        [JsonPropertyName("last")]
        public string Last { get; set; } = string.Empty;
    }

    public class Info
    {
        [JsonPropertyName("position")]
        public string Position { get; set; } = string.Empty;

        [JsonPropertyName("shirtNum")]
        public int? ShirtNum { get; set; }

        [JsonPropertyName("positionInfo")]
        public string PositionInfo { get; set; } = string.Empty;

        [JsonPropertyName("loan")]
        public bool Loan { get; set; }
    }

    public class Team
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("altIds")]
        public AltIds? AltIds { get; set; }

        [JsonPropertyName("metadata")]
        public JsonElement Metadata { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("club")]
        public Club? Club { get; set; }

        [JsonPropertyName("teamType")]
        public string TeamType { get; set; } = string.Empty;

        [JsonPropertyName("grounds")]
        public JsonElement Grounds { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; } = string.Empty;

        [JsonPropertyName("awards")]
        public JsonElement Awards { get; set; }

        [JsonPropertyName("source")]
        public JsonElement Source { get; set; }
    }

    public class Club
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("altIds")]
        public AltIds? AltIds { get; set; }

        [JsonPropertyName("metadata")]
        public JsonElement Metadata { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; } = string.Empty;

        [JsonPropertyName("abbr")]
        public string Abbr { get; set; } = string.Empty;

        [JsonPropertyName("shortAbbr")]
        public string ShortAbbr { get; set; } = string.Empty;

        [JsonPropertyName("founded")]
        public JsonElement Founded { get; set; }

        [JsonPropertyName("country")]
        public JsonElement Country { get; set; }

        [JsonPropertyName("city")]
        public JsonElement City { get; set; }

        [JsonPropertyName("postalCode")]
        public JsonElement PostalCode { get; set; }

        [JsonPropertyName("source")]
        public JsonElement Source { get; set; }

        [JsonPropertyName("teams")]
        public JsonElement Teams { get; set; }
    }
}
