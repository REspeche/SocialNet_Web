using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.Classes
{
    public class ResponseMessage
    {
        public int code { get; set; }
        public string message { get; set; }
        public Rules.actionMessage action { get; set; }

        public ResponseMessage()
        {
            this.code = 0;
            this.message = string.Empty;
            this.action = Rules.actionMessage.None;
        }
    }
}
