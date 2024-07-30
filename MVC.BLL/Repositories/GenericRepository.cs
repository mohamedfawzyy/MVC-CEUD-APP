using Microsoft.EntityFrameworkCore;
using MVC.BLL.Interfaces;
using MVC.DAL.Data;
using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T:BaseModel
    {
        private protected  MvcDbContext mvcDbContext;

        public GenericRepository(MvcDbContext mvcDbContext)
        { 
            this.mvcDbContext = mvcDbContext;
        }
        public void Add(T entity)
        {
           this.mvcDbContext.Set<T>().Add(entity);
           
        }

        public void Delete(T entity)
        {
            this.mvcDbContext.Set<T>().Remove(entity);  
          
        }

        public async Task<T> getAsync(int id)
        {
            if (typeof(T) == typeof(Employee)) {
                return await this.mvcDbContext.Set<Employee>().Include(E=>E.Department).Where(E=>E.Id == id).FirstOrDefaultAsync() as T;
            }
          T entity=await this.mvcDbContext.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<T>> getAllAsync()
        {
            if (typeof(T) == typeof(Employee)) {
                return  (IEnumerable<T>) await this.mvcDbContext.Set<Employee>().Include(E=>E.Department).AsNoTracking().ToListAsync();
            }
          return await this.mvcDbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public void Update(T entity)
        {
            this.mvcDbContext.Set<T>().Update(entity);
           
        }
    }
}
