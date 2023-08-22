namespace Web.Models
{
    public class CreatePost
    {
        public string comment { get; set; }
        public string id { get; set; }
        public string idTarget { get; set; }
        public string idDd { get; set; }
        public string typeEvent { get; set; }
        public string visibility { get; set; }
        public string locationString { get; set; }
        public string locationCode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string externalType { get; set; }
        public string externalLink { get; set; }
    }
}
