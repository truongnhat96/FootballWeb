using FootballWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using FootballWeb.Models;

namespace FootballWeb.Controllers
{
    public class FootballController : Controller
    {
        private readonly FootballDataService _footballService;
        private readonly HttpClient _httpClient;

        public FootballController(FootballDataService footballService, HttpClient httpClient)
        {
            _footballService = footballService;
            _httpClient = httpClient;
        }

        // Hiển thị danh sách các trận đấu Premier League
        public async Task<IActionResult> Index()
        {
            var matches = await _footballService.GetPremierLeagueMatchesAsync();
            ViewBag.Matches = matches;
            return View();
        }

        // Hiển thị bảng xếp hạng Premier League
        public async Task<IActionResult> BangXepHang()
        {
            var matches = await _footballService.GetPremierLeagueMatchesAsync();
            var standings = await _footballService.GetPremierLeagueStandingsAsync();

            ViewBag.Matches = matches;
            ViewBag.Standings = standings;
            return View("TableXepHang");
        }

        // Hiển thị chi tiết trận đấu theo Match ID
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var apiUrl = $"https://api.football-data.org/v2/matches/{id}";

                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(content);
                var match = jsonDoc.RootElement.GetProperty("match");

                var homeTeamName = match.GetProperty("homeTeam").GetProperty("name").GetString();
                var awayTeamName = match.GetProperty("awayTeam").GetProperty("name").GetString();

                var statistics = match.GetProperty("statistics");

                var homeStats = ExtractStatistics(statistics.GetProperty("home"));
                var awayStats = ExtractStatistics(statistics.GetProperty("away"));

                var viewModel = new MatchDetailViewModel
                {
                    MatchId = id,
                    HomeTeamName = homeTeamName,
                    AwayTeamName = awayTeamName,
                    HomeStatistics = homeStats,
                    AwayStatistics = awayStats
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API error: {ex.Message}");
                return StatusCode(500, "Lỗi xử lý dữ liệu từ API.");
            }
        }

        // Hàm tiện ích để tạo MatchStatistics từ JsonElement
        private MatchStatistics ExtractStatistics(JsonElement teamStats)
        {
            return new MatchStatistics
            {
                Possession = teamStats.GetProperty("possession").GetString(),
                Shots = teamStats.GetProperty("shots").GetInt32(),
                ShotsOnTarget = teamStats.GetProperty("shotsOnTarget").GetInt32(),
                Corners = teamStats.GetProperty("corners").GetInt32(),
                YellowCards = teamStats.GetProperty("yellowCards").GetInt32(),
                RedCards = teamStats.GetProperty("redCards").GetInt32()
            };
        }
    }
}
