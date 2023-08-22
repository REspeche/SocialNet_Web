namespace Web_BusinessLayer.Classes
{
    public class ResponseLogin : ResponseMessage
    {
        public long userId { get; set; }
        public string userName { get; set; }
        public string userCode { get; set; }
        public string photoProfile { get; set; }
        public string photoCover { get; set; }
        public int? typeEntity { get; set; }

        public string locale { get; set; }

        public long view_userId { get; set; }
        public string view_userName { get; set; }
        public string view_userCode { get; set; }
        public string view_photoProfile { get; set; }
        public string view_photoCover { get; set; }
        public int? view_typeEntity { get; set; }
        public long? view_targetId { get; set; }

        public int view_isFriend { get; set; }
        public int view_isMember { get; set; }
        public int view_isFollow { get; set; }
        public int view_canPost { get; set; }
    }
}
