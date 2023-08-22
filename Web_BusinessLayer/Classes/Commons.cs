using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Web_BusinessLayer.Classes
{
    public static class Commons
    {
        /// <summary>
        /// Gets the hash sha256.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string getHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        /// <summary>
        /// Gets the code profile.
        /// </summary>
        /// <returns></returns>
        public static string getCodeProfile()
        {
            int length = 20;
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        /// <summary>
        /// Gets the new unique identifier.
        /// </summary>
        /// <returns></returns>
        public static Guid getNewGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Serialize/Deserialize JSON  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.UTF8.GetString(ms.ToArray());
            return retVal;
        }

        /// <summary>
        /// Deserializes the specified json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

        /// <summary>
        /// Gets the alert description.
        /// </summary>
        /// <param name="typeAlert">The type alert.</param>
        /// <param name="paramRemplace">The parameter remplace.</param>
        /// <returns></returns>
        public static string getAlertDescription(int? typeAlert, string[] paramRemplace)
        {
            string retAlert = String.Empty;

            switch (typeAlert)
            {
                case 2:
                    //Friend
                    retAlert = String.Format(Web_Resource.Alert.friendSuggest, paramRemplace);
                    break;
                case 4:
                    retAlert = String.Format(Web_Resource.Alert.inviteJoinMember, paramRemplace);
                    break;
                case 5:
                    retAlert = String.Format(Web_Resource.Alert.joinMember, paramRemplace);
                    break;
                case 1:
                    //Post
                    retAlert = String.Format(Web_Resource.Alert.postFriend, paramRemplace);
                    break;
                case 6:
                    //Comment
                    retAlert = String.Format(Web_Resource.Alert.commentPost, paramRemplace);
                    break;
                case 7:
                    //Follower
                    retAlert = String.Format(Web_Resource.Alert.joinFollower, paramRemplace);
                    break;
            }
            return retAlert;
        }

        /// <summary>
        /// Gets the gallery title.
        /// </summary>
        /// <param name="typeGallery">The type gallery.</param>
        /// <returns></returns>
        public static string getGalleryTitle(string typeGallery)
        {
            string retTitle = String.Empty;

            switch (typeGallery)
            {
                case "{valGalleryWall}":
                    retTitle = Web_Resource.Value.valGalleryWall;
                    break;
                default:
                    retTitle = typeGallery;
                    break;
            }
            return retTitle;
        }

        /// <summary>
        /// Converts the date.
        /// </summary>
        /// <param name="strDate">The string date.</param>
        /// <returns></returns>
        public static DateTime convertDate(string strDate)
        {
            DateTime outputDateTimeValue;
            if (DateTime.TryParseExact(strDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputDateTimeValue))
            {
                return outputDateTimeValue;
            }
            else
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String[] getClientCountry()
        {
            String[] strReturnVal = new String[2];
            string ipResponse = Methods.IPRequestHelper("http://ip-api.com/xml/" + getIPAddress());

            //return ipResponse;
            XmlDocument ipInfoXML = new XmlDocument();
            ipInfoXML.LoadXml(ipResponse);
            XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");

            NameValueCollection dataXML = new NameValueCollection();

            dataXML.Add(responseXML.Item(0).ChildNodes[2].InnerText, responseXML.Item(0).ChildNodes[2].Value);

            strReturnVal[0] = responseXML.Item(0).ChildNodes[1].InnerText.ToString(); // Contry
            strReturnVal[1] = responseXML.Item(0).ChildNodes[2].InnerText.ToString();  // Contry Code 

            return strReturnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string getIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
