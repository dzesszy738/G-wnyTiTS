using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Logowanie.Models;
using Logowanie.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Logowanie.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AdminController(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {


            var mail = _userManager.GetUserName(User);
            var user = _userManager.FindByNameAsync(mail).Result;
            if (user == null || !(_userManager.
                      IsEmailConfirmedAsync(user).Result))
            {
                ViewBag.Message = "Error while resetting your password!";
                return View("ErrorReset");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);


            return View("Reset");

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Index(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Hasło);
                var a = model.Role.ToString();
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role.ToString());
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, model.Role.ToString()));

                    string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;

                    var result2 = await _userManager.ConfirmEmailAsync(user, ctoken);

                    _logger.LogInformation("User created a new account with password.");
                    return View();
                }
            }
            return View(model);

        }

    }
}

