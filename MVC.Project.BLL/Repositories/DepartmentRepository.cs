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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext) // Ask CLR for Creating Object from "ApplicationDbContext"
        {
            //_dbContext = new ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext>);
            _dbContext = dbContext;
        }

        public int Add(Department entity)
        {
            _dbContext.Departments.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(Department entity)
        {
            _dbContext.Departments.Update(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(Department entity)
        {
            _dbContext.Departments.Remove(entity);
            return _dbContext.SaveChanges();
        }
        

        public Department Get(int id)
        {
            ///var department = _dbContext.departments.Local.Where(d => d.Id == id).FirstOrDefault();
            ///if(department == null)
            ///    department = _dbContext.departments.Local.Where(d => d.Id == id).FirstOrDefault();
            ///return department;

            //return _dbContext.departments.Find(id);

            return _dbContext.Find<Department>(id); // EF Core 3.1 New Feature
        }

        public IEnumerable<Department> GetAll()
            => _dbContext.Departments.AsNoTracking().ToList();




    }
}
