using System.Collections.Generic;

namespace Web_BusinessLayer.Classes
{
    public class ResponseAlert: ResponseMessage
    {
        public IEnumerable<AlertItem> items { get; set; }
    }

    public class AlertItem
    {
        public long id { get; set; }
        public long? userId { get; set; }
        public string userProfileCode { get; set; }
        public string profileGuid { get; set; }
        public int? type { get; set; }
        public int? date { get; set; }
        public string description { get; set; }
        public bool? view { get; set; }

        public AlertItem()
        {
        }
    }
}
