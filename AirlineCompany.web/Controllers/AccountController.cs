using AirlineCompany.web.Helpers;
using AirlineCompany.web.Models;
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

        
    }
}
