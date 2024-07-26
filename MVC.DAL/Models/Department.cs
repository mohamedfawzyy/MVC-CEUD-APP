using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        override public string Name { get; set; }
        [DisplayName("Date of Creation")]
        public DateTime DateOfCreation { get; set; }
        [InverseProperty(nameof(Employee.Department))]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
