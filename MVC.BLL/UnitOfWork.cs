using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.DAL.Data;
using MVC.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MvcDbContext mvcDbContext;
        private Hashtable ReposContainer;
      

        public UnitOfWork(MvcDbContext mvcDbContext)
        {
            this.mvcDbContext = mvcDbContext;
            this.ReposContainer = new Hashtable();
          
           
        }
        public int Complete()
        {
           return  this.mvcDbContext.SaveChanges();
        }

        public void Dispose()
        {
           this.mvcDbContext.Dispose();
        }

        public IGenericRepository<T> GetRepository<T>() where T : BaseModel
        {
            var Key=typeof(T).Name;
            if (!this.ReposContainer.ContainsKey(Key)) {
                IGenericRepository<T> Repo;
                if (typeof(T) == typeof(Employee)) {
                     Repo = new EmployeeRepository(this.mvcDbContext) as IGenericRepository<T>;
                }
                else {
                    Repo = new GenericRepository<T>(this.mvcDbContext);
                }
                 this.ReposContainer.Add(Key, Repo);  
            }
            return ReposContainer[Key] as IGenericRepository<T>;
        }
    }
}
