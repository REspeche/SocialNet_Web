using System.Collections.Generic;

namespace Web_BusinessLayer.Classes
{
    public class ResponseComment : ResponseMessage
    {
        public IEnumerable<CommentItem> items { get; set; }
    }
}