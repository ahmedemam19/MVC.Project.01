using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;
using MVC.Project.DAL.Models;
using MVC.Project.PL.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MVC.Project.PL.Controllers
{
    // Inheritance : Departmentcontroller is a controller 
    // Association : Departmentcontroller has a DepartmentRepository


    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IDepartmentRepository _deparmtentsRepo;
        private readonly IWebHostEnvironment _env; // For the [ catch in Action Edit ]

        // Ask CLR for creating object from class implementing IDepartmentRepository
        public DepartmentController(
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            //IDepartmentRepository departmentRepo, 
            IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_deparmtentsRepo = departmentRepo;
            _env = env;
        }

         
        #region Action Index

        // /Department/Index
        // Return all the departments in the DepartmentRepo
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

            var mappedDeps = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);

            return View(mappedDeps);
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
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) // Server side validation 
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVM);


                _unitOfWork.DepartmentRepository.Add(mappedDep);

                // 3. TempData : to tranfer Data from the Current request (Create) to the Subsquent request (Index)

                var count = _unitOfWork.Complete();

                if (count > 0)
                    TempData["Message"] = "Department is Created Successfuly";
                else
                    TempData["Message"] = "Error occured while Creating the Department";

                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }

        #endregion


        #region Action Details

        // /Department/Details/10
        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest(); // 400

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);

            var mappedDep = _mapper.Map<Department, DepartmentViewModel>(department);

            if (department is null)
                return NotFound(); // 404

            return View(ViewName, mappedDep);
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
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM) // [FromRoute] indicates that the parameter should be bound from the [ route data ] of the incoming request URL.
        {

            if (id != departmentVM.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(departmentVM);

            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.DepartmentRepository.Update(mappedDep);
                _unitOfWork.Complete();

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

                return View(departmentVM);
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
        public IActionResult Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVM);

                _unitOfWork.DepartmentRepository.Delete(mappedDep);
                _unitOfWork.Complete();
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

                return View(departmentVM);

            }
        }

        #endregion


    }
}
