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
    using System.Collections.Generic;
    
    public partial class ale_alert
    {
        public long ale_id { get; set; }
        public Nullable<long> ent_id { get; set; }
        public Nullable<bool> ale_send { get; set; }
        public Nullable<System.DateTime> ale_timestampSent { get; set; }
        public Nullable<long> ale_idTarget { get; set; }
        public Nullable<int> ale_typeTarget { get; set; }
        public Nullable<bool> ale_view { get; set; }
        public Nullable<System.DateTime> ale_timestampViewed { get; set; }
        public System.DateTime ale_timestampCreated { get; set; }
        public bool ale_enabled { get; set; }
    
        public virtual ent_entity ent_entity { get; set; }
    }
}
