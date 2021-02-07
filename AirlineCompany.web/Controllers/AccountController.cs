using AirlineCompany.web.Data.Entities;
using AirlineCompany.web.Helpers;
using AirlineCompany.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineCompany.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper UserHelper;

        public AccountController (IUserHelper userHelper)
        {
            UserHelper = userHelper;
        }

        public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await this.UserHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnURL"))
                    {
                        return this.Redirect(this.Request.Query["ReturnURL"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return this.View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await this.UserHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username
                    };

                    var result = await this.UserHelper.AddUserAsync(user, model.Password);

                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created");
                        return this.View(model);
                    }

                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };

                    var result2 = await this.UserHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                    this.ModelState.AddModelError(string.Empty, "The user couldn't be logged");
                    return this.View(model);
                }

                this.ModelState.AddModelError(string.Empty, "The username is already registered");
            }

            return this.View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await this.UserHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var response = await this.UserHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated";
                    }

                    else
                    {
                        this.ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }

                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found");
                }
            }

            return this.View(model);
        }

        public IActionResult ChangePassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    var result = await this.UserHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }

                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }

                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found");
                }
            }

            return this.View(model);
        }
    }
}
