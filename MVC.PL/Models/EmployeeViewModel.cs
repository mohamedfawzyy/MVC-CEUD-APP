using MVC.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace MVC.PL.Models
{
    public class EmployeeViewModel : BaseViewModel
    {
        
        [MinLength(3, ErrorMessage = "Name must be not less than 3 chars")]
        [MaxLength(50,ErrorMessage ="Name must be less than 50 characters")]
        [Required(ErrorMessage = "Name is Required!")]
        override public string Name { get; set; }
        [Range(22, 30)]
        public int? Age { get; set; }
        [RegularExpression(@"^[1-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "adress must be like this 123-street-city-country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }

        public Gender Gender { get; set; }
        [Display(Name = "Employee Type")]
        public EmpType EmpType { get; set; }

        [Display(Name ="Employee Iamge")]
        public IFormFile EmployeeImg { get; set; }
        public string ImageName { get; set; }

        [ForeignKey(nameof(Employee.Department))]
        public int? DepartmentId { get; set; }
        [InverseProperty(nameof(DAL.Models.Department.Employees))]
        public Department Department { get; set; }
    }
}
