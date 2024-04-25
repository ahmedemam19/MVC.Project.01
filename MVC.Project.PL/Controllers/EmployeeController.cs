using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;
using MVC.Project.DAL.Models;
using MVC.Project.PL.Helpers;
using MVC.Project.PL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Project.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env; // For the [ catch in Action Edit ]

        // Ask CLR for creating object from class implementing IEmployeeRepository
        public EmployeeController(
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            //IEmployeeRepository employeeRepository,
            /*IDepartmentRepository departmentRepository ,*/ 
            IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _env = env;
        }


        #region Action Index
        // Return all the Employees in the EmployeeRepo
        public async Task<IActionResult> Index(string SearchInput) // [ string SearchInput ] for the search input
        {
            TempData.Keep();

            #region Notes

            // Binding through View Dictionary : Data Sent from the Action to View [one way]

            // 1. ViewData
            // 1. ViewData is a Dictionary Type Property (introduced in ASP .NET Framework 3.5)
            //    => It helps us to transfer the data from controller[Action] to View
            //ViewData["Message"] = "Hello From ViewData";


            // 2. ViewBag
            // 2. ViewBag is a Dynamic Type Property (introduced in ASP .NET Framework 4.0 based on dynamic feature
            //    => It helps us to transfer the data from controller[Action] to view
            //ViewBag.Message = "Hello ViewBag"; 
            #endregion

            var employees = Enumerable.Empty<Employee>();

            var employeeRepo = _unitOfWork.Repository<Employee>() as EmployeeRepository;

            if (string.IsNullOrEmpty(SearchInput))
                 employees = await employeeRepo.GetAllAsync();
            else
                 employees = employeeRepo.SearchByName(SearchInput.ToLower());

            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(mappedEmps);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            // first it checks if the info added by the user compatible by the system requirments
            if(ModelState.IsValid) // Server side validation 
            {

                employeeVM.ImageName = await DoucmentSetting.UploadFile(employeeVM.Image, "images");

                // Mapping from EmployeeViewModel to Employee
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                

                _unitOfWork.Repository<Employee>().Add(mappedEmp);


                // 3. TempData : to tranfer Data from the Current request (Create) to the Subsquent request (Index)

                var count = await _unitOfWork.Complete();

                if (count > 0)
                    TempData["Message"] = "Employee is Created Successfuly";
                else
                    TempData["Message"] = "Error occured while Creating the Employee";

                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);
        }
        #endregion

        #region Action Details
        // Action for returning the employee Details in the [ Details View ]
        [HttpGet]
        // the Action Details take two parameters (1) for the employee ID (2) for the ViewName for displaying the Employee Details
        public async Task<IActionResult> Details(int? id, string ViewName = "Details") 
        {
            if (id is null)
                return BadRequest(); // 400
            var employees = await _unitOfWork.Repository<Employee>().GetAsync(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employees);

            if (employees is null)
                return NotFound(); //404

            if(ViewName.Equals("Delete", StringComparison.OrdinalIgnoreCase))
                TempData["ImageName"] = employees.ImageName;

            return View(ViewName, mappedEmp);
        }
        #endregion

        #region Action Edit
        [HttpGet]
        // Edit Action take the Employee ID and return the Employee Details in the Edit View to Edit the Employee Info
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll(); // to return all departments in the Repository

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Action FIlter used to prevent cross-site request forgery (CSRF) attacks
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeViewModel employeeVM) // [FromRoute] indicates that the parameter should be bound from the [ route data ] of the incoming request URL.
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(employeeVM);




            try
            {

                employeeVM.ImageName = await DoucmentSetting.UploadFile(employeeVM.Image, "images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.Repository<Employee>().Update(mappedEmp);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occurred during updating your Employee Info");

                return View(employeeVM);
            }
        }
        #endregion

        #region Action Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            try
            {
                employeeVM.ImageName = TempData["ImageName"] as string;

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);


                _unitOfWork.Repository<Employee>().Delete(mappedEmp);

                var count = await _unitOfWork.Complete();
                if(count > 0)
                {
                    DoucmentSetting.DeleteFile(employeeVM.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
                return View(employeeVM);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "An Error has Occurred during deleting your Employee");

                return View(employeeVM);
            }
        } 
        #endregion

    }
}
