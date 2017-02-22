using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace artfriks.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string OTP { get; set; }
        public int OTPfrom { get; set; }
    }
}
