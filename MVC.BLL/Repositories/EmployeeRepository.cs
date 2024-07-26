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
    public class EmployeeRepository:GenericRepository<Employee> , IEmployeeRepository
    {

        public EmployeeRepository(MvcDbContext mvcDbContext):base(mvcDbContext)
        {
            this.mvcDbContext=mvcDbContext;
        }

        public IQueryable<Employee> GetByName(string SearchName)
        {
          return  this.mvcDbContext.Employees.Include(e=>e.Department).Where(e => e.Name.ToLower().Contains(SearchName));
        }
        /* solution of overriding
public new IEnumerable<Employee> getAll()
{
   return this.mvcDbContext.Set<Employee>().Include(E=>E.Department).AsNoTracking();
}*/
    }
}
