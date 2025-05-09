using FootballWeb.PlayerAPI.ApiModel;
using FootballWeb.PlayerAPI.Service;
using FootballWeb.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FootballWeb.Controllers
{
    public class TeamsController : Controller
    {
        private readonly IFplService _fpl;
        private readonly DataContext _context;

        public TeamsController(IFplService fpl, DataContext context)
        {
            _fpl = fpl;
            _context = context;
        }

        [HttpGet("/Manage/Team")]
        public IActionResult TeamList()
        {
            var teams = _context.Teams.ToList();
            return View(teams);
        }

        [HttpGet("/Edit/Team/{id:int}")]
        public IActionResult Edit(int id)
        {
            var team = _context.Teams.Find(id);
            if (team == null)
            {
                return View();
            }
            return View(new FplTeam
            {
                id = team.Id,
                name = team.Name,
                short_name = team.ShortName,
            });
        }

        [HttpPost("/Edit/Team/{id:int}")]
        public async Task<IActionResult> Edit(FplTeam team)
        {
            if (ModelState.IsValid)
            {
                var existingTeam = _context.Teams.Find(team.id);
                if (existingTeam != null)
                {
                    existingTeam.Name = team.name;
                    existingTeam.ShortName = team.short_name;
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật đội bóng thành công!";
                }
                else
                {
                    var newTeam = new Repository.Team
                    {
                        Id = team.id,
                        Name = team.name,
                        ShortName = team.short_name,
                    };
                    await _context.Teams.AddAsync(newTeam);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Thêm đội bóng thành công!";
                }
            }
            return View(team);
        }

        [HttpPost("/Delete/Team/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if(team == null)
            {
                return NotFound("Không tìm thấy Team có id: " + id);
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Xóa đội thành công!";
            return View("Edit");
        }

        [HttpGet("/team")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var data = await _fpl.GetBootstrapAsync(ct);
            var list = data.teams.Select(t => new {
                t.id,
                t.name,
                t.short_name
            });
            return Ok(list);
        }
    }
}
