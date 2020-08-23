using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using G14App1.Models;
using G14App1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace G14App1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager,
            RoleManager<IdentityRole> _roleManager
            )
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager=  _roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    City=model.City,
                    Gender=model.Gender
                };

                var r = await userManager.CreateAsync(user, model.Password);
                if (r.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("AllProducts", "Home");
                }
                foreach (var err in r.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("AllProducts", "Home");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (r.Succeeded)
                {
                    return RedirectToAction("AllProducts", "Home");
                }

                ModelState.AddModelError(string.Empty, "Wrong User / Pass");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole {Name=model.RoleName};
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Account");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(model);
        }


        public IActionResult ListRoles()
        {
            return View(roleManager.Roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role==null)
            {
                ViewBag.ErrorMsg = "role Cannot be found";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMsg = "role Cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var r = await roleManager.UpdateAsync(role);
                if (r.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var err in r.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
                return View(model);
            }

        }

    }
}