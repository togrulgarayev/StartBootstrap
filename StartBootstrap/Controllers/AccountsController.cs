using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StartBootstrap.Data.Entities;
using StartBootstrap.Data.ViewModels.Account;

namespace StartBootstrap.Controllers
{
    public class AccountsController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid) return View(register);

            AppUser newUser = new AppUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.Username
            };

            var identityResult = await _userManager.CreateAsync(newUser, register.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }

                return View(register);
            }

            //await _userManager.AddToRoleAsync(newUser, UserRoles.Admin.ToString());  /* For create user with Admin Role */
            await _userManager.AddToRoleAsync(newUser, UserRoles.Member.ToString());  /* For create user with Member Role */
            return RedirectToAction("Login", "Accounts");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Username, model.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");


            ModelState.AddModelError(string.Empty, "Login Failed");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        #region  Create roles
        /*    First -> add role name to UserRoles (enum) 
              Second -> call CreateAdminRole action          */


        

        public enum UserRoles
        {
            Admin,
            Member
        }

        /*

        public async Task CreateAdminRole()
        {

            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });

                }
            }
        }

        */

        #endregion


    }
}
