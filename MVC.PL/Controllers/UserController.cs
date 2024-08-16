using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.DAL.Models;
using MVC.PL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
	[Authorize(Roles ="Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly SignInManager<ApplicationUser> signInManager;

		public UserController(UserManager<ApplicationUser> userManager ,
			IMapper mapper
			,SignInManager<ApplicationUser> signInManager)
        {
			this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
		}
        public async  Task<IActionResult> Index()
		{
			var Users = new List<UserViewModel>();
			
				 Users =await this.userManager.Users.Select( U=>new UserViewModel() { 
							Email = U.Email,
							FName = U.FirstName,
							LName = U.LastName,	
							Id= U.Id,
							PhoneNumber = U.PhoneNumber,	
							Roles= this.userManager.GetRolesAsync(U).Result
				}).ToListAsync();
				
		
			return View(Users);
		}
		[HttpGet]
		public async Task<IActionResult> Search(string SearchInput)
		{
            var Users = new List<UserViewModel>();
			if (string.IsNullOrEmpty(SearchInput))
			{
                Users = await this.userManager.Users.Select(U => new UserViewModel()
                {
                    Email = U.Email,
                    FName = U.FirstName,
                    LName = U.LastName,
                    Id = U.Id,
                    PhoneNumber = U.PhoneNumber,
                    Roles = this.userManager.GetRolesAsync(U).Result
                }).ToListAsync();
				return PartialView(Users);
            }
			Users = await this.userManager.Users.Where(U => U.NormalizedEmail.Contains(SearchInput)).Select(U => new UserViewModel() {
				Email = U.Email,
				FName = U.FirstName,
				LName = U.LastName,
				Id = U.Id,
				PhoneNumber = U.PhoneNumber,
				Roles = this.userManager.GetRolesAsync(U).Result
			}).ToListAsync();
		
			return PartialView(Users);
		}

        public async  Task<IActionResult> Detailes(string id,string ViewName="Detailes") {

			if (string.IsNullOrEmpty(id)) {
				return BadRequest();			
			}
			var User = await this.userManager.FindByIdAsync(id);
			if (User is not null) {
				var UserVM=this.mapper.Map<ApplicationUser,UserViewModel>(User);
				return View(ViewName,UserVM);	
			}
			return BadRequest();
		}
		[HttpGet]
		public async Task<IActionResult> Update(string id) {
		
			return await Detailes(id,"Update");
		}

		[HttpPost]
		public async Task<IActionResult> Update([FromRoute]string id, UserViewModel UserVM) {
		
			if (ModelState.IsValid)
			{
				var User =await this.userManager.FindByIdAsync(id);
				User.PhoneNumber = UserVM.PhoneNumber;
				User.LastName = UserVM.LName;
				User.FirstName = UserVM.FName;
			
				var Result=await this.userManager.UpdateAsync(User);
				if (Result.Succeeded)
				{
					return RedirectToAction(nameof(Index));
				}
				else {

                    foreach (var Error in Result.Errors)
                    {
						ModelState.AddModelError("", Error.Description);
                    }
                }
					
				
			}
			return View(UserVM);
		
		
		}

		[HttpPost]
		public async  Task<IActionResult> Delete(string id) { 
			if(string.IsNullOrEmpty(id))
				return BadRequest();
			var User =await this.userManager.FindByIdAsync(id);
			if (User != null)
			{
				var Result=await this.userManager.DeleteAsync(User);
				if (Result.Succeeded)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return await Detailes(id,ViewName:"Delete");
		}
	
	}
}
