using System.Collections.Generic;

namespace Web_BusinessLayer.Classes
{
    public class ResponseItemCombo : ResponseMessage
    {
        public IEnumerable<ComboItem> items { get; set; }
    }

    public class ComboItem
    {
        public long id { get; set; }
        public string label { get; set; }

        public ComboItem()
        {
        }
    }
}
