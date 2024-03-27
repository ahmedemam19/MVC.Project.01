using Microsoft.Extensions.DependencyInjection;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;

namespace MVC.Project.PL.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        }

    }
}
