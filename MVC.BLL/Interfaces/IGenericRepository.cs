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
       Task<IEnumerable<T>> getAllAsync();
        Task<T> getAsync(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
