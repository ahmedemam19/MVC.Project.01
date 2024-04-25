
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.DAL.Models
{

    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }

    public enum EmpType
    {
        [EnumMember(Value = "FullTime")]
        FullTime = 1,

        [EnumMember(Value = "PartTime")]
        PartTime = 2
    }

    public class Employee : ModelBase
    {

        #region Data

        public string Name { get; set; }


        public int? Age { get; set; }


        public string Address { get; set; }


        public decimal Salary { get; set; }


        public bool IsActive { get; set; }


        public string Email { get; set; }


        public string PhoneNumber { get; set; }


        public DateTime HiringDate { get; set; }


        public Gender Gender { get; set; }


        public EmpType EmpType { get; set; }

        #endregion


        public string ImageName { get; set; }


        public int? DepartmentId { get; set; }


        public virtual Department Department { get; set; }

    }
}
