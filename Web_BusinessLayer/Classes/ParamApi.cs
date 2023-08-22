namespace Web_BusinessLayer.Classes
{
    public partial class ParamApi
    {
        public string user { get; set; }
        public string password { get; set; } // se usa en el login común, si es con facebook este parametro no se pasa
        public string idFB { get; set; } // “app” cuando es desde una aplicacion externa, si es login común este parametron no se pasa
    }

    public partial class ParamApiPost : ParamApi
    {
        public string text { get; set; }
        public string picture { get; set; }
        public long entityId { get; set; }
        public int anonymous { get; set; }
    }
}
