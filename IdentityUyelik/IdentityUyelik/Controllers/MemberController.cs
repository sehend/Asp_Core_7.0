using IdentityUyelik.Extenions;
using IdentityUyelik.Models;
using IdentityUyelik.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace IdentityUyelik.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManager<AppUser> _userManager;

        private readonly IFileProvider _fileProvider;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider, IWebHostEnvironment webHostEnvironment)
        {
            _signInManager = signInManager;

            _userManager = userManager;

            _fileProvider = fileProvider;

            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var userViewModel = new UserViewModel { Email = currentUser.Email, UserName = currentUser.UserName, PhoenNumber = currentUser.PhoneNumber, PictureUrl = currentUser.Picture };

            return View(userViewModel);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();

        }

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski Şifreniz Yanlış....");

                return View();
            }

            var resualtChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

            if (!resualtChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resualtChangePassword.Errors.Select(x => x.Description).ToList());

                return View();
            }

            TempData["SuccededMessage"] = "Şifreniz Başarı ile Degiştirilmiştir....";

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();

            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            return View();
        }

        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));

            var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,

                Email = currentUser.Email,

                Phone = currentUser.PhoneNumber,

                BirthDay = currentUser.BirthDay,

                City = currentUser.City,

                Gender = currentUser.Gender

            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            currentUser.UserName = request.UserName;

            currentUser.Email = request.Email;

            currentUser.BirthDay = request.BirthDay;

            currentUser.City = request.City;

            currentUser.Gender = request.Gender;

            currentUser.PhoneNumber = request.Phone;


            if (currentUser.Picture != null)
            {


                var newPicturePath = Path.Combine(_webHostEnvironment.WebRootPath, "UserPicture", currentUser.Picture);

                FileInfo fi = new FileInfo(newPicturePath);

                System.IO.File.Delete(currentUser.Picture);

                fi.Delete();
            }

            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "UserPicture").PhysicalPath!, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;
            }

            var updateToUserResualt = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResualt.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResualt.Errors);
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();

            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccededMessage"] = "Üye Bilgileri Başarı ile Degiştirilmiştir....";


            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName,

                Email = currentUser.Email,

                Phone = currentUser.PhoneNumber,

                BirthDay = currentUser.BirthDay,

                City = currentUser.City,

                Gender = currentUser.Gender

            };


            return View(userEditViewModel);
        }

        public async Task<IActionResult> AccessDenied(string ReturnUrl)
        {
            string massage = "Bu Sayfayı Görmeye Yaetkiniz Yoktur.Yetki Almak İçin Yöneticinizle Görüşün....";

            ViewBag.massage = massage;

            return View();
        }

    }
}
