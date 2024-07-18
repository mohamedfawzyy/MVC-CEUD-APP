using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using System;

namespace MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IWebHostEnvironment env;

        public EmployeeController (IEmployeeRepository employeeRepository, IWebHostEnvironment env)
        {
            this.employeeRepository = employeeRepository;
            this.env = env;
        }
        public IActionResult Index()
        {
            var Result=this.employeeRepository.getAll();    
            return View(Result);
        }

        public IActionResult Create() {
        
            return View();  
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) { 
               var Result= this.employeeRepository.Add(employee);
                if (Result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest();
            }
            return View(employee);
        }

        public IActionResult view(int? id,string ViewName="view") {
            if (!id.HasValue)
                return BadRequest();
            var Result=this.employeeRepository.get(id.Value);   
            if(Result is null)
                return NotFound();  
            return View(ViewName, Result);    
        
        }

        public IActionResult Update(int? id) {
           return view(id,"UPdate");
        }
        [HttpPost]
        public IActionResult Update([FromRoute]int id,Employee employee)
        {
            if(id != employee.Id)
                return BadRequest();    
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            try
            {
                var Result = this.employeeRepository.Update(employee);
                if (Result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                if (this.env.IsDevelopment())
                {
                    ModelState.AddModelError("", ex.Message);

                }
                else {

                    ModelState.AddModelError("", "Error whie updating your data sorry :(");
                }

            }
            return View(employee);

        }
        [HttpPost]
        public IActionResult Delete(int id) {
            var Employee = this.employeeRepository.get(id);
            if (Employee is null)
                return NotFound();
            var Result =this.employeeRepository.Delete(Employee);
            if(Result > 0)
                return RedirectToAction(nameof(Index));
            return View(Employee);
        }
       
    }
}
