using FootballWeb.PlayerAPI.ApiModel;
using FootballWeb.PlayerAPI.Service;
using FootballWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballWeb.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IFplService _fpl;
        private readonly IPremierLeagueClient _pl;
        private readonly ILogger<PlayersController> _logger;
        private readonly DataContext _context;

        public PlayersController(IFplService fpl, IPremierLeagueClient pl, ILogger<PlayersController> logger, DataContext context)
        {
            _fpl = fpl;
            _pl = pl;
            _logger = logger;
            _context = context;
        }

        [HttpGet("/player/{compSeasonId:int}/{teamId:int}")]
        public async Task<IActionResult> GetByTeam(int compSeasonId, int teamId, CancellationToken ct)
        {
            if(teamId >= 1000)
            {
                var team = await _context.Teams.FindAsync(teamId);
                var players = await _context.Players
                    .Where(p => p.TeamId == teamId)
                    .ToListAsync();
                var model = new List<PlayerModel>();
                foreach(var item in players)
                {
                    model.Add(new PlayerModel
                    {
                        ShirtNo = item.ShirtNo,
                        Name = item.Name,
                        Age = item.Age.ToString(),
                        Nationality = item.Nationality,
                        DateOfBirth = item.DateOfBirth.ToShortDateString(),
                        Position = item.Position,
                        Team = team!.Name,
                        ImageUrl = item.ImageUrl,
                        Id = item.Id
                    });
                }
                TempData["Team"] = team!.Name;
                return View("ListPlayer", model);
            }
            var fplData = await _fpl.GetBootstrapAsync(ct);

            var plList = await _pl.GetPlayersAsync(compSeasonId, ct);

            var plMap = plList
                .Where(pl => !string.IsNullOrEmpty(pl.AltIds.opta))
                .GroupBy(pl => pl.AltIds.opta)
                .Select(g => g.First())
                .ToDictionary(pl => pl.AltIds.opta!, pl => pl);


            // 3) Merge: chỉ current squad FPL, tách code từ e.photo
            var squad = fplData.elements
                .Where(e => e.team == teamId)
                .Select(e =>
                {
                    var code = "p" + Path.GetFileNameWithoutExtension(e.photo);
                    var r = plMap.TryGetValue(code, out var plInfo);

                    return new PlayerModel
                    {
                        Id = e.id,
                        Name = plInfo != null
                                      ? $"{plInfo.Name?.First} {plInfo.Name?.Last}"
                                      : $"{e.first_name} {e.second_name}",
                        ShirtNo = (int)(plInfo?.Infor?.ShirtNum ?? 0),
                        Nationality = plInfo?.NationalTeam?.Country!,
                        Age = plInfo?.Age!,
                        DateOfBirth = plInfo?.Birth?.DateOfBirth?.Label!,
                        Team = fplData.teams.FirstOrDefault(t => t.id == e.team)!.name,
                        Position = plInfo?.Infor?.Position!,
                        ImageUrl = $"https://resources.premierleague.com/premierleague/photos/players/110x140/{code}.png"
                    };
                })
        .ToList();
            TempData["Team"] = fplData.teams.FirstOrDefault(t => t.id == teamId)!.name;
            return View("ListPlayer", squad);
        }

        [HttpPost]
        public async Task<IActionResult> GetPlayerDetail(PlayerModel model)
        {
            if(model.Id >= 10000)
            {
                var player = await _context.Players.FindAsync(model.Id);
                var record = await _context.PlayerRecords.FindAsync(model.Id);
                return View("PlayerRecord", new PlayerResponseModel
                {
                    Player = new PlayerModel
                    {
                        Id = model.Id,
                        Name = player!.Name,
                        Age = player.Age.ToString(),
                        Nationality = player.Nationality,
                        DateOfBirth = player.DateOfBirth.ToShortDateString(),
                        Position = player.Position,
                        ShirtNo = player.ShirtNo,
                        Team = (await _context.Teams.FindAsync(player.TeamId))!.Name,
                        ImageUrl = player.ImageUrl
                    },
                    Appearances = record!.Appearances,
                    Stats = new PlayerAPI.ApiModel.PlayerRecord
                    {
                        Id = model.Id,
                        Saves = record.Saves,
                        CleanSheets = record.CleanSheets,
                        YellowCards = record.YellowCards,
                        Assists = record.Assists,
                        GoalsConceded = record.GoalsConceded,
                        GoalsScored = record.GoalsScored,
                        RedCards = record.RedCards,
                        Minutes = record.Minutes,
                        OwnGoals = record.OwnGoals,
                        Value = record.Value,
                        PenaltiesMissed = record.PenaltiesMissed,
                        PenaltiesSaved = record.PenaltiesSaved,
                        TotalPoints = record.TotalPoints
                    }
                });
            }
            var result = await _pl.GetPlayerResultAsync(model.Name);
            if (result?.Hits?.Hit.First().Response == null)
            {
                var data = await _fpl.GetPlayerStatsAsync(model.Id);
                var player = new PlayerAPI.ApiModel.PlayerRecord();
                foreach (var item in data.Records)
                {
                    player.Id = item.Id;
                    player.TotalPoints += item.TotalPoints;
                    player.Minutes += item.Minutes;
                    player.GoalsScored += item.GoalsScored;
                    player.Assists += item.Assists;
                    player.CleanSheets += item.CleanSheets;
                    player.GoalsConceded += item.GoalsConceded;
                    player.OwnGoals += item.OwnGoals;
                    player.PenaltiesSaved += item.PenaltiesSaved;
                    player.PenaltiesMissed += item.PenaltiesMissed;
                    player.YellowCards += item.YellowCards;
                    player.RedCards += item.RedCards;
                    player.Saves += item.Saves;
                    player.Value = item.Value;
                }
                return View("PlayerRecord", new PlayerResponseModel
                {
                    Player = model,
                    Stats = player,
                    Appearances = data.Records.Count
                });
            }
            return View("PlayerDetail", result);
        }

        [HttpGet("/Player/List")]
        public async Task<IActionResult> PlayersManage()
        {
            var players = await _context.Players.ToListAsync();
            var model = new List<PlayerModel>();
            foreach(var item in players)
            {
                model.Add(new PlayerModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Age = item.Age.ToString(),
                    DateOfBirth = item.DateOfBirth.ToShortDateString(),
                    Nationality = item.Nationality,
                    ShirtNo = item.ShirtNo,
                    Position = item.Position,
                    Team = (await _context.Teams.FindAsync(item.TeamId))!.Name,
                    ImageUrl = item.ImageUrl
                });
            }
            return View(model);
        }

        [HttpGet("/Edit/Player/{Id:int}")]
        public async Task<IActionResult> EditPlayer(int Id)
        {
            var player = await _context.Players.FindAsync(Id);
            if (player != null)
            {
                var record = await _context.PlayerRecords.FindAsync(Id) ?? throw new();
                var model = new PlayerResponseModel
                {
                    Appearances = record.Appearances,
                    Stats = new PlayerAPI.ApiModel.PlayerRecord
                    {
                        Saves = record.Saves,
                        CleanSheets = record.CleanSheets,
                        Assists = record.Assists,
                        GoalsConceded = record.GoalsConceded,
                        GoalsScored = record.GoalsScored,
                        RedCards = record.RedCards,
                        YellowCards = record.YellowCards,
                        Minutes = record.Minutes,
                        OwnGoals = record.OwnGoals,
                        Value = record.Value,
                        PenaltiesMissed = record.PenaltiesMissed,
                        PenaltiesSaved = record.PenaltiesSaved,
                        TotalPoints = record.TotalPoints
                    },
                    Player = new PlayerModel
                    {
                        Id = player.Id,
                        Age = player.Age.ToString(),
                        Name = player.Name,
                        Nationality = player.Nationality,
                        Position = player.Position,
                        ShirtNo = player.ShirtNo,
                        DateOfBirth = player.DateOfBirth.ToShortDateString(),
                        ImageUrl = player.ImageUrl,
                        Team = (await _context.Teams.FindAsync(player.TeamId))!.Name
                    },
                    Teams = await _context.Teams.ToListAsync()
                };
                return View(model);
            }
            return View(new PlayerResponseModel
            {
                Teams = await _context.Teams.ToListAsync()
            });
        }

        [HttpPost("/Edit/Player/{Id:int}")]
        public async Task<IActionResult> EditPlayer(PlayerResponseModel model)
        {
            var player = await _context.Players.FindAsync(model.Player?.Id);
            if(player != null)
            {
                var record = await _context.PlayerRecords.FindAsync(model?.Player?.Id);
                player.Name = model?.Player?.Name!;
                player.Age = int.Parse(model?.Player?.Age!);
                player.DateOfBirth = DateTime.Parse(model?.Player?.DateOfBirth!);
                player.ShirtNo = model!.Player!.ShirtNo!;
                player.Position = model?.Player?.Position!;
                player.Nationality = model?.Player?.Nationality!;
                player.TeamId = Convert.ToInt32(model?.Player?.Team!);
                if (model!.Player.ImageFile != null)
                {
                    var fileName = Path.GetFileName(model.Player.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Player.ImageFile.CopyToAsync(stream);
                    }
                    player.ImageUrl = "/images/" + fileName;
                }
                if (record != null)
                {
                    record.Appearances = model!.Appearances;
                    record.Saves = model!.Stats?.Saves ?? 0;
                    record.CleanSheets = model.Stats?.CleanSheets ?? 0;
                    record.Assists = model.Stats?.Assists ?? 0;
                    record.GoalsConceded = model.Stats?.GoalsConceded ?? 0;
                    record.GoalsScored = model.Stats?.GoalsScored ?? 0;
                    record.RedCards = model.Stats?.RedCards ?? 0;
                    record.YellowCards = model.Stats?.YellowCards ?? 0;
                    record.Minutes = model.Stats?.Minutes ?? 0;
                    record.OwnGoals = model.Stats?.OwnGoals ?? 0;
                    record.Value = model.Stats?.Value ?? 0;
                    record.PenaltiesMissed = model.Stats?.PenaltiesMissed ?? 0;
                    record.PenaltiesSaved = model.Stats?.PenaltiesSaved ?? 0;
                    record.TotalPoints = model.Stats?.TotalPoints ?? 0;
                }
                await _context.SaveChangesAsync();
                TempData["Message"] = "Cập nhật thông tin cầu thủ thành công!";
            }
            else
            {
                if(model?.Player?.ImageFile != null)
                {
                    var fileName = Path.GetFileName(model.Player.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Player.ImageFile.CopyToAsync(stream);
                    }
                    model.Player!.ImageUrl = "/images/" + fileName;
                }
                await _context.Players.AddAsync(new Player
                {
                    ShirtNo = model!.Player!.ShirtNo,
                    Name = model.Player.Name,
                    Age = int.Parse(model.Player.Age),
                    Nationality = model.Player.Nationality,
                    Position = model.Player.Position,
                    DateOfBirth = DateTime.Parse(model.Player.DateOfBirth),
                    TeamId = Convert.ToInt32(model.Player.Team),
                    ImageUrl = model.Player.ImageUrl,
                });
                await _context.SaveChangesAsync();
                var playerId = await _context.Players
                    .Where(p => p.Name == model.Player.Name && p.TeamId == Convert.ToInt32(model.Player.Team))
                    .Select(p => p.Id)
                    .FirstOrDefaultAsync();
                await _context.AddAsync(new Repository.PlayerRecord
                {
                    Id = playerId,
                    Appearances = model.Appearances,
                    Saves = model.Stats?.Saves ?? 0,
                    CleanSheets = model.Stats?.CleanSheets ?? 0,
                    Assists = model.Stats?.Assists ?? 0,
                    GoalsConceded = model.Stats?.GoalsConceded ?? 0,
                    GoalsScored = model.Stats?.GoalsScored ?? 0,
                    RedCards = model.Stats?.RedCards ?? 0,
                    YellowCards = model.Stats?.YellowCards ?? 0,
                    Minutes = model.Stats?.Minutes ?? 0,
                    OwnGoals = model.Stats?.OwnGoals ?? 0,
                    Value = model.Stats?.Value ?? 0,
                    PenaltiesMissed = model.Stats?.PenaltiesMissed ?? 0,
                    PenaltiesSaved = model.Stats?.PenaltiesSaved ?? 0,
                    TotalPoints = model.Stats?.TotalPoints ?? 0
                });
                await _context.SaveChangesAsync();
                TempData["Message"] = "Thêm mới cầu thủ thành công!";
            }
            return View(model);
        }

        [HttpPost("/Delete/{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var player = await _context.Players.FindAsync(Id);
            if (player != null)
            {
                var record = await _context.PlayerRecords.FindAsync(Id);
                if (record != null)
                {
                    _context.PlayerRecords.Remove(record);
                }
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Xóa cầu thủ thành công!";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy cầu thủ!";
            }
            return View("EditPlayer", new PlayerResponseModel
            {
                Teams = await _context.Teams.ToListAsync()
            });
        }
    }
}
