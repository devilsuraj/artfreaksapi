using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
    public class ArtCategory
    {
        public int Id { get; set; }
        public int ArtId { get; set; }
        public int Category { get; set; }

    }
    public class CategoryChildFromStore
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string category { get; set; }
        public int Level { get; set; }
    }
}
