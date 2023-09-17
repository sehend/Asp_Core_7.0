using IdentityUyelik.Areas.Admin.Models;
using IdentityUyelik.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityUyelik.Extenions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IdentityUyelik.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]

    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,

                Name = x.Name
            }).ToListAsync();

            return View(roles);
        }

        [Authorize(Roles = "roleaction")]
        public IActionResult RolesCreat()
        {
            return View();
        }

        [Authorize(Roles = "roleaction")]
        [HttpPost]
        public async Task<IActionResult> RolesCreat(RolesCreatViewModel request)
        {
            var resualt = await _roleManager.CreateAsync(new AppRole() { Name = request.Name });

            if (!resualt.Succeeded)
            {
                ModelState.AddModelErrorList(resualt.Errors);

                return View();
            }

            return RedirectToAction(nameof(RolesController.Index));
        }

        [Authorize(Roles = "roleaction")]
        public async Task<IActionResult> RolesUpdate(string id)
        {
            var roleToUpadate = await _roleManager.FindByIdAsync(id);

            if (roleToUpadate == null)
            {
                throw new Exception("Güncellenicek Rol Bulunamamıştır....");
            }

            return View(new RoleUpdateViewModel() { Id = roleToUpadate.Id, Name = roleToUpadate.Name });
        }

        [Authorize(Roles = "roleaction")]
        [HttpPost]
        public async Task<IActionResult> RolesUpdate(RoleUpdateViewModel request)
        {
            var roleToUpadate = await _roleManager.FindByIdAsync(request.Id);

            if (roleToUpadate == null)
            {
                throw new Exception("Güncellenicek Rol Bulunamamıştır....");
            }

            roleToUpadate.Name = request.Name;

            await _roleManager.UpdateAsync(roleToUpadate);

            ViewData["SuccededMessage"] = "Rol Bilgisi Güncellenmiştir....";

            return View();
        }

        [Authorize(Roles = "roleaction")]
        public async Task<IActionResult> RolesDelete(string id)
        {
            var roleToUpadate = await _roleManager.FindByIdAsync(id);

            var resualt = await _roleManager.DeleteAsync(roleToUpadate);

            if (!resualt.Succeeded)
            {
                ModelState.AddModelErrorList(resualt.Errors);
            }

            TempData["SuccededMessage"] = "Role Sillimiştir....";

            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> AssignRolesToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);

            ViewBag.userId = currentUser.Id;

            var roles = await _roleManager.Roles.ToListAsync();

            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var roleViewModelList = new List<AssignRoleToUserViewModel>();

            foreach (var role in roles)
            {

                var assignRoleToUserViewModel = new AssignRoleToUserViewModel()
                {
                    Id = role.Id,

                    Name = role.Name,

                };

                if (userRoles.Contains(role.Name))
                {
                    assignRoleToUserViewModel.Exist = true;
                }

                roleViewModelList.Add(assignRoleToUserViewModel);
            }

            return View(roleViewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRolesToUser(string userId, List<AssignRoleToUserViewModel> requestList)
        {
            var userToAssignRoles = await _userManager.FindByIdAsync(userId);

            foreach (var role in requestList)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(userToAssignRoles, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userToAssignRoles, role.Name);
                }
            }

            return RedirectToAction(nameof(HomeController.UserList), "Home");
        }
    }
}
