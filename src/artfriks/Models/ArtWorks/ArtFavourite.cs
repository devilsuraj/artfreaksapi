using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
    public class ArtFavourite
    {
        public int Id { get; set; }
        public int ArtId { get; set; }
        public string UserId { get; set; }
    }
}
