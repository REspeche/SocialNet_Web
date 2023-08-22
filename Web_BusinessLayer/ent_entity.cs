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
    
    public partial class ent_entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ent_entity()
        {
            this.ale_alert = new HashSet<ale_alert>();
            this.lik_like = new HashSet<lik_like>();
            this.rel_relationship = new HashSet<rel_relationship>();
            this.rel_relationship1 = new HashSet<rel_relationship>();
            this.rel_relationship2 = new HashSet<rel_relationship>();
        }
    
        public long ent_id { get; set; }
        public Nullable<long> ent_idTarget { get; set; }
        public string ent_email { get; set; }
        public string ent_password { get; set; }
        public string ent_firstName { get; set; }
        public string ent_lastName { get; set; }
        public Nullable<long> evn_idProfile { get; set; }
        public Nullable<long> evn_idCover { get; set; }
        public Nullable<int> ent_gender { get; set; }
        public Nullable<System.DateTime> ent_birthdate { get; set; }
        public Nullable<bool> ent_close { get; set; }
        public Nullable<bool> ent_active { get; set; }
        public Nullable<long> cou_id { get; set; }
        public Nullable<long> lan_id { get; set; }
        public Nullable<int> ent_typeEntity { get; set; }
        public Nullable<System.DateTimeOffset> ent_timeZone { get; set; }
        public Nullable<System.DateTime> ent_timestampModified { get; set; }
        public System.DateTime ent_timestampCreated { get; set; }
        public bool ent_enabled { get; set; }
        public string ent_hash { get; set; }
        public string ent_profileName { get; set; }
        public string ent_profileCode { get; set; }
        public string ent_idFacebook { get; set; }
        public bool ent_canPost { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ale_alert> ale_alert { get; set; }
        public virtual lan_language lan_language { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<lik_like> lik_like { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rel_relationship> rel_relationship { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rel_relationship> rel_relationship1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rel_relationship> rel_relationship2 { get; set; }
    }
}
