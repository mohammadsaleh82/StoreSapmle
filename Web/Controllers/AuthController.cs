using Application.Core;
using Application.Dtos.Auth;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? ReturnUrl = "")
    {
        ViewData["returnUrl"] = ReturnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginDto loginDto, string? ReturnUrl)
    {
        Result<object> response = new Result<object>();
        var validator = new LoginValidator();
        var validationResult = await validator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            ModelState.Clear();
            string errors = "";
            foreach (var error in validationResult.Errors)
            {
                errors += ('\n' + error.ErrorMessage);
            }

            response = Result<object>.Failure(errors);
            return Ok(response);
        }

        var result =
            await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, loginDto.RememberMe, false);

        // check if the sign in was successful
        if (result.Succeeded)
        {
            // var user = await _userManager.FindByNameAsync(loginDto.Username);
            // await _signInManager.SignInAsync(user, false);
            // return a 200 OK response with the username


            ReturnUrl = ReturnUrl is not null? ReturnUrl : "/";


            response = Result<object>.Success(new { ReturnUrl = ReturnUrl });
            return Ok(response);
        }

        response = Result<object>.Failure("please check username and password");
        return Ok(response);
    }

    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        if (User.Identity.IsAuthenticated)
            return Redirect("/");

        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var validator = new RegisterValidator();
        var validationResult = await validator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
        {
            ModelState.Clear();
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return View(registerDto);
        }

        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Avatar = "default.png"
        };

        // use the user manager to create the user with the password
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        // check if the creation was successful
        if (result.Succeeded)
        {
            // sign in the user with the sign in manager
            await _signInManager.SignInAsync(user, false);

            // return a 200 OK response with the user data
            ViewData["Succeeded"] = "true";
            return View();
        }

        foreach (var error in result.Errors)
        {
            var key = error.Code.Split("Duplicate")[1];
            ModelState.AddModelError(key,error.Description);
        }

      
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Index));
    }
}