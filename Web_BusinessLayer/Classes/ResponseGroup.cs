using System.Collections.Generic;

namespace Web_BusinessLayer.Classes
{
    public class ResponseGroup : ResponseMessage
    {
        public IEnumerable<GroupItem> items { get; set; }
    }

    public class ResponseGroupSearch : ResponseMessage
    {
        public IEnumerable<GroupSearch> items { get; set; }
    }

    public class ResponseGroupBadges : ResponseMessage
    {
        public int? countAll { get; set; }

        public int? countSend { get; set; }

        public int? countReceive { get; set; }
    }

    public class GroupItem
    {
        public long? joinId { get; set; }
        public long? groupId { get; set; }
        public string groupName { get; set; }
        public string profileGuid { get; set; }
        public string profileCode { get; set; }
        public bool? seggested { get; set; }
        public bool? following { get; set; }
        public int? countMember { get; set; }
        public bool? isMe { get; set; }

        public GroupItem()
        {
        }
    }

    public class GroupSearch
{
        public long? groupId { get; set; }
        public string groupName { get; set; }
        public string description { get; set; }
        public string profileGuid { get; set; }
        public bool isJoin { get; set; }

        public GroupSearch()
        {
        }
    }
}
