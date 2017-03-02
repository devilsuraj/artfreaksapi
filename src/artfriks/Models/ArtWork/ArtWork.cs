using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
    public class ArtWork
    {
        public int Id { get; set; }
        public string PictureUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public decimal Price { get; set; }
        public decimal Width { get; set; } 
        public decimal Height { get; set; }
        public string DimensionUnit { get; set; }
        public string MediumString { get; set; }
        public DateTime AddedDate { get; set; }
        public bool TermAccepted { get; set; }
        public int Status { get; set; } // 0 for new , 1 for approved , 2 for rejected , 3 for deleted
        public string Category { get; set; } // 0 for normal - 1 for deals - 2 for anything else
    }
}
