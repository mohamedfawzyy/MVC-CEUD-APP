using System.ComponentModel.DataAnnotations;

namespace MVC.PL.Models
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage ="Password is Required")]
		[MinLength(5,ErrorMessage ="Password Can not be less than 5 chars")]
		[DataType(DataType.Password)]
        public string NewPassword { get; set; }
		[Required(ErrorMessage ="Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword),ErrorMessage ="Confirm Password Not Match Password")]
        public string ConfirmPassword { get; set; }
    }
}
