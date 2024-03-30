using MVC.Project.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MVC.Project.PL.ViewModels
{
    public class DepartmentViewModel
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Code is Required!!")]
        public string Code { get; set; }


        [Required(ErrorMessage = "Name is Required!!")]
        public string Name { get; set; }


        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }


        // Navigational Prop Many
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    }
}
