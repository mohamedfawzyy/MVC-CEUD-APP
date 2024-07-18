using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Models
{
    public class Department : BaseModel
    {
        

    
        [Required(ErrorMessage ="Code is Required!!")]
        public string Code { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [DisplayName("Date of Creation")]
        public DateTime DateOfCreation { get; set; }
    }
}
