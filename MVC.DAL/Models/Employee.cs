using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Models
{
    public enum Gender {
            [EnumMember(Value ="Male")]
            Male=1,
            [EnumMember(Value ="Female")]
            Female=2
        }
    public enum EmpType {
        [EnumMember(Value ="Full Time")]
        FullTime=1,
        [EnumMember(Value ="Part Time")]
        PartTime=2
    }
    public class Employee :BaseModel
    {
      
        [MinLength(3,ErrorMessage ="Name must be not less than 3 chars")]
        [Required(ErrorMessage ="Name is Required!")]
        public string Name { get; set; }
        [Range(22,30)]
        public int? Age { get; set; }
        [RegularExpression(@"^[1-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage ="adress must be like this 123-street-city-country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name ="Is Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name="Hiring Date")]
        public DateTime HiringDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
        public Gender Gender { get; set; }
        [Display(Name ="Employee Type")]
        public EmpType EmpType { get; set; }

    }
}
