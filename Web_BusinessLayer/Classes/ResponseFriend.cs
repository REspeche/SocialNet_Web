using System.Collections.Generic;

namespace Web_BusinessLayer.Classes
{
    public class ResponseFriend : ResponseMessage
    {
        public IEnumerable<FriendItem> items { get; set; }
    }

    public class ResponseFriendSearch : ResponseMessage
    {
        public IEnumerable<FriendSearch> items { get; set; }
    }

    public class ResponseFriendBadges : ResponseMessage
    {
        public int? countAll { get; set; }

        public int? countSend { get; set; }

        public int? countReceive { get; set; }
    }

    public class FriendItem
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

        public FriendItem()
        {
        }
    }

    public class FriendSearch
    {
        public long? userId { get; set; }
        public string userName { get; set; }
        public string description { get; set; }
        public string profileGuid { get; set; }
        public string userProfileCode { get; set; }
        public bool isFriend { get; set; }

        public FriendSearch()
        {
        }
    }
}
