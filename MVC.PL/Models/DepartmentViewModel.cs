using MVC.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace MVC.PL.Models
{
    public class DepartmentViewModel : BaseViewModel
    {
        

        [Required(ErrorMessage = "Code is Required!!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is required")]
        override public  string Name { get; set; }
        [DisplayName("Date of Creation")]
        public DateTime DateOfCreation { get; set; }
        [InverseProperty(nameof(Employee.Department))]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
