using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace artfriks.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Iso { get; set; }
        public string CountryName { get; set; }
        public string NiceName { get; set; }
        public string Iso3 { get; set; }
        public Int16 Numcode { get; set; }
        public int Phonecode { get; set; }
    }
}
