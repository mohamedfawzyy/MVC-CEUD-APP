﻿using Microsoft.EntityFrameworkCore;
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
        public int Add(T entity)
        {
           this.mvcDbContext.Set<T>().Add(entity);
            return this.mvcDbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            this.mvcDbContext.Set<T>().Remove(entity);  
            return this.mvcDbContext.SaveChanges(); 
        }

        public T get(int id)
        {
          T entity=this.mvcDbContext.Set<T>().Find(id);
            return entity;
        }

        public IEnumerable<T> getAll()
        {
          return  this.mvcDbContext.Set<T>().AsNoTracking();
        }

        public int Update(T entity)
        {
            this.mvcDbContext.Set<T>().Update(entity);
            return this.mvcDbContext.SaveChanges();
        }
    }
}