using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
    public class ArtArticles
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Article { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime PublishTime { get; set; }
        public bool IsPublished { get; set; }
        public string Author { get; set; }
    }

    public class ArtilceTags
    {
        public int ArticleId {get;set;}
        public int TagId {get;set;}
    }
}
