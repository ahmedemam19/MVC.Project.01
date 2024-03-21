using Microsoft.AspNetCore.Mvc;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;
using MVC.Project.DAL.Models;

namespace MVC.Project.PL.Controllers
{
    // Inheritance : Departmentcontroller is a controller 
    // Association : Departmentcontroller has a DepartmentRepository


    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _deparmtentsRepo;

        // Ask CLR for creating object from class implementing IDepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepo)
        {
            _deparmtentsRepo = departmentRepo;
        }

        // /Department/Index
        public IActionResult Index()
        {
            var departments = _deparmtentsRepo.GetAll();
            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid) // Server side validation 
            {
               var count = _deparmtentsRepo.Add(department);
                if(count > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
    }
}
