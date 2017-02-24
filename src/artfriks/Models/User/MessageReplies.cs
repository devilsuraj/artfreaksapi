using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace artfriks.Models.User
{
    public class MessageReplies
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string UserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
