using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        IEnumerable<T> getAll();
        T get(int id);

        int Add(T entity);

        int Update(T entity);

        int Delete(T entity);
    }
}
