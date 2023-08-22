namespace Web_BusinessLayer.Classes
{
    public class ResponseProfile : ResponseMessage
    {
        public ProfileItem item { get; set; }
    }

    public class ProfileItem
    {
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int? gender { get; set; }
        public string genderLabel { get; set; }
        public string birthdate { get; set; }
        public string birthdateLabel { get; set; }
        public long? language { get; set; }
        public string languageLabel { get; set; }
        public long? country { get; set; }
        public string countryLabel { get; set; }
        //Photos
        public string photoProfile { get; set; }
        public string photoCover { get; set; }
        public bool canPost { get; set; }
    }
}
