using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.DAL.Models;
using MVC.PL.Helpers;
using MVC.PL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(string SerachName)
        {
            var Result = Enumerable.Empty<Employee>();
            var EmployeeRepo = this.unitOfWork.GetRepository<Employee>() as EmployeeRepository ; 
            if (string.IsNullOrEmpty(SerachName))
            {
                Result =await EmployeeRepo.getAllAsync();
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
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) {
                employeeVM.ImageName =await DocumentSetting.UploadFileAsync(employeeVM.EmployeeImg, "images");
                var employee = this.mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                this.unitOfWork.GetRepository<Employee>().Add(employee);
                var Result=await this.unitOfWork.CompleteAsync();
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

        public async Task<IActionResult> view(int? id,string ViewName="view") {
            if (!id.HasValue)
                return BadRequest();
            var Result=await this.unitOfWork.GetRepository<Employee>().getAsync(id.Value);
            var ResultVM = this.mapper.Map<Employee, EmployeeViewModel>(Result);
           
                if(Result is null)
                return NotFound();  
            return View(ViewName, ResultVM);    
        
        }

        public async Task<IActionResult> Update(int? id) {
           return await view(id,"UPdate");
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromRoute]int id,EmployeeViewModel employeeVM)
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
                this.unitOfWork.GetRepository<Employee>().Update(employee);
                var Result =await this.unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int id) {
            var Employee =await this.unitOfWork.GetRepository<Employee>().getAsync(id);
            if (Employee is null)
                return NotFound();
            this.unitOfWork.GetRepository<Employee>().Delete(Employee);
            var Result =await this.unitOfWork.CompleteAsync();
            if (Result > 0) {
                DocumentSetting.DeleteFile("images", Employee.ImageName);
                return RedirectToAction(nameof(Index));
            }
                
            var EmployeeVM=this.mapper.Map<Employee,EmployeeViewModel>(Employee);   
            return View(EmployeeVM);
        }
       
    }
}
