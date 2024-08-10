using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVC.DAL.Models;
using MVC.PL.Models;
using MVC.PL.Services.SendEmailService;
using System.Threading.Tasks;

namespace MVC.PL.Controllers
{
    public class AcountController : Controller
    {
		private readonly IConfiguration configuration;
		private readonly ISenderEmail senderEmail;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AcountController(
            IConfiguration configuration,
            ISenderEmail senderEmail,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
			this.configuration = configuration;
			this.senderEmail = senderEmail;
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel) 
        {

            if (ModelState.IsValid) 
            {
               var User=await this.userManager.FindByNameAsync(viewModel.UserName);
                if (User == null)
                {
                    User = new ApplicationUser()
                    {
                        FirstName = viewModel.Fname,
                        LastName = viewModel.Lname,
                        UserName = viewModel.UserName,
                        Email = viewModel.Email,
                        PhoneNumber = viewModel.PhoneNumber,
                        IsAgree = viewModel.IsAgree,
                    };
                    var Result=await this.userManager.CreateAsync(User,viewModel.Password);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                    else 
                    {
                        foreach (var error in Result.Errors)
                        {
							ModelState.AddModelError(string.Empty, error.Description);
						}
						return View(viewModel);
					}
                }
                ModelState.AddModelError(string.Empty, "This user Name has been selected for another user");
            }
            return View(viewModel);  
        }

        [HttpGet]
        public IActionResult SignIn() { 
        
        
        return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel ViewModel) 
        {
            if (ModelState.IsValid) {

                var User =await this.userManager.FindByEmailAsync(ViewModel.Email);
                if (User is not null) {
                    var Flag=await this.userManager.CheckPasswordAsync(User,ViewModel.Password);
                    if (Flag) {
                        var Result =await this.signInManager.PasswordSignInAsync(User, ViewModel.Password, ViewModel.RememberMe, false);
                        if (Result.IsLockedOut)
                            ModelState.AddModelError(string.Empty, "You are Locked for 5 mins");
                        if (Result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index),"Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");
            
            }
            return View(ViewModel);  
        }

        [HttpGet]
        public async new Task<IActionResult> SignOut() 
        {
           await this.signInManager.SignOutAsync();

            return RedirectToAction(nameof(SignIn));
        }
        [HttpGet]
        public IActionResult ForgetPassword() 
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel modelVM) {
        
            if (ModelState.IsValid)
            {
                var User=await this.userManager.FindByEmailAsync(modelVM.Email);  
                if (User != null) { 
                    var Token =await this.userManager.GeneratePasswordResetTokenAsync(User);
                    var Body = Url.Action("ResetPassword", "Acount", new { Email = User.Email, Token = Token },"Https");
                    await this.senderEmail.SendAsync(
                        From: this.configuration["EmailSetting:EmailUser"],
                        Recipients: User.Email,
                        Subject: "Reset Your Password",
                        Body: Body);

                        return RedirectToAction(nameof(CheckYourInbox));
				}
                ModelState.AddModelError(string.Empty, "There is User for this Email");
            }

            return View(modelVM);

        }

        [HttpGet]
        public IActionResult CheckYourInbox() {
        return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string Email, string Token)
        {
            TempData["Email"] = Email;
            TempData["Token"] = Token;
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> ResetPassword(ResetPasswordViewModel modelVM)
        {
            string Email = TempData["Email"] as string;
			string Token = TempData["Token"] as string;
			if (ModelState.IsValid)
            {
                var User=await this.userManager.FindByEmailAsync(Email);  
                if (User is not  null)
                {
                    var Result =await this.userManager.ResetPasswordAsync(User, Token, modelVM.NewPassword);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                    else {

                        foreach (var error in Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);  
                        }
                    }
                }
            }
            return View(modelVM);

        }
    }
}
