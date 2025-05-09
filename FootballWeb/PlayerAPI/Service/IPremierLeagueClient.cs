using FootballWeb.PlayerAPI.ApiModel;
using static FootballWeb.PlayerAPI.ApiModel.PulseliveModel;

namespace FootballWeb.PlayerAPI.Service
{
    public interface IPremierLeagueClient
    {
        public Task<List<PulselivePlayer>> GetPlayersAsync(int seasonId, CancellationToken ct = default);
        public Task<Root?> GetPlayerResultAsync(string playerName, CancellationToken ct = default);
    }
}
