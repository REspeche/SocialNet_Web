//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_BusinessLayer
{
    using System;
    
    public partial class SP_GET_GROUPS_Result
    {
        public long joinId { get; set; }
        public long groupId { get; set; }
        public string groupName { get; set; }
        public string groupProfileCode { get; set; }
        public string profileGuid { get; set; }
        public Nullable<bool> rel_suggested { get; set; }
        public Nullable<bool> rel_accepted { get; set; }
        public Nullable<bool> rel_rejected { get; set; }
        public Nullable<bool> rel_following { get; set; }
        public string typeAction { get; set; }
        public Nullable<int> countMember { get; set; }
        public int isMe { get; set; }
    }
}
