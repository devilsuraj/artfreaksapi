using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models.AccountViewModels
{

    public class ChangePasswordViewModel
    {
        [Required]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string code { get; set; }

    }

    public class UpdateProfileModel
    {
        [Required]
        public string username { get; set; }
        public string fullame { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string oldpassword { get; set; }
        public string Email { get; set; }
    }
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string Address { get; set; }
        public string UserName { get; set; }
        public string OTP { get; set; }
        public int OTPfrom { get; set; }
        public string Role { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
