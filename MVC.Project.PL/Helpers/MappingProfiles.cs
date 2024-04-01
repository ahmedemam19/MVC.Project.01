using AutoMapper;
using MVC.Project.DAL.Models;
using MVC.Project.PL.ViewModels;

namespace MVC.Project.PL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();

            CreateMap<DepartmentViewModel, Department>().ReverseMap();

        }
    }
}
