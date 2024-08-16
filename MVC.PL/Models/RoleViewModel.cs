using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.PL.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }
        public RoleViewModel()
        {
            this.Id=Guid.NewGuid().ToString();
        }

    }
}
