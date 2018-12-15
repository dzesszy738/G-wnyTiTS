using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Logowanie.Models;
using Microsoft.AspNetCore.Authorization;
using Logowanie.Models.AccountViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text;

namespace Logowanie.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        const string DOMAIN = "sandbox915dc42a586d45a2afccef193af0eee8.mailgun.org";
        const string API_KEY = "f0b8649cda7531f100a399133a846159-b3780ee5-eb6d9383";


        public AccountController(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        

        public void Test1(string to,string text)
        {
            Test1Async(to,text).Wait();
        }

        public async Task Test1Async(string to, string text)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes("api" + ":" + API_KEY)));


            var form = new Dictionary<string, string>();
            form["from"] = "prywatna.przychodnia@tits.com";
            form["to"] = to;
            form["subject"] = "Weryfikacja konta";
            form["text"] = "W celu weryfikacji adresu email prosimy o kliknięcie w poniższy link:"+"\n"+text;

            var response = await client.PostAsync("https://api.mailgun.net/v2/" + DOMAIN + "/messages", new FormUrlEncodedContent(form));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Debug.WriteLine("Success");
            }
            else
            {
                Debug.WriteLine("StatusCode: " + response.StatusCode);
                Debug.WriteLine("ReasonPhrase: " + response.ReasonPhrase);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Hasło);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Pacjent");

                    string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    string ctokenlink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        token = ctoken
                    }, protocol: HttpContext.Request.Scheme);
                    ViewBag.token = ctokenlink;
                    Test1(model.Email, ctokenlink);





                    await _signInManager.SignInAsync(user, isPersistent: false);

                    _logger.LogInformation("User created a new account with password.");
                    return View(model);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Hasło, model.Zapamiętaj, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}