using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using MVC.Project.DAL.Models;
using MVC.Project.PL.ViewModels.User;
using System.Threading.Tasks;

namespace MVC.Project.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}


        #region Sign Up

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // to check that their isn't any username with the same name
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null) // when the username is new 
                {
                    // Manual Mapping
                    user = new ApplicationUser()
                    {
                        FName = model.FirstName,
                        LName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        IsAgree = model.IsAgree,
                    };

                    var result =  await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    // when an error occur while Registering
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }

                // this is called whenn their is a similar username
                ModelState.AddModelError(string.Empty, "this UserName is already in use for another account");
            }
            return View(model);
        }

        #endregion



        #region Sign In


        public IActionResult SignIn()
        {
            return View();
        }


        #endregion

    }
}
