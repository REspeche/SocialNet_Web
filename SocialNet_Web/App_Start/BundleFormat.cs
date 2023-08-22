namespace Web.App_Start
{
    public class BundleFormat
    {
        public const string cssAsync = @"<link href=""{0}"" rel=""stylesheet"" type=""text/css"" media=""none"" onload=""if(media!='all')media='all'"" />";

        public const string jsAsync = @"<script src=""{0}"" async></script>";

        public const string jsDefer = @"<script src=""{0}"" defer></script>";
    }
}
