using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.DAL.Models;
using MVC.PL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
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
        public async  Task<IActionResult> Index(string SearchEmail)
		{
			var Users = new List<UserViewModel>();
			if (string.IsNullOrEmpty(SearchEmail))
			{
				
				 Users = this.userManager.Users.Select( U=>new UserViewModel() { 
							Email = U.Email,
							FName = U.FirstName,
							LName = U.LastName,	
							Id= U.Id,
							PhoneNumber = U.PhoneNumber,	
							Roles= this.userManager.GetRolesAsync(U).Result
				}).ToList();
				
			}
			else { 
			
				var User=await this.userManager.FindByEmailAsync(SearchEmail);	
				if (User is not null) {
					var UserVM = new UserViewModel()
					{
						Email = User.Email,
						FName = User.FirstName,
						LName = User.LastName,
						Id = User.Id,
						PhoneNumber = User.PhoneNumber,
						Roles = this.userManager.GetRolesAsync(User).Result
					};
					Users=new List<UserViewModel>() {UserVM};	

				}
	
			}
			
			return View(Users);
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
	}
}
