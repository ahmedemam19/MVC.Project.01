using Microsoft.Extensions.DependencyInjection;
using MVC.Project.BLL;
using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;

namespace MVC.Project.PL.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            return services;

        }

    }
}
