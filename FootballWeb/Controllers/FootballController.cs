using FootballWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using FootballWeb.Models;
using FootballWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FootballWeb.Controllers
{
    public class FootballController : Controller
    {
        private readonly FootballDataService _footballService;
        private readonly HttpClient _httpClient;
        private readonly DataContext _context;

        public FootballController(FootballDataService footballService, HttpClient httpClient, DataContext context)
        {
            _footballService = footballService;
            _httpClient = httpClient;
            _context = context;
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

        // Hiển thị chi tiết trận đấu theo Match ID - Lấy thông tin từ Database (thiếu giao diện)
        public async Task<IActionResult> Details(int id, string name)
        {
            var matchesDetail = await _context.Statistics
                .Where(s => s.MatchId == id.ToString())
                .ToListAsync();
            if(matchesDetail != null && matchesDetail.Count > 1)
            {
                var matchDetail = new MatchStatistic
                {
                    HomeTeam = matchesDetail.First(),
                    AwayTeam = matchesDetail.Last()
                };
                return View(matchDetail);
            }
            else
            {
                TempData["Message"] = "Không tìm thấy thông số trận đấu";
                return View(new MatchStatistic());
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

        #region Manager Match (CRUD)
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMatch()
        {
            var matcheStr = await _footballService.GetPremierLeagueMatchesAsync();
            return View("MatchList", matcheStr);
        }

        [HttpGet("/Edit/Match/{id}/{name}")]
        public async Task<IActionResult> EditMatch(string id, string name)
        {
            var statistics = await _context.Statistics.Where(s => s.MatchId == id).ToListAsync();
            TempData["MatchName"] = name;
            if (statistics != null && statistics.Count > 1)
            {
                var matches = new MatchStatistic
                {
                    HomeTeam = statistics.First(),
                    AwayTeam = statistics.Last()
                };
                return View(matches);
            }
            return View(new MatchStatistic
            {
            });
        }

        [HttpPost("/Edit/Match/{id}/{name}")]
        public async Task<IActionResult> EditMatch(string id, string name, MatchStatistic matchStatistic)
        {
            var statistics = await _context.Statistics.Where(s => s.MatchId == id).ToListAsync();
            TempData["MatchName"] = name;
            if (matchStatistic.HomeTeam == null || matchStatistic.AwayTeam == null)
            {
                TempData["Message"] = "Không tìm thấy thông số để cập nhật";
                return View(new MatchStatistic());
            }
            if (statistics != null && statistics.Count > 1)
            {
                var homeTeam = statistics.First();
                var awayTeam = statistics.Last();
                homeTeam.CornerKicks = matchStatistic.HomeTeam.CornerKicks;
                homeTeam.YellowCards = matchStatistic.HomeTeam.YellowCards;
                homeTeam.RedCards = matchStatistic.HomeTeam.RedCards;
                homeTeam.Shots = matchStatistic.HomeTeam.Shots;
                homeTeam.ShotsOnGoal = matchStatistic.HomeTeam.ShotsOnGoal;
                homeTeam.BallPossession = matchStatistic.HomeTeam.BallPossession;
                homeTeam.Fouls = matchStatistic.HomeTeam.Fouls;
                homeTeam.Offsides = matchStatistic.HomeTeam.Offsides;
                homeTeam.ShotsOffGoal = matchStatistic.HomeTeam.ShotsOffGoal;
                homeTeam.FreeKicks = matchStatistic.HomeTeam.FreeKicks;
                homeTeam.Saves = matchStatistic.HomeTeam.Saves;
                homeTeam.ThrowIns = matchStatistic.HomeTeam.ThrowIns;
                awayTeam.CornerKicks = matchStatistic.AwayTeam.CornerKicks;
                awayTeam.YellowCards = matchStatistic.AwayTeam.YellowCards;
                awayTeam.RedCards = matchStatistic.AwayTeam.RedCards;
                awayTeam.Shots = matchStatistic.AwayTeam.Shots;
                awayTeam.ShotsOnGoal = matchStatistic.AwayTeam.ShotsOnGoal;
                awayTeam.BallPossession = matchStatistic.AwayTeam.BallPossession;
                awayTeam.Fouls = matchStatistic.AwayTeam.Fouls;
                awayTeam.Offsides = matchStatistic.AwayTeam.Offsides;
                awayTeam.ShotsOffGoal = matchStatistic.AwayTeam.ShotsOffGoal;
                awayTeam.FreeKicks = matchStatistic.AwayTeam.FreeKicks;
                awayTeam.Saves = matchStatistic.AwayTeam.Saves;
                awayTeam.ThrowIns = matchStatistic.AwayTeam.ThrowIns;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Cập nhật thông số thành công";
            }
            else
            {
                matchStatistic.HomeTeam.MatchId = id;
                matchStatistic.AwayTeam.MatchId = id;
                await _context.Statistics.AddAsync(matchStatistic.HomeTeam);
                await _context.Statistics.AddAsync(matchStatistic.AwayTeam);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Thêm thông số thành công";
            }
            return View(matchStatistic);
        }

        [HttpPost("Delete/Match/{id}/{name}")]
        public async Task<IActionResult> DeleteMatch(string id, string name)
        {
            var statistics = await _context.Statistics.Where(s => s.MatchId == id).ToListAsync();
            TempData["MatchName"] = name;
            if (statistics != null && statistics.Count > 1)
            {
                _context.Statistics.RemoveRange(statistics);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Xóa thông số thành công";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy thông số để xóa";
            }
            return View("EditMatch", new MatchStatistic());
        }

        #endregion
    }
}
