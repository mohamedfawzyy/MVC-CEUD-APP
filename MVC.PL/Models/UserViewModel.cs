using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.PL.Models
{
	public class UserViewModel
	{
	
		public string Id { get; set; }
	
		public string FName { get; set; }
		
		public string LName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
	
		[DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
	}

}
