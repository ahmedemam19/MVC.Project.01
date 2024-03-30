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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext) // Ask CLR for Creating Object from "ApplicationDbContext"
        {
            //_dbContext = new ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<ApplicationDbContext>);
            _dbContext = dbContext;
        }

        public void Add(T entity)
            => _dbContext.Set<T>().Add(entity);


        public void Update(T entity)
            => _dbContext.Set<T>().Update(entity);
        

        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);


        public T Get(int id)
        {
            //return _dbContext.Set<T>().Find(id);

            return _dbContext.Find<T>(id); // EF Core 3.1 New Feature
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>) _dbContext.Employees.Include(e => e.Department).AsNoTracking().ToList();
            else
                return  _dbContext.Set<T>().AsNoTracking().ToList();
        }
    }
}
