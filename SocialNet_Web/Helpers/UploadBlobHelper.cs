using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.Enum;
using Web_Resource;

namespace Web.Helpers
{
    public static class UploadBlobHelper
    {
        public static ReturnUpload UploadFiles(HttpFileCollectionBase files)
        {
            ReturnUpload response = new ReturnUpload();
            try
            {
                if (files.Count > 0)
                {
                    foreach (string upload in files)
                    {
                        if (!(files[upload] != null && files[upload].ContentLength > 0)) continue;
                        HttpPostedFileBase fileUpload = files[upload];

                        if (fileUpload == null)
                        {
                            response.code = 1;
                            response.message = Message.errorUploadFileNotFound;
                        }
                        else if (fileUpload.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 5; //5 MB
                            string[] AllowedFileExtensions = new string[] { "jpg", "jpeg", "gif", "png" };
                            string _fileExtension = _fileExtension = fileUpload.FileName.ToLower().Substring(fileUpload.FileName.ToLower().LastIndexOf('.') + 1);
                            if (_fileExtension.Equals("blob")) _fileExtension = fileUpload.ContentType.ToLower().Substring(fileUpload.ContentType.ToLower().LastIndexOf('/') + 1);

                            if (!AllowedFileExtensions.Contains(_fileExtension))
                            {
                                response.code = 2;
                                response.message = String.Format(Message.errorUploadWrongFormat, string.Join(", ", AllowedFileExtensions));
                            }

                            else if (fileUpload.ContentLength > MaxContentLength)
                            {
                                response.code = 3;
                                response.message = String.Format(Message.errorUploadSizeLong, String.Concat(MaxContentLength, " MB"));
                            }
                            else
                            {
                                //Calculate image width / height
                                int _fileWidth = 0, _fileHeight = 0;
                                using (Image tif = Image.FromStream(
                                    stream: fileUpload.InputStream,
                                    useEmbeddedColorManagement: false,
                                    validateImageData: false))
                                {
                                    _fileWidth = Convert.ToInt32(tif.PhysicalDimension.Width);
                                    _fileHeight = Convert.ToInt32(tif.PhysicalDimension.Height);
                                }
                                fileUpload.InputStream.Position = 0;

                                JavaScriptSerializer js = new JavaScriptSerializer();

                                Guid _newGuid = Commons.getNewGuid();
                                response.code = UploadBlobHelper.UploadFileToBlobStorage(
                                    String.Concat(_newGuid.ToString().ToUpper(), ".", _fileExtension),
                                    fileUpload.InputStream);

                                if (response.code == 0)
                                {
                                    response.files.Add(new FileUpload
                                    {
                                        newGuid = _newGuid,
                                        fileExtension = _fileExtension,
                                        fileWidth = _fileWidth,
                                        fileHeight = _fileHeight
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    response.code = 1;
                    response.message = Message.errorUploadFileNotFound;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.UtcNow);
                response.code = -1;
                response.message = ex.Message;
            }

            return response;
        }

        public static int UploadFileToBlobStorage(string blockBlogName, Stream fileStream)
        {
            string containerName = "images";
            int returnValue = 0;
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve a reference to a container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();

                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blockBlogName);

                blockBlob.UploadFromStream(fileStream);
            }
            catch (Exception)
            {
                returnValue = -1;
            }
            return returnValue;
        }
    }

    public class ReturnUpload : ResponseMessage
    {
        public List<FileUpload> files { get; set; }

        public ReturnUpload()
        {
            files = new List<FileUpload>();
        }
    }

    public class FileUpload
    {
        public Guid newGuid { get; set; }
        public string fileExtension { get; set; }
        public int fileWidth { get; set; }
        public int fileHeight { get; set; }
    }
}