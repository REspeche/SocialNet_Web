using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Helpers;
using Web.Models;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.DTO;
using Web_BusinessLayer.Enum;
using Web_Resource;

namespace Web.Controllers
{
    [RoutePrefix("")]
    public class WallController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [CompressFilter(Order = 1)]
        public ActionResult Index()
        {
            if (ViewBag.viewOtherUser == null) Session["view_userId"] = 0;
            return View();
        }

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="view_userId">The view_user identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetPosts(
            int lastId
        )
        {
            int pageSize = int.Parse((Request.Browser.IsMobileDevice) ?
                ConfigurationManager.AppSettings["WallPageSize.mobile"].ToString() :
                ConfigurationManager.AppSettings["WallPageSize"].ToString());

            ResponsePost response = new ResponsePost();
            WallDto wallDto = new WallDto();
            await Task.Factory.StartNew(() =>
                {
                    response = wallDto.GetPosts(
                        0,
                        Convert.ToInt64(Session["view_userId"]),
                        Convert.ToInt64(Session["userId"]),
                        lastId,
                        pageSize,
                        0 //All post
                    );
                });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetPost(int postId)
        {
            ResponsePost response = new ResponsePost();
            WallDto wallDto = new WallDto();
            await Task.Factory.StartNew(() =>
            {
                response = wallDto.GetPosts(
                    postId,
                    (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                    (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                    0,
                    1,
                    0 //All post
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetComments(int firstId, long postId)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["CommentPageSize"].ToString());

            ResponseComment response = new ResponseComment();
            WallDto wallDto = new WallDto();
            await Task.Factory.StartNew(() =>
            {
                response = wallDto.GetComments(
                    0,
                    postId,
                    (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                    firstId,
                    pageSize
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Writes the post.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="idTarget">The identifier target.</param>
        /// <param name="idDo">The identifier do.</param>
        /// <param name="idDd">The identifier dd.</param>
        /// <param name="typeEvent">The type event.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="locationString">The location string.</param>
        /// <param name="locationCode">The location code.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="externalType">Type of the external.</param>
        /// <param name="externalLink">The external link.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> WritePost(
            string comment,
            string id,
            string idTarget,
            string typeEvent,
            string visibility,
            string locationString,
            string locationCode,
            string latitude,
            string longitude,
            string externalType,
            string externalLink
        )
        {
            ResponsePost response = new ResponsePost();
            await Task.Factory.StartNew(() =>
            {
                response = SavePost(
                    comment,
                    id,
                    idTarget,
                    Session["userId"].ToString(),
                    (Session["view_userId"].ToString().Equals("0")) ? Session["userId"].ToString() : Session["view_userId"].ToString(),
                    typeEvent,
                    visibility,
                    locationString,
                    locationCode,
                    latitude,
                    longitude,
                    externalType,
                    externalLink,
                    null
                    );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Writes the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="idTarget">The identifier target.</param>
        /// <param name="idDo">The identifier do.</param>
        /// <param name="idDd">The identifier dd.</param>
        /// <param name="typeEvent">The type event.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="locationString">The location string.</param>
        /// <param name="locationCode">The location code.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="externalType">Type of the external.</param>
        /// <param name="externalLink">The external link.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> WriteComment(
            string comment,
            string idTarget,
            string idDd,
            string typeEvent,
            string visibility,
            string locationString,
            string locationCode,
            string latitude,
            string longitude,
            string externalType,
            string externalLink
        )
        {
            ResponseComment response = new ResponseComment();
            await Task.Factory.StartNew(() =>
            {
                response = SaveComment(
                    comment,
                    idTarget,
                    Session["userId"].ToString(),
                    idDd,
                    visibility,
                    locationString,
                    locationCode,
                    latitude,
                    longitude,
                    externalType,
                    externalLink,
                    null
                    );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Writes the post upload.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> WritePostUpload(String data)
        {
            ResponsePost response = new ResponsePost();

            try
            {
                await Task.Factory.StartNew(() =>
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    CreatePost dataParameters = js.Deserialize<CreatePost>(data);
                    ReturnUpload returnUpload = UploadBlobHelper.UploadFiles(Request.Files);

                    if (returnUpload.files.Count > 0)
                    {
                        CommonDto commonDto = new CommonDto();

                        foreach (FileUpload file in returnUpload.files)
                        {
                            response = SavePost(
                                dataParameters.comment,
                                dataParameters.id,
                                dataParameters.idTarget,
                                Session["userId"].ToString(),
                                (Session["view_userId"].ToString().Equals("0")) ? Session["userId"].ToString() : Session["view_userId"].ToString(),
                                dataParameters.typeEvent,
                                dataParameters.visibility,
                                dataParameters.locationString,
                                dataParameters.locationCode,
                                dataParameters.latitude,
                                dataParameters.longitude,
                                dataParameters.externalType,
                                dataParameters.externalLink,
                                commonDto.InsertBlob(
                                    file.newGuid,
                                    file.fileExtension,
                                    file.fileWidth,
                                    file.fileHeight
                                    ).ToString()
                                );
                        }
                    }
                    else
                    {
                        response = SavePost(
                           dataParameters.comment,
                           dataParameters.id,
                           dataParameters.idTarget,
                           Session["userId"].ToString(),
                           (Session["view_userId"].ToString().Equals("0")) ? Session["userId"].ToString() : Session["view_userId"].ToString(),
                           dataParameters.typeEvent,
                           dataParameters.visibility,
                           dataParameters.locationString,
                           dataParameters.locationCode,
                           dataParameters.latitude,
                           dataParameters.longitude,
                           dataParameters.externalType,
                           dataParameters.externalLink,
                           null
                           );
                    };
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.UtcNow);
                response.code = -1;
                response.message = ex.Message;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the post.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="idTarget">The identifier target.</param>
        /// <param name="idDo">The identifier do.</param>
        /// <param name="idDd">The identifier dd.</param>
        /// <param name="typeEvent">The type event.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="locationString">The location string.</param>
        /// <param name="locationCode">The location code.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="externalType">Type of the external.</param>
        /// <param name="externalLink">The external link.</param>
        /// <param name="idBlobMaterial">The identifier BLOB material.</param>
        /// <returns></returns>
        private ResponsePost SavePost(
            string comment,
            string id,
            string idTarget,
            string idDo,
            string idDd,
            string typeEvent,
            string visibility,
            string locationString,
            string locationCode,
            string latitude,
            string longitude,
            string externalType,
            string externalLink,
            string idBlobMaterial
        )
        {
            long _id = (id != null && !id.Equals("")) ? Convert.ToInt64(id) : 0;
            long? _idTarget = (idTarget != null && !idTarget.Equals("")) ? Convert.ToInt64(idTarget) : (long?)null;
            string _locationString = (locationString != null && !locationString.Equals("")) ? locationString : null;
            string _locationCode = (locationCode != null && !locationCode.Equals("")) ? locationCode : null;
            decimal? _latitude = (latitude != null && !latitude.Equals("")) ? Convert.ToDecimal(latitude) : (decimal?)null;
            decimal? _longitude = (longitude != null && !longitude.Equals("")) ? Convert.ToDecimal(longitude) : (decimal?)null;
            string _externalLink = (externalLink != null && !externalLink.Equals("")) ? externalLink : null;
            long? _idBlobMaterial = (idBlobMaterial != null && !idBlobMaterial.Equals("")) ? Convert.ToInt64(idBlobMaterial) : (long?)null;

            WallDto wallDto = new WallDto();
            return wallDto.WritePosts(
                comment,
                _id,
                _idTarget,
                Convert.ToInt64(idDo),
                Convert.ToInt64(idDd),
                Convert.ToInt32(typeEvent),
                Convert.ToInt32(visibility),
                _locationString,
                _locationCode,
                _latitude,
                _longitude,
                Convert.ToInt32(externalType),
                _externalLink,
                _idBlobMaterial,
                false,
                Convert.ToInt64(Session["userId"])
                );
        }

        /// <summary>
        /// Saves the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="idTarget">The identifier target.</param>
        /// <param name="idDo">The identifier do.</param>
        /// <param name="idDd">The identifier dd.</param>
        /// <param name="typeEvent">The type event.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="locationString">The location string.</param>
        /// <param name="locationCode">The location code.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="externalType">Type of the external.</param>
        /// <param name="externalLink">The external link.</param>
        /// <param name="idBlobMaterial">The identifier BLOB material.</param>
        /// <returns></returns>
        private ResponseComment SaveComment(
            string comment,
            string idTarget,
            string idDo,
            string idDd,
            string visibility,
            string locationString,
            string locationCode,
            string latitude,
            string longitude,
            string externalType,
            string externalLink,
            string idBlobMaterial
        )
        {
            long? _idTarget = (idTarget != null && !idTarget.Equals("")) ? Convert.ToInt64(idTarget) : (long?)null;
            string _locationString = (locationString != null && !locationString.Equals("")) ? locationString : null;
            string _locationCode = (locationCode != null && !locationCode.Equals("")) ? locationCode : null;
            decimal? _latitude = (latitude != null && !latitude.Equals("")) ? Convert.ToDecimal(latitude) : (decimal?)null;
            decimal? _longitude = (longitude != null && !longitude.Equals("")) ? Convert.ToDecimal(longitude) : (decimal?)null;
            string _externalLink = (externalLink != null && !externalLink.Equals("")) ? externalLink : null;
            long? _idBlobMaterial = (idBlobMaterial != null && !idBlobMaterial.Equals("")) ? Convert.ToInt64(idBlobMaterial) : (long?)null;

            WallDto wallDto = new WallDto();
            return wallDto.WriteComment(
                comment,
                _idTarget,
                Convert.ToInt64(idDo),
                Convert.ToInt64(idDd),
                visibility,
                _locationString,
                _locationCode,
                _latitude,
                _longitude,
                Convert.ToInt32(externalType),
                _externalLink,
                _idBlobMaterial,
                Convert.ToInt64(Session["userId"])
                );
        }

        /// <summary>
        /// Removes the post.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RemovePost(string id)
        {
            ResponseMessage response = new ResponseMessage();
            WallDto wallDto = new WallDto();
            await Task.Factory.StartNew(() =>
            {
                response = wallDto.RemovePosts(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes the comment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RemoveComment(string id)
        {
            ResponseMessage response = new ResponseMessage();
            WallDto wallDto = new WallDto();
            await Task.Factory.StartNew(() =>
            {
                response = wallDto.RemovePosts(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Likes the post.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> LikePost(string id)
        {
            ResponseMessage response = new ResponseMessage();
            WallDto wallDto = new WallDto();
            await Task.Factory.StartNew(() =>
            {
                response = wallDto.LikePosts(
                    Convert.ToInt64(id),
                    Convert.ToInt64(Session["userId"])
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}