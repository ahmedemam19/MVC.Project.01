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

        public int Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            //_dbContext.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            //_dbContext.Update(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            //_dbContext.Remove(entity);
            return _dbContext.SaveChanges();
        }


        public T Get(int id)
        {
            //return _dbContext.Set<T>().Find(id);

            return _dbContext.Find<T>(id); // EF Core 3.1 New Feature
        }

        public IEnumerable<T> GetAll()
            => _dbContext.Set<T>().AsNoTracking().ToList();
    }
}
