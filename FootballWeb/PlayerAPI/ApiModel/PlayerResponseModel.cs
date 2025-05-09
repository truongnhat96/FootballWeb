namespace FootballWeb.PlayerAPI.ApiModel
{
    public class PlayerResponseModel
    {
        public PlayerModel? Player {  get; set; }
        public PlayerRecord? Stats { get; set; }
        public int Appearances { get; set; }
        public List<FootballWeb.Repository.Team> Teams { get; set; } = [];
    }
}
