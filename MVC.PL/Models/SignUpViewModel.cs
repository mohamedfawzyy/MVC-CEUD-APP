using System.ComponentModel.DataAnnotations;

namespace MVC.PL.Models
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="First Name is Required")]
		[Display(Name ="First Name")]
        public string Fname { get; set; }
		[Required(ErrorMessage = "Last Name is Required")]
		[Display(Name = "Last Name")]
		public string Lname { get; set; }
        [Required(ErrorMessage ="user name is required")]
        public string UserName { get; set; }
		[Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage ="Password is required")]
		[MinLength(5,ErrorMessage ="Minumim password length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Compare(nameof(Password),ErrorMessage ="Confirm Password doesn't match Password")]
        public string ConfirmPassword { get; set; }
		[Required(ErrorMessage ="Phone number is required!!")]
		[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public bool IsAgree { get; set; }


    }
}
