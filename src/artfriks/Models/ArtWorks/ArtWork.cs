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
        public string Orientation { get; set; }
        public string DimensionUnit { get; set; }
        public string MediumString { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ArtCreationDate { get; set; }
        public bool TermAccepted { get; set; }
        public int Views { get; set; }
        public int Status { get; set; } // 0 for new , 1 for approved , 2 for rejected , 3 for deleted
        public string Category { get; set; } // 0 for normal - 1 for deals - 2 for anything else
    }

    public class ArtWorkView
    {
        public ArtWork artwork { get; set; }
        public string user { get; set; }
        public int favcount { get; set; }
        public bool isfav { get; set; }
        public IEnumerable<ArtTag> tags { get; set; }
        public IEnumerable< ARtKeywords> keywords { get; set; }
    }
    public class ArtWorkViewWithTag
    {
        public ArtWork art { get; set; }
        public string UserId { get; set; }
        public int favcount { get; set; }
        public bool isfav { get; set; }
        public IEnumerable<ArtTagView> tags { get; set; }
    }
    public class artsview
    {
        public IEnumerable<ArtWorkViewWithTag> arts { get; set; }
    }

    public class ArtTagView {
        public ArtTag Tags { get; set; }
    }
    public class ArtWorkEditView
    {
        public ArtWork Artwork { get; set; }
        public string User { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Medium> Medium { get; set; }
        public IEnumerable<Type> Types { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<ArtWithTagsView> Tagset { get; set; }
        public IEnumerable<ArtTag> Tags { get; set; }
        public string Tag { get; set; }
    }
    public class PostTags
    {
      public string artId { get; set; }
      public string tagId { get; set; }
    }
}
