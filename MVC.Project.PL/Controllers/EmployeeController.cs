using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;
using MVC.Project.DAL.Models;
using System;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace MVC.Project.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env; // For the [ catch in Action Edit ]

        // Ask CLR for creating object from class implementing IEmployeeRepository
        public EmployeeController(IEmployeeRepository employeeRepository, /*IDepartmentRepository departmentRepository ,*/ IWebHostEnvironment env)
        {
            _employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _env = env;
        }


        #region Action Index
        // Return all the Employees in the EmployeeRepo
        public IActionResult Index(string SearchInput) // [ string SearchInput ] for the search input
        {
            TempData.Keep();

            // Binding through View Dictionary : Data Sent from the Action to View [one way]

            // 1. ViewData
            // 1. ViewData is a Dictionary Type Property (introduced in ASP .NET Framework 3.5)
            //    => It helps us to transfer the data from controller[Action] to View
            //ViewData["Message"] = "Hello From ViewData";


            // 2. ViewBag
            // 2. ViewBag is a Dynamic Type Property (introduced in ASP .NET Framework 4.0 based on dynamic feature
            //    => It helps us to transfer the data from controller[Action] to view
            //ViewBag.Message = "Hello ViewBag";

            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(SearchInput))
                 employees = _employeeRepository.GetAll();
            else
                 employees = _employeeRepository.SearchByName(SearchInput.ToLower());

            return View(employees);



        }
        #endregion

        #region Action Create
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll(); // to return all departments in the Repository
            /*ViewBag.Departments = _departmentRepository.GetAll();*/ // to return all departments in the Repository

            return View(); // return tthe same view with same name of action
        }


        // Action for Validating the info of employee entered by the user and add it to EmployeeRepo if true .
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            // first it checks if the info added by the user compatible by the system requirments
            if(ModelState.IsValid) // Server side validation 
            {
                var count = _employeeRepository.Add(employee);

                // 3. TempData : to tranfer Data from the Current request (Create) to the Subsquent request (Index)

                if (count > 0)
                    TempData["Message"] = "Employee is Created Successfuly";
                else
                    TempData["Message"] = "Error occured while Creating the Employee";

                return RedirectToAction(nameof(Index));

            }
            return View(employee);
        }
        #endregion

        #region Action Details
        // Action for returning the employee Details in the [ Details View ]
        [HttpGet]
        // the Action Details take two parameters (1) for the employee ID (2) for the ViewName for displaying the Employee Details
        public ActionResult Details(int? id, string ViewName = "Details") 
        {
            if (id is null)
                return BadRequest(); // 400
            var employees = _employeeRepository.Get(id.Value);
            if (employees is null)
                return NotFound(); //404
            return View(ViewName, employees);
        }
        #endregion

        #region Action Edit
        [HttpGet]
        // Edit Action take the Employee ID and return the Employee Details in the Edit View to Edit the Employee Info
        public IActionResult Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll(); // to return all departments in the Repository

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Action FIlter used to prevent cross-site request forgery (CSRF) attacks
        public IActionResult Edit([FromRoute] int? id, Employee employee) // [FromRoute] indicates that the parameter should be bound from the [ route data ] of the incoming request URL.
        {
            if (id != employee.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(employee);

            try
            {
                _employeeRepository.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occurred during updating your Employee Info");

                return View(employee);
            }
        }
        #endregion

        #region Action Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                _employeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occurred during deleting your Employee");

                return View(employee);
            }
        } 
        #endregion

    }
}
