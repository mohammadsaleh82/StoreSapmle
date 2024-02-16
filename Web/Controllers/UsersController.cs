using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        ViewData["UserName"] = user.UserName;
        var userRoles = await _userManager.GetRolesAsync(user);
        ViewData["UserRols"] = userRoles;
        var roles =await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(List<string> Roles, string UserName)
    {
        var user = await _userManager.FindByNameAsync(UserName);
        var userRoles = await _userManager.GetRolesAsync(user);

        var rolesToAdd = Roles.Except(userRoles);
        var rolesToRemove = userRoles.Except(Roles);

        await _userManager.AddToRolesAsync(user, rolesToAdd);
        await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        if (UserName==User.Identity.Name)
        {
            return Redirect($"/Auth/LogOut");
        }
        return RedirectToAction(nameof(Index));
    }
}