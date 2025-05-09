using FootballWeb.Models;
using FootballWeb.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FootballWeb.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _dataContext;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, DataContext dataContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dataContext = dataContext;
        }

        [HttpGet("/Roles")]
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpGet("/Add")]
        [HttpGet("/Update")]
        public IActionResult AddUpdate(RoleModel model)
        {
            return View("Add", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(model.Name);
                if (model.IsUpdate)
                {
                    var roleToUpdate = await _roleManager.FindByIdAsync(model.Id.ToString());
                    if (roleToUpdate != null)
                    {
                        roleToUpdate.Name = model.Name;
                        await _roleManager.UpdateAsync(roleToUpdate);
                    }
                }
                else
                {
                    await _roleManager.CreateAsync(role);
                }
                TempData["Status"] = model.IsUpdate ? "Cập nhật thành công" : "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["Status"] = "Xóa thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Xóa không thành công");
                }
            }
            else
            {
                ModelState.AddModelError("", "Role không tồn tại");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/User")]
        public async Task<IActionResult> UserView()
        {
            var users = await (from user in _dataContext.Users
                               join userRole in _dataContext.UserRoles on user.Id equals userRole.UserId
                               join role in _dataContext.Roles on userRole.RoleId equals role.Id
                               select new UsersModel
                               {
                                   UserName = user.UserName ?? string.Empty,
                                   RoleName = role.Name ?? "User"
                               }).ToListAsync();
            foreach (var user in _userManager.Users)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                if (userRole.Count == 0)
                {
                    users.Add(new UsersModel
                    {
                        UserName = user.UserName ?? string.Empty,
                        RoleName = "User"
                    });
                }
            }
            return View(users);
        }

        [HttpGet]
        public IActionResult UpdateRole(UsersModel model)
        {
            model.Roles = _roleManager.Roles.Select(r => r.Name ?? string.Empty).ToList() ?? throw new ArgumentNullException();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(UsersModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (model.RoleName == "User")
                    {
                        var allRole = _dataContext.UserRoles.Where(ur => ur.UserId == user.Id);
                        _dataContext.UserRoles.RemoveRange(allRole);
                        await _dataContext.SaveChangesAsync();
                        TempData["Status"] = "Cập nhật thành công";
                        return RedirectToAction("UserView");
                    }
                    var roles = await _userManager.GetRolesAsync(user);
                    var result = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (result.Succeeded)
                        {
                            TempData["Status"] = "Cập nhật thành công";
                            return RedirectToAction("UserView");
                        }
                    }
                }
            }
            return View("UpdateRole", model);
        }
    }
}
