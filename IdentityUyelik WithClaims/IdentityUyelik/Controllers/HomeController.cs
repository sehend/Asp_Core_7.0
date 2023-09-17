using IdentityUyelik.Extenions;
using IdentityUyelik.Models;
using IdentityUyelik.Services;
using IdentityUyelik.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace IdentityUyelik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;

            _userManager = userManager;

            _signInManager = signInManager;

            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            AppUser hasUser = null;

            if (model.Email != null)
            {
                hasUser = await _userManager.FindByEmailAsync(model.Email);
            }

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email Veya Şifre Yanlış....");

                return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResualt;

            if (model.Password != null)
            {

                signInResualt = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RemmemberMe, true);
            }
            else
            {
                return View();
            }

            if (signInResualt.Succeeded)
            {
                if (hasUser.BirthDay.HasValue)
                {
                    await _signInManager.SignInWithClaimsAsync(hasUser, model.RemmemberMe, new[] { new Claim("BirthDay", hasUser.BirthDay.Value.ToString()) });
                }


                return RedirectToAction("Index", "Member");
            }

            if (signInResualt.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 Dakkika Boyunca Giriş Yapamazsınız...." });

                return View();
            }

            ModelState.AddModelErrorList(new List<string>() { "Email Veya Şifre Yanlış.... ", $"Başarısız Giriş Sayısı = {await _userManager.GetAccessFailedCountAsync(hasUser)}" });

            return View();
        }



        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm);

            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

                return View();
            }

            if (identityResult.Succeeded)
            {
                var exChangeExpireClaim = new Claim("ExChangeExpireDate", DateTime.Now.AddDays(10).ToString());

                var user = await _userManager.FindByEmailAsync(request.Email);

                var claimResualt = await _userManager.AddClaimAsync(user, exChangeExpireClaim);

                TempData["SuccededMessage"] = "Üyelik Kayıt İşlemi Başarılı....";

                if (!claimResualt.Succeeded)
                {
                    ModelState.AddModelErrorList(claimResualt.Errors.Select(x => x.Description).ToList());

                    return View();
                }

                return RedirectToAction(nameof(HomeController.SignUp));

            }

            return View();

        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {

            var hasUser = await _userManager.FindByEmailAsync(request.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu E Mail Adresine Sahip Kulanıcı Bulunamamıştır....");

                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

            TempData["success"] = "Şifre Yenileme Linki E Maili Adresinize Gönderilmiştir";

            return RedirectToAction(nameof(ForgetPassword));
        }



        public IActionResult ResetPassword(string userId, string token)
        {

            TempData["userId"] = userId;

            TempData["token"] = token;


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {

            var userId = TempData["userId"];

            var token = TempData["token"];

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = userId, Token = token }, HttpContext.Request.Scheme);

            if (userId == null || token == null)
            {
                return RedirectToActionPermanent("ResetPassword", "Home", new { userId = userId, Token = token });
            }


            //if (userId == null || token == null)
            //{
            //    throw new Exception("Bir Hata Meydana Geldi");
            //}

            var hasUser = await _userManager.FindByIdAsync(userId.ToString());

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kulanıcı Bulunamamiştır....");

                return View();
            }

            var resualt = await _userManager.ResetPasswordAsync(hasUser, token.ToString(), request.Password);

            if (resualt.Succeeded)
            {

                TempData["SuccsessMessage"] = "Şifre Başarı İle Yenilenmiştir....";

            }
            else
            {
                ModelState.AddModelErrorList(resualt.Errors.Select(x => x.Description).ToList());

            }



            return RedirectToActionPermanent("ResetPassword", "Home", new { userId = userId, Token = token });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
