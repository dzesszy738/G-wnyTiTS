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

using System.Security.Claims;
using Logowanie.Models;
using Microsoft.Extensions.DependencyInjection;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text;

using Logowanie.Data;

using System.Net.Mail;
using System.Security.Cryptography;



namespace Logowanie.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;






        ApplicationDbContext _db;
       




        public AccountController(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    ILogger<AccountController> logger,ApplicationDbContext db)
        {
            _db = db;
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

        // Metody obsługujące wysyłanie emaili
        public void Test1(string to, string text, string sub)
        {
            Test1Async(to, text, sub).Wait();
        }

        public async Task Test1Async(string to, string text, string sub)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("api" + ":" + API_KEY)));


            var form = new Dictionary<string, string>();
            form["from"] = "prywatna.przychodnia@tits.com";
            form["to"] = to;
            form["subject"] = sub;
            form["text"] = text;

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
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            returnUrl = null;
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Hasło);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Pacjent");
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Pacjent"));

                    Pacjent p = new Pacjent();
                    p.Email = user.Email;
                    _db.Add(p);

                    _db.SaveChanges();
                    string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    string ctokenlink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = user.Id,
                        token = ctoken
                    }, protocol: HttpContext.Request.Scheme);
                    ViewBag.token = ctokenlink;

                    string sub = "Weryfikacja adresu email";
                    Test1(user.Email, "W celu weryfikacji adresu email prosimy o kliknięcie w link: " + ctokenlink, sub);


                    await _signInManager.SignInAsync(user, isPersistent: false);

                    _logger.LogInformation("User created a new account with password.");
                    return View(model);
                }
                AddErrors(result);
            }

            
            return View(model);
        }
       
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Login(string returnUrl = null)
        {
           

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
                    ModelState.AddModelError(string.Empty, "Niepoprawne dane logowania.");
                    return View(model);
                }
            }

           
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
        


        // Metoda obsługująca zmianę hasła - odpowiada jej ChangePasswordModel oraz widok ResetPassword znajdujący się w Home
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
                ModelState.AddModelError(string.Empty, "Brak potwierdzenia adresu email.");
                return View("ErrorReset");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);

            
            return View("Reset");

        }

        //Metody do zmiany hasła przez email odpowiada im model ForgottPassword oraz widoki ForgotPassword oraz Reset
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordMail(ForgottPassword model)
        {
            var user = _userManager.
                FindByNameAsync(model.Email).Result;

            if (user == null || !(_userManager.
                  IsEmailConfirmedAsync(user).Result))
            {
                ViewBag.Message = "Error while resetting your password!";
                return View("ErrorReset");
            }

            var ctoken = _userManager.
                  GeneratePasswordResetTokenAsync(user).Result;

          

            string nowehaslo = GetUniqueKey(10)+"*";

            IdentityResult result = await _userManager.ResetPasswordAsync(user, ctoken, nowehaslo);
            
           
            Test1(user.Email, "Oto twoje nowe hasło: "+nowehaslo, "Zmiana hasła");

            return View("Reset");

        }
        //Metoda do generowania losowego hasła
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(data);
            data = new byte[maxSize];
            crypto.GetBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}