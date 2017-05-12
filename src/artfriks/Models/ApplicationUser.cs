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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string FullName { get; set; }
        public string Profession { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string OTP { get; set; }
        public int OTPfrom { get; set; }
        public string FormattedAddress { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public DateTime Adddate { get; set; }
        public ApplicationUser() { this.Adddate = DateTime.Now; }
    }

    public class homesection
    {
        public int Id { get; set; }
        public string Sectiontype { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Title2 { get; set; }
        public string Description { get; set; }
        public string TextonButton { get; set; }
    }

    public class Featured
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
    }

    public class Styles
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
    }

    public class Catgoryhomesection
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
    }
}
