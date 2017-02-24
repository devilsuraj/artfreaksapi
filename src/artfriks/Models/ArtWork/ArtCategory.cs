using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models.ArtWork
{
    public class ArtCategory
    {
        public int Id { get; set; }
        public int ArtId { get; set; }
        public int Category { get; set; }
    }
}
