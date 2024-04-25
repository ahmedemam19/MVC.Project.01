using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVC.Project.DAL.Models;
using MVC.Project.PL.Services.EmailSender;
using MVC.Project.PL.ViewModels.Account;
using System.Threading.Tasks;

namespace MVC.Project.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly IEmailSender _emailSender;
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(
            IEmailSender emailSender,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
			_emailSender = emailSender;
			_configuration = configuration;
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

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "Your Account is Locked !!");

                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        if (result.IsNotAllowed)
                            ModelState.AddModelError(string.Empty, "Your Account is not Confirmed yet !!");

					}
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            }
            return View(model);
        }

        #endregion



        #region Sign Out


        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }


        #endregion



        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmailAsync(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {

                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user); // UniQue token for this user

                    var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, token = resetPasswordToken });
                   
                    // https://localhost:5001/Account/ResetPassword?email=ahmed@gmail.com&token=hwbkjbekfbvkbekfjvbkejvbkjevb

					await _emailSender.SendAsync(
                        from: _configuration["EmailSettings:SenderEmail"],
                        recipients: model.Email,
                        subject: "Reset your Password .",
                        body: resetPasswordUrl);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "There is no Account with this Email !!");
            }
            return View(model);
        }



        public IActionResult CheckYourInbox()
        {
            return View();
        }


        #endregion



        #region Reset Password


        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["Email"] = email;
            TempData["Token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var token = TempData["Token"] as string;

                var user = await _userManager.FindByEmailAsync(email);


                if(user is not null)
                {
					await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    return RedirectToAction(nameof(SignIn));
				}

                ModelState.AddModelError(string.Empty, "Url is not Valid !!");
				
            }
            return View(model);
        }




		#endregion


	}
}
