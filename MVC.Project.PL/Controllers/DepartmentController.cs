using Microsoft.AspNetCore.Mvc;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;

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
            //var departments = _deparmtentsRepo;
            return View();
        }
    }
}
