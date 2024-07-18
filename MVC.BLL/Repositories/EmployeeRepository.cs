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

    }
}
