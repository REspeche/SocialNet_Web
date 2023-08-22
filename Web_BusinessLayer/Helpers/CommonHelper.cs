using System.Configuration;
using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.Helpers
{
    public static class CommonHelper
    {
        public static string getHandlerPath()
        {
            return ConfigurationManager.AppSettings["PathHandler"].ToString();
        }

        public static string getImageWidth(Rules.imageSize type)
        {
            string wValue = string.Empty;
            switch (type)
            {
                case Rules.imageSize.Profile: wValue = ConfigurationManager.AppSettings["ImageWidth_Profile"].ToString(); break;
                case Rules.imageSize.Photo: wValue = ConfigurationManager.AppSettings["ImageWidth_Photo"].ToString(); break;
                case Rules.imageSize.PhotoFull: wValue = ConfigurationManager.AppSettings["ImageWidth_PhotoFull"].ToString(); break;
                case Rules.imageSize.Cover: wValue = ConfigurationManager.AppSettings["ImageWidth_Cover"].ToString(); break;
            }
            if (!wValue.Equals("")) wValue = "?w=" + wValue;
            return wValue;
        }
    }
}