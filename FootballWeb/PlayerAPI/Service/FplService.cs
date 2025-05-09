using FootballWeb.PlayerAPI.ApiModel;

namespace FootballWeb.PlayerAPI.Service
{
    public class FplService : IFplService
    {
        private readonly HttpClient _http;
        public FplService(HttpClient http) => _http = http;

        public async Task<FplBootstrap> GetBootstrapAsync(CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<FplBootstrap>(
                "https://fantasy.premierleague.com/api/bootstrap-static/", ct
            ) ?? throw new();
        }

        public async Task<PlayerStatsResponse> GetPlayerStatsAsync(int playerId, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<PlayerStatsResponse>(
                $"https://fantasy.premierleague.com/api/element-summary/{playerId}/", ct
            ) ?? throw new();
        }
    }
}
