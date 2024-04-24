using System.ComponentModel.DataAnnotations;

namespace MVC.Project.PL.ViewModels.Account
{
	public class ForgetPasswordViewModel
	{

		[Required(ErrorMessage = "Email is REQUIRED")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }


	}
}
