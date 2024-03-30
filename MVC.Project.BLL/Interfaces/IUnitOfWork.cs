using MVC.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<T> Repository<T>() where T : ModelBase;


        int Complete();
    }
}
