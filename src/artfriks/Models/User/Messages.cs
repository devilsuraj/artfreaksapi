using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string ToUserId { get; set; }
        public int ArtId { get; set; }
        public string FromUserId { get; set; }
        public string Message { get; set; }
        public DateTime AddedDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
    }
}
