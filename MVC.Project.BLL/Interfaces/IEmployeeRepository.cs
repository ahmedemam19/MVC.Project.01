using MVC.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        #region Inherited From Class IGenericRepository
        //IEnumerable<Employee> GetAll();
        //Employee Get(int id);
        //int Add(Employee entity);
        //int Update(Employee entity);
        //int Delete(Employee entity); 
        #endregion

        IQueryable<Employee> GetEmployeesByAddress(string address);

        IQueryable<Employee> SearchByName(string Name);
    }
}
