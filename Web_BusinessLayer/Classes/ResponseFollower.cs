using System.Collections.Generic;

namespace Web_BusinessLayer.Classes
{
    public class ResponseFollower : ResponseMessage
    {
        public IEnumerable<FollowerItem> items { get; set; }
    }

    public class FollowerItem
    {
        public long friendId { get; set; }
        public long? userId { get; set; }
        public string userName { get; set; }
        public string userProfileCode { get; set; }
        public string profileGuid { get; set; }
        public bool? seggested { get; set; }
        public bool? offered { get; set; }
        public bool? following { get; set; }
        public bool? blocked { get; set; }
        public bool? isFriend { get; set; }

        public FollowerItem()
        {
        }
    }
}
