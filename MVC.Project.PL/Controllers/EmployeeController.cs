using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;
using MVC.Project.DAL.Models;
using System;

namespace MVC.Project.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _env; // For the [ ctach in Action Edit ]

        // Ask CLR for creating object from class implementing IEmployeeRepository
        public EmployeeController(IEmployeeRepository employeeRepository, IWebHostEnvironment env)
        {
            _employeeRepository = employeeRepository;
            _env = env;
        }


        #region Action Index
        // Return all the Employees in the EmployeeRepo
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        }
        #endregion

        #region Action Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // Action for Validating the info of employee entered by the user and add it to EmployeeRepo if true .
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            // first it checks if the info added by the user compatible by the system requirments
            if(ModelState.IsValid) // Server side validation 
            {
                var count = _employeeRepository.Add(employee);
                if (count > 0)
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
