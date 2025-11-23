using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CharityHub.Models;
using CharityHub.Models.ViewModels;

namespace CharityHub.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(
            UserManager<AppUser> userMgr,
            SignInManager<AppUser> signInMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            roleManager = roleMgr;
        }

        public ViewResult Login(string? returnUrl)
        {
            return View(new LoginModel
            {
                Email = string.Empty,
                Password = string.Empty,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await userManager.FindByEmailAsync(loginModel.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user,
                        loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("", "Invalid email or password");
            }
            return View(loginModel);
        }

        public ViewResult Register()
        {
            var model = new RegisterModel();
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "User", Text = "User" },
                new SelectListItem { Value = "Admin", Text = "Admin" }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = registerModel.Email,
                    Email = registerModel.Email,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName
                };

                IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    // Призначаємо роль
                    string roleToAssign = !string.IsNullOrEmpty(registerModel.Role) && 
                                         await roleManager.RoleExistsAsync(registerModel.Role)
                        ? registerModel.Role 
                        : "User";
                    
                    if (!await roleManager.RoleExistsAsync(roleToAssign))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleToAssign));
                    }
                    await userManager.AddToRoleAsync(user, roleToAssign);

                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "User", Text = "User" },
                new SelectListItem { Value = "Admin", Text = "Admin" }
            };
            return View(registerModel);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            AppUser? user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var roles = await userManager.GetRolesAsync(user);
            var model = new ProfileModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileModel profileModel)
        {
            if (ModelState.IsValid)
            {
                AppUser? user = await userManager.FindByIdAsync(profileModel.Id);
                if (user != null)
                {
                    user.FirstName = profileModel.FirstName;
                    user.LastName = profileModel.LastName;
                    user.Email = profileModel.Email;
                    user.UserName = profileModel.Email;

                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Profile updated successfully.";
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(profileModel);
        }

        [Authorize]
        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

