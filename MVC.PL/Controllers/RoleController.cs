using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MVC.DAL.Models;
using MVC.PL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MVC.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , IMapper mapper,
                            UserManager<ApplicationUser> userManager
            )
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Roles=await this.roleManager.Roles.Select(R=>new RoleViewModel() { 
                    Id = R.Id,
                     RoleName = R.Name
            }).ToListAsync();   
            return View(Roles);
        }
        [HttpGet]
        public IActionResult Create() {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel Model) {

            if (Model == null)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var Role=new IdentityRole() { 
                    Id=Model.Id,
                    Name =Model.RoleName
                };
              var Result= await this.roleManager.CreateAsync(Role);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
            }
            return View(Model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id, string ViewName = "Details") { 
            
            if(string.IsNullOrEmpty(id))
                return BadRequest("Error");
            var Role =await this.roleManager.FindByIdAsync(id);
            var RoleVM = this.mapper.Map<IdentityRole, RoleViewModel>(Role);
            if (Role == null)
                return NotFound();  
            return View(ViewName,RoleVM);    
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id) {

            return await Details(id, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromRoute]string id,RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid) {
                var Role = await this.roleManager.FindByIdAsync(id);
                if (Role is not null) {
                    Role.Name = roleViewModel.RoleName;
                   var Result= await this.roleManager.UpdateAsync(Role);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else {
                        ModelState.AddModelError("", "Error while updating Role");
                    }
                }
            }
            return View(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var Role=await this.roleManager.FindByIdAsync(id);
            if(Role is not null)
            {
                var RoleVM = this.mapper.Map<IdentityRole, RoleViewModel>(Role);
              var Result=  await this.roleManager.DeleteAsync(Role);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else {
                    ModelState.AddModelError("", "Error while Delete Role");
                }
                return View(RoleVM);
            }
            return NotFound();  
            
        }

        [HttpGet]
        public async  Task<IActionResult> ManageUserRoles(string RoleId) { 
            var Role=await this.roleManager.FindByIdAsync(RoleId);
            if (Role is null)
                    return NotFound();
            ViewData["RoleName"] = Role.Name;
            var Users=await this.userManager.Users.ToListAsync();
            var UsersInRolesViewModel = new List<UserInRolesViewModel>();
            foreach (var user in Users) { 
                var UserInViewModel=new UserInRolesViewModel() { 
                        UserId = user.Id,
                        UserName = user.UserName,
                };
                if (await this.userManager.IsInRoleAsync(user, Role.Name))
                {
                    UserInViewModel.IsRolled = true;
                }
                else { 
                    UserInViewModel.IsRolled=false;
                }
                UsersInRolesViewModel.Add(UserInViewModel);
            }

            return View(UsersInRolesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles (List<UserInRolesViewModel> usersInRolesViewModels , string RoleName) {
            var Role=await this.roleManager.FindByNameAsync(RoleName);
            if (Role is null) return NotFound();    
            foreach (var UserInRolesVM in usersInRolesViewModels)
            {
                var User = await this.userManager.FindByIdAsync(UserInRolesVM.UserId);
                if (User is null) 
                    return View(usersInRolesViewModels);
                if (UserInRolesVM.IsRolled && !await this.userManager.IsInRoleAsync(User,Role.Name)) {
                  var Result= await this.userManager.AddToRoleAsync(User, Role.Name);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else {
                        ModelState.AddModelError("", "Error while assigning users to Role");
                    }

                }
            }
            return View(usersInRolesViewModels);
        }
    }
}
