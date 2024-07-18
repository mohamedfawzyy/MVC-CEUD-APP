using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using System;

namespace MVC.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IWebHostEnvironment env;

        public DepartmentController(IDepartmentRepository departmentRepository, IWebHostEnvironment env)
        {
            this.departmentRepository = departmentRepository;
            this.env = env;
        }
        public IActionResult Index()
        {
            var Departments=this.departmentRepository.getAll();
            return View(Departments);
        }

        public IActionResult create() { 
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult create(Department department) {
                if(ModelState.IsValid)
            {
                var result = this.departmentRepository.Add(department);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
           
            }
            return View(department);
        }

        public IActionResult view(int? id,string ViewName="view") {
            if (id.HasValue)
            {
                var Result = this.departmentRepository.get(id.Value);
                if (Result == null) {
                    return NotFound();
                }
                return View(ViewName,Result);
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
        public IActionResult Update([FromRoute]int id,Department department) {
            if (!ModelState.IsValid)
                return View(department);
            if(id != department.Id)
                return BadRequest();
            try {
                var Result = this.departmentRepository.Update(department);
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
           
            return View(department);
              
        }
        [HttpPost]
        public IActionResult Delete(int id) {
            var department=this.departmentRepository.get(id);
            if (department == null) 
                return BadRequest();
            try
            {
                this.departmentRepository.Delete(department);
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
            return View();  

        }
    }
}
