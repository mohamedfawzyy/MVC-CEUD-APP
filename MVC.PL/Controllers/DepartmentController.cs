using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using MVC.PL.Models;
using System;
using System.Collections.Generic;

namespace MVC.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;

        public DepartmentController(IMapper mapper,IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.env = env;
        }
        public IActionResult Index()
        {
            var Departments=this.unitOfWork.GetRepository<Department>().getAll();
            var DepartmentsVM = this.mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(Departments);
            return View(DepartmentsVM);
        }

        public IActionResult create() { 
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult create(DepartmentViewModel departmentVM) {
                if(ModelState.IsValid)
            {
                var department = this.mapper.Map<DepartmentViewModel, Department>(departmentVM);
                var result = this.unitOfWork.GetRepository<Department>().Add(department);
                if (result > 0)
                {
                    TempData["Message"] = "Your Department Created Successfully";
                }
                else {

                    TempData["Message"] = "sorry your Department can't be created :(";

                }
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        
        public IActionResult view(int? id,string ViewName="view") {
            if (id.HasValue)
            {
                var Result = this.unitOfWork.GetRepository<Department>().get(id.Value);
                var REsultVM = this.mapper.Map<Department, DepartmentViewModel>(Result);
                if (REsultVM == null) {
                    return NotFound();
                }
                return View(ViewName, REsultVM);
            }

            return BadRequest(new
            {
                statusCode = 400,
                Message = "Bad Request"
            });
              
        }

        public IActionResult Update(int? id) {
            return view(id, "Update");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update([FromRoute]int id,DepartmentViewModel departmentVM) {
            if (!ModelState.IsValid)
                return View(departmentVM);
            if(id != departmentVM.Id)
                return BadRequest();
            try {
                var department = this.mapper.Map<DepartmentViewModel, Department>(departmentVM);
                var Result = this.unitOfWork.GetRepository<Department>().Update(department);
                if (Result > 0)
                {
                    return RedirectToAction("Index");
                }
            } catch (Exception ex) {

                if (this.env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else {
                    ModelState.AddModelError(string.Empty, "Error occurs while you updating your data");
                }

            }
           
            return View(departmentVM);
              
        }
        [HttpPost]
        public IActionResult Delete(int id) {
            var department=this.unitOfWork.GetRepository<Department>().get(id);
            var departmentVM = this.mapper.Map<Department, DepartmentViewModel>(department);
            if (department == null) 
                return BadRequest();
            try
            {
                this.unitOfWork.GetRepository<Department>().Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (this.env.IsDevelopment())
                {

                    ModelState.AddModelError("", ex.Message);
                }
                else {
                    ModelState.AddModelError("", "error while deleting your Department");
                }
            }
            return View(departmentVM);  

        }
    }
}
