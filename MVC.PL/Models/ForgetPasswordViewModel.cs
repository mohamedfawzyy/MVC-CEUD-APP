using System.ComponentModel.DataAnnotations;

namespace MVC.PL.Models
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage ="Email is Required")]
		[EmailAddress(ErrorMessage ="Inavlid Email")]
        public string Email { get; set; }
    }
}
