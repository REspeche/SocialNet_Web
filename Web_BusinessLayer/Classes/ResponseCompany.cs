using System.Collections.Generic;

namespace Web_BusinessLayer.Classes
{
    public class ResponseCompany : ResponseMessage
    {
        public IEnumerable<CompanyItem> items { get; set; }
    }

    public class ResponseCompanySearch : ResponseMessage
    {
        public IEnumerable<CompanySearch> items { get; set; }
    }

    public class ResponseCompanyBadges : ResponseMessage
    {
        public int? countAll { get; set; }
    }

    public class CompanyItem
    {
        public long? joinId { get; set; }
        public long? companyId { get; set; }
        public string companyName { get; set; }
        public string profileGuid { get; set; }
        public string profileCode { get; set; }
        public bool? seggested { get; set; }
        public bool? following { get; set; }
        public int? countMember { get; set; }
        public bool? isMe { get; set; }

        public CompanyItem()
        {
        }
    }

    public class CompanySearch
    {
        public long? companyId { get; set; }
        public string companyName { get; set; }
        public string description { get; set; }
        public string profileGuid { get; set; }
        public bool isJoin { get; set; }

        public CompanySearch()
        {
        }
    }
}
