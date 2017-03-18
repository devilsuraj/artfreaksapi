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
        [Required]
        public string FormattedAddress { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string UserName { get; set; }
        public string OTP { get; set; }
        public int OTPfrom { get; set; }
        public string Role { get; set; }
        public string Profession { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
