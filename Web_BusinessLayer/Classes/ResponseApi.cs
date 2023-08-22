using System.Collections.Generic;
using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.Classes
{
    public class ResponseApi
    {
        public Api.Result result { get; set; }

        public ResponseApi()
        {
            this.result = Api.Result.Bad;
        }
    }

    public class ResponseApiListEntities : ResponseApi
    {
        public IEnumerable<EntityItem> items { get; set; }

        public ResponseApiListEntities()
        {
            items = new EntityItem[] { };
        }
    }

    public class EntityItem
    {
        public long entityId { get; set; }
        public string description { get; set; }
        public int? type { get; set; }
    }
}
