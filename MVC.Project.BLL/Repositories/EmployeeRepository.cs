using Microsoft.EntityFrameworkCore;
using MVC.Project.BLL.Interfaces;
using MVC.Project.DAL.Data;
using MVC.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        #region         #region Inherited From IEmployeeRepository and then inherited from IGernericRepository
        //private readonly ApplicationDbContext _dbContext;

        //public EmployeeRepository(ApplicationDbContext dbContext) // Ask CLR for Creating Object from "ApplicationDbContext"
        //{
        //    //_dbContext = new ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext>);
        //    _dbContext = dbContext;
        //}

        //public int Add(Employee entity)
        //{
        //    _dbContext.Employees.Add(entity);
        //    return _dbContext.SaveChanges();
        //}


        //public int Update(Employee entity)
        //{
        //    _dbContext.Employees.Update(entity);
        //    return _dbContext.SaveChanges();
        //}


        //public int Delete(Employee entity)
        //{
        //    _dbContext.Employees.Remove(entity);
        //    return _dbContext.SaveChanges();
        //}


        //public Employee Get(int id)
        //{
        //    return _dbContext.Find<Employee>(id);
        //}


        //public IEnumerable<Employee> GetAll()
        //    => _dbContext.Employees.AsNoTracking().ToList(); 
        #endregion
        
        //private readonly ApplicationDbContext ;

        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


        public IQueryable<Employee> GetEmployeesByAddress(string address)
            => _dbContext.Employees.Where(e => e.Address.ToLower() == address.ToLower());

        public IQueryable<Employee> SearchByName(string Name)
            => _dbContext.Employees.Where(e => e.Name.ToLower().Contains(Name));

    }
}
