using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PictureUrl { get; set; }
        public string BioData { get; set; }
        public string UserBrief { get; set; }
        public DateTime UpdateDate { get; set; }
    }

 
}
