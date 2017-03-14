using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
   
    public class ArtWithTags
    {
        public int Id { get; set; }
        public int ArtId { get; set; }
        public int TagId { get; set; }

    }
    public class ArtWithTagsView
    {
        public int ArtId { get; set; }
        public int TagId { get; set; }
        public string Tag { get; set; }
    }

}
