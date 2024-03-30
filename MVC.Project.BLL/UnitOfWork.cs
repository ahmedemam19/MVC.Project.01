using MVC.Project.BLL.Interfaces;
using MVC.Project.BLL.Repositories;
using MVC.Project.DAL.Data;
using MVC.Project.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.BLL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;


        private Hashtable _repositories;


        public UnitOfWork(ApplicationDbContext dbContext) // Ask CLR for creating object from DBContext
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }


        public IGenericRepository<T> Repository<T>() where T : ModelBase
        {
            var Key = typeof(T).Name; // Employee

            if(!_repositories.ContainsKey(Key))
            {
                
                if (Key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    _repositories.Add(Key , repository);
                }
                else
                {
                    var repository = new GenericRepository<T>(_dbContext);
                    _repositories.Add(Key, repository);
                }
            }

            return _repositories[Key] as IGenericRepository<T>;
        }

        
        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose(); // to close the connection
        }

        
    }
}
