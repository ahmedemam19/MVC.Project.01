using System.ComponentModel.DataAnnotations;

namespace MVC.Project.PL.ViewModels.Account
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "First Name is REQUIRED")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }


		[Required(ErrorMessage = "Last Name is REQUIRED")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }


        [Required(ErrorMessage = "UserName is REQUIRED")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        

        [Required(ErrorMessage = "Email is REQUIRED")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is REQUIRED")]
        [MinLength(5, ErrorMessage = "Minimum password length is 5")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


		[Required(ErrorMessage = "Confirm Password is REQUIRED")]
		[DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password Dont match with password")]
		public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "You must agree on terms and conditions")]
        public bool IsAgree { get; set; }

    }
}
