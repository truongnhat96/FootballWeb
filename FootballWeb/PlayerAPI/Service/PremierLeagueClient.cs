using FootballWeb.PlayerAPI.ApiModel;
using Microsoft.AspNetCore.WebUtilities;
using static FootballWeb.PlayerAPI.ApiModel.PulseliveModel;

namespace FootballWeb.PlayerAPI.Service
{
    public class PremierLeagueClient : IPremierLeagueClient
    {
        private readonly HttpClient _http;
        public PremierLeagueClient(HttpClient http) => _http = http;

        public async Task<Root?> GetPlayerResultAsync(string playerName, CancellationToken ct = default)
        {
            var url = $"search/PremierLeague/?terms={Uri.EscapeDataString(playerName)}&type=player&size=30&start=0&fullObjectResponse=true";
            return await _http.GetFromJsonAsync<Root>(url, ct);
        }


        public async Task<List<PulselivePlayer>> GetPlayersAsync(int compSeasonId, CancellationToken ct = default)
        {
            var all = new List<PulselivePlayer>();
            int page = 0;

            dynamic resp;
            do
            {
                // Build URL đúng

                //var url = $"football/players"
                //        + $"?compSeasons={compSeasonId}"
                //        + $"&altIds=true&type=player&id=-1"
                //        + $"&pageSize=100&page={page}";


                //var url = QueryHelpers.AddQueryString(
                //    "football/players",
                //    new Dictionary<string, string?>
                //    {
                //        ["compSeasons"] = compSeasonId.ToString(),
                //        ["compSeasonId"] = compSeasonId.ToString(),
                //        ["altIds"] = "true",
                //        ["type"] = "player",
                //        ["id"] = "-1",
                //        ["pageSize"] = "100",
                //        ["page"] = page.ToString()
                //    });

                var basePath = "football/players";
                var queryParams = new Dictionary<string, string?>
                {
                    ["pageSize"] = "100",
                    ["compSeasons"] = compSeasonId.ToString(),
                    ["altIds"] = "true",
                    ["page"] = page.ToString(),
                    ["type"] = "player",
                    ["id"] = "-1",
                    ["compSeasonId"] = compSeasonId.ToString()
                };
                var url = QueryHelpers.AddQueryString(basePath, queryParams);

                // var url = $"football/teams/14/squad?compSeasons={compSeasonId}&pageSize=100&altIds=true&detail=2";

                resp = await _http.GetFromJsonAsync<PlayerListResponse>(url, ct)
                             ?? throw new InvalidOperationException($"Null response at page {page}");

                all.AddRange(resp.Content);
                page++;
            }
            while (page < resp.PageInfo.NumPages);

            return all;
        }
    }
}
