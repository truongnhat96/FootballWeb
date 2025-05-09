using System.Net.Http.Headers;
using System.Text.Json;

namespace FootballWeb.Services
{
    public class FootballDataService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.football-data.org/v4/";
        private const string ApiKey = "e59b50e27a8949d394e474ad8789b0d0";

        public FootballDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("X-Auth-Token", ApiKey);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey); // football-data dùng X-Auth-Token chứ không dùng Bearer
        }

        // Lấy danh sách trận đấu Premier League
        public async Task<string> GetPremierLeagueMatchesAsync()
        {
            var response = await _httpClient.GetAsync("competitions/PL/matches");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Lấy bảng xếp hạng Premier League
        public async Task<string> GetPremierLeagueStandingsAsync()
        {
            var response = await _httpClient.GetAsync("competitions/PL/standings");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        // Lấy đội hình thi đấu (lineup) theo matchId
        public async Task<JsonDocument> GetLineupAsync(int matchId)
        {
            var response = await _httpClient.GetAsync($"matches/{matchId}/lineups"); // Thêm /lineups vào URL
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(content);
        }


        // Lấy danh sách các trận đấu theo competitionId
        public async Task<JsonDocument> GetMatchesByCompetitionAsync(int competitionId)
        {
            var response = await _httpClient.GetAsync($"competitions/{competitionId}/matches");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(content);
        }
        // Lấy chi tiết trận đấu theo matchId
        public async Task<JsonDocument> GetMatchDetailAsync(int matchId)
        {
            if (matchId <= 0)
                throw new ArgumentException("Match ID không hợp lệ.", nameof(matchId));

            var url = $"matches/{matchId}";
            Console.WriteLine("Gọi API với URL: " + url);

            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Log chi tiết lỗi để dễ debug
                throw new HttpRequestException($"Lỗi API: {(int)response.StatusCode} - {response.ReasonPhrase}\nNội dung trả về: {content}");
            }

            return JsonDocument.Parse(content);
        }
        public async Task<JsonDocument> GetPremierLeagueMatchesJsonAsync()
        {
            var response = await _httpClient.GetAsync("competitions/PL/matches");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(content);
        }

    }
}
