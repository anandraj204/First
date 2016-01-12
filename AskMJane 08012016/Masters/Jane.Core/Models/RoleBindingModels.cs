using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jane.Core.Models
{
    public class CreateRoleBindingModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

    }

    public class UsersInRoleModel
    {

        public int Id { get; set; }
        public List<int> EnrolledUsers { get; set; }
        public List<int> RemovedUsers { get; set; }
    }
}