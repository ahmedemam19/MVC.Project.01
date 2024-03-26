using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;
using MVC.Project.DAL.Models;
using System;

namespace MVC.Project.PL.Controllers
{
    // Inheritance : Departmentcontroller is a controller 
    // Association : Departmentcontroller has a DepartmentRepository


    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _deparmtentsRepo;
        private readonly IWebHostEnvironment _env; // For the [ catch in Action Edit ]

        // Ask CLR for creating object from class implementing IDepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepo, IWebHostEnvironment env)
        {
            _deparmtentsRepo = departmentRepo;
            _env = env;
        }

         
        #region Action Index

        // /Department/Index
        // Return all the departments in the DepartmentRepo
        public IActionResult Index()
        {
            var departments = _deparmtentsRepo.GetAll();
            return View(departments);
        }

        #endregion


        #region Action Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // Action for Validating the info of departments entered by the user and add it to the DepartmentRepo if true .
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) // Server side validation 
            {
                var count = _deparmtentsRepo.Add(department);
                if (count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        #endregion


        #region Action Details

        // /Department/Details/10
        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); // 400

            var department = _deparmtentsRepo.Get(id.Value);

            if (department is null)
                return NotFound(); // 404

            return View(ViewName, department);
        }

        #endregion


        #region Action Edit

        // /Department/Edit/10
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ///if (id is null)
            ///    return BadRequest(); // 400
            ///var department = _deparmtentsRepo.Get(id.Value);
            ///if (department is null)
            ///    return NotFound(); // 404
            ///return View(department);

            return Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // Action FIlter used to prevent cross-site request forgery (CSRF) attacks
        public IActionResult Edit([FromRoute] int id, Department department) // [FromRoute] indicates that the parameter should be bound from the [ route data ] of the incoming request URL.
        {

            if (id != department.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(department);

            try
            {
                _deparmtentsRepo.Update(department);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Message

                if(_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occurred during updating yuor Department"); // Should be written in a JSON file

                return View(department);
            }
        }

        #endregion


        #region Action Delete

        // /Department/Delete/10
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _deparmtentsRepo.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                // 2. Friendly Message

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occurred during Deleting your Department"); // Should be written in a JSON file

                return View(department);

            }
        }

        #endregion


    }
}
