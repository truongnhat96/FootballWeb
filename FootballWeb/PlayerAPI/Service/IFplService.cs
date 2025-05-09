using FootballWeb.PlayerAPI.ApiModel;

namespace FootballWeb.PlayerAPI.Service
{
    public interface IFplService
    {
        Task<FplBootstrap> GetBootstrapAsync(CancellationToken ct = default);
        Task<PlayerStatsResponse> GetPlayerStatsAsync(int playerId, CancellationToken ct = default);
    }
}
