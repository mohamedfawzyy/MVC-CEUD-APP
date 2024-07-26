using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.DAL.Models;
using MVC.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment env;

        public EmployeeController (IUnitOfWork unitOfWork,IMapper mapper, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
           
            this.env = env;
        }
        public IActionResult Index(string SerachName)
        {
            var Result = Enumerable.Empty<Employee>();
            var EmployeeRepo = this.unitOfWork.GetRepository<Employee>() as EmployeeRepository ; 
            if (string.IsNullOrEmpty(SerachName))
            {
                Result = EmployeeRepo.getAll();
            }
            else {
                Result = EmployeeRepo.GetByName(SerachName.ToLower());
            }
            var MapResult = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Result);
            return View(MapResult);
        }

        public IActionResult Create() {
            
            return View();  
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) {
                var employee = this.mapper.Map<EmployeeViewModel, Employee>(employeeVM);
               var Result= this.unitOfWork.GetRepository<Employee>().Add(employee);
                if (Result > 0) { 
                    TempData["Message"] = "Your Employee Created Successfully";
                }
                else
                {

                    TempData["Message"] = "sorry your Employee can't be created :(";

                }
                return RedirectToAction(nameof(Index));
        }
            return View(employeeVM);
        }

        public IActionResult view(int? id,string ViewName="view") {
            if (!id.HasValue)
                return BadRequest();
            var Result=this.unitOfWork.GetRepository<Employee>().get(id.Value);
            var ResultVM = this.mapper.Map<Employee, EmployeeViewModel>(Result);
            if(Result is null)
                return NotFound();  
            return View(ViewName, ResultVM);    
        
        }

        public IActionResult Update(int? id) {
           return view(id,"UPdate");
        }
        [HttpPost]
        public IActionResult Update([FromRoute]int id,EmployeeViewModel employeeVM)
        {
            if(id != employeeVM.Id)
                return BadRequest();    
            if (!ModelState.IsValid)
            {
                return View(employeeVM);
            }
            try
            {
                var employee = this.mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                var Result = this.unitOfWork.GetRepository<Employee>().Update(employee);

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
            return View(employeeVM);

        }
        [HttpPost]
        public IActionResult Delete(int id) {
            var Employee = this.unitOfWork.GetRepository<Employee>().get(id);
            if (Employee is null)
                return NotFound();
            var Result =this.unitOfWork.GetRepository<Employee>().Delete(Employee);
            if(Result > 0)
                return RedirectToAction(nameof(Index));
            var EmployeeVM=this.mapper.Map<Employee,EmployeeViewModel>(Employee);   
            return View(EmployeeVM);
        }
       
    }
}
