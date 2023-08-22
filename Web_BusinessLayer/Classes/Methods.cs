using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace Web_BusinessLayer.Classes
{
    public static class Methods
    {
        /// <summary>
        /// Images to base64.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ImageToBase64(this Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        /// <summary>
        /// Base64s to image.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns></returns>
        public static Image Base64ToImage(this string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static Stream ToStream(this Image img, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            img.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Gets the image format.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns></returns>
        public static String GetImageFormat(this Image img)
        {
            if (img.RawFormat.Equals(ImageFormat.Jpeg))
                return ImageFormat.Jpeg.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.Bmp))
                return ImageFormat.Bmp.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.Png))
                return ImageFormat.Png.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.Emf))
                return ImageFormat.Emf.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.Exif))
                return ImageFormat.Exif.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.Gif))
                return ImageFormat.Gif.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.Icon))
                return ImageFormat.Icon.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.MemoryBmp))
                return ImageFormat.MemoryBmp.ToString().ToLower();
            if (img.RawFormat.Equals(ImageFormat.Tiff))
                return ImageFormat.Tiff.ToString().ToLower();
            else
                return ImageFormat.Wmf.ToString().ToLower();
        }

        public static string IPRequestHelper(string url)
        {

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            responseStream.Close();
            responseStream.Dispose();

            return responseRead;
        }
    }
}
