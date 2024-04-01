using Microsoft.AspNetCore.Http;
using MVC.Project.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.Project.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "Max Length of Name is 50 chars")]
        [MinLength(3, ErrorMessage = "Min Length of Name is 3 chars")]
        public string Name { get; set; }


        [Range(22, 30)]
        public int? Age { get; set; }


        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$")]
        [Required(ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }


        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }


        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }


        [EmailAddress]
        public string Email { get; set; }


        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }


        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }


        public Gender Gender { get; set; }


        public EmpType EmpType { get; set; }


        public int? DepartmentId { get; set; } // Foregin Key Column


        // Navigational Prop [One]
        public Department Department { get; set; }


        public IFormFile Image { get; set; }


        public string ImageName { get; set; }


    }
}
