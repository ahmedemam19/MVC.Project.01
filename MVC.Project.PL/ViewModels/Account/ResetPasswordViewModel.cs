using System.ComponentModel.DataAnnotations;

namespace MVC.Project.PL.ViewModels.Account
{
	public class ResetPasswordViewModel
	{


		[Required(ErrorMessage = "Password is REQUIRED")]
		[MinLength(5, ErrorMessage = "Minimum password length is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }


		[Required(ErrorMessage = "Confirm Password is REQUIRED")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm Password Dont match with password")]
		public string ConfirmPassword { get; set; }

    }
}
