using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Skeleton.Presentation.Models.AccountViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The value of {0} must contain not less than {2} characters.", MinimumLength = 4)]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The value of {0} must contain not less than {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The value of {0} must contain not less than {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage="Passwords are not equal")]
        public string RePassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid address")]
        [StringLength(50, ErrorMessage = "The value of {0} must contain not less than {2} characters.", MinimumLength = 6)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}