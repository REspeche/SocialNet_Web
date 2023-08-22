namespace Web_BusinessLayer.Classes 
{
    public class ResponseBadges : ResponseMessage
    {
        public int? all { get; set; }
        public int? friend { get; set; }
        public int? group { get; set; }
        public int? member { get; set; }
        public int? follower { get; set; }
    }
}
