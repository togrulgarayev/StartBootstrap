using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
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

            if (identityResult.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = Url.Action("ConfirmEmail", "Accounts", new { token, email = register.Email }, Request.Scheme);
                

                bool emailResponse = SendEmail(register.Email, confirmationLink);

                if (emailResponse)
                    return RedirectToAction("ConfirmedEmail","Accounts");


            }
            else
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }

            //await _userManager.AddToRoleAsync(newUser, UserRoles.Admin.ToString());  /* For create user with Admin Role */
            await _userManager.AddToRoleAsync(newUser, UserRoles.Member.ToString());  /* For create user with Member Role */
            return RedirectToAction("Login", "Accounts");
        }

        public ActionResult ConfirmedEmail()
        {
            return View();
        }

        public ActionResult ConfirmEmail()
        {
            return View();
        }

        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("yaponiski.togrul@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("yaponiski.togrul@gmail.com", "zyrjlbrmuvibsrqr");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
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
            
            var appUser = await _userManager.FindByNameAsync(model.Username);

            var result = await _signInManager.PasswordSignInAsync(
                model.Username, model.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            bool emailStatus = await _userManager.IsEmailConfirmedAsync(appUser);
             if (emailStatus == false)
             {
                 ModelState.AddModelError(nameof(model.Username), "Email is unconfirmed, please confirm it first");
             }

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
