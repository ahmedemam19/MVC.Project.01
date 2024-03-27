﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.DAL.Models
{
    public class Department : ModelBase
    {
        //public int Id { get; set; } // Inherited from class " ModelBase "

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
