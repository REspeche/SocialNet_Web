using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Helpers;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.DTO;
using Web_BusinessLayer.Enum;
using Web_BusinessLayer.Helpers;

namespace Web.Controllers
{
    [RoutePrefix("")]
    public class ProfileController : BaseController
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
        /// Indexes the group.
        /// </summary>
        /// <returns></returns>
        [CompressFilter(Order = 1)]
        public ActionResult IndexGroup()
        {
            return View();
        }

        /// <summary>
        /// Gets the profile data.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetProfileData()
        {
            ResponseProfile response = new ResponseProfile();
            ProfileDto profileDto = new ProfileDto();
            await Task.Factory.StartNew(() =>
            {
                response = profileDto.GetProfileData(
                    (Convert.ToInt64(Session["view_userId"])==0)? 
                    Convert.ToInt64(Session["userId"])
                    :Convert.ToInt64(Session["view_userId"])
                    );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the profile.
        /// </summary>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="birthdate">The birthdate.</param>
        /// <param name="language">The language.</param>
        /// <param name="country">The country.</param>
        /// <param name="typeEntity">The type entity.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> SaveProfile(
            string firstname,
            string lastname,
            int gender,
            string birthdate,
            long language,
            long country
        )
        {
            ResponseProfile response = new ResponseProfile();
            ProfileDto profileDto = new ProfileDto();
            await Task.Factory.StartNew(() =>
            {
                response = profileDto.SaveProfile(
                    Convert.ToInt64(Session["userId"]),
                    firstname,
                    lastname,
                    Convert.ToInt16(gender),
                    birthdate,
                    Convert.ToInt64(language),
                    Convert.ToInt64(country)
                    );

                if (response.code == 0)
                {
                    Session["userName"] = String.Concat(response.item.firstname," ", response.item.lastname);
                    Session["view_userName"] = String.Concat(response.item.firstname, " ", response.item.lastname);
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the photo.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> SavePhoto()
        {
            ResponseProfile response = new ResponseProfile();

            await Task.Factory.StartNew(() =>
            {
                ReturnUpload returnUpload = UploadBlobHelper.UploadFiles(Request.Files);
                if (returnUpload.code == 0 && returnUpload.files.Count == 1)
                {
                    CommonDto commonDto = new CommonDto();

                    WallDto wallDto = new WallDto();
                    ResponsePost responsePost = wallDto.WritePosts(
                        "",
                        0,
                        null,
                        Convert.ToInt64(Session["userId"].ToString()),
                        Convert.ToInt64(Session["userId"].ToString()),
                        (int)Tables.eventType.Profile,
                        (int)Tables.visibility.Public,
                        null,
                        null,
                        null,
                        null,
                        (int)Tables.externalType.None,
                        null,
                        commonDto.InsertBlob(
                                    returnUpload.files[0].newGuid,
                                    returnUpload.files[0].fileExtension,
                                    returnUpload.files[0].fileWidth,
                                    returnUpload.files[0].fileHeight
                                    ),
                        true,
                        Convert.ToInt64(Session["userId"])
                        );

                    if (responsePost.code == 0) {
                        ProfileDto profileDto = new ProfileDto();
                        response = profileDto.SavePhoto(
                            Convert.ToInt64(Session["userId"].ToString()),
                            responsePost.items.First().postId
                        );
                        response.item = new ProfileItem();
                        response.item.photoProfile = String.Concat(CommonHelper.getHandlerPath(), returnUpload.files[0].newGuid.ToString().ToUpper(), ".", returnUpload.files[0].fileExtension);

                        if (response.code == 0)
                        {
                            Session["photoProfile"] = response.item.photoProfile;
                            Session["view_photoProfile"] = response.item.photoProfile;
                        }
                    }
                    else
                    {
                        response.code = responsePost.code;
                        response.message = responsePost.message;
                    }
                }
                else
                {
                    response.code = returnUpload.code;
                    response.message = returnUpload.message;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the cover.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> SaveCover()
        {
            ResponseProfile response = new ResponseProfile();

            await Task.Factory.StartNew(() =>
            {
                ReturnUpload returnUpload = UploadBlobHelper.UploadFiles(Request.Files);
                if (returnUpload.code==0 && returnUpload.files.Count == 1)
                {
                    CommonDto commonDto = new CommonDto();

                    WallDto wallDto = new WallDto();
                    ResponsePost responsePost = wallDto.WritePosts(
                        "",
                        0,
                        null,
                        Convert.ToInt64(Session["userId"].ToString()),
                        Convert.ToInt64(Session["userId"].ToString()),
                        (int)Tables.eventType.Cover,
                        (int)Tables.visibility.Public,
                        null,
                        null,
                        null,
                        null,
                        (int)Tables.externalType.None,
                        null,
                        commonDto.InsertBlob(
                                    returnUpload.files[0].newGuid,
                                    returnUpload.files[0].fileExtension,
                                    returnUpload.files[0].fileWidth,
                                    returnUpload.files[0].fileHeight
                                    ),
                        true,
                        Convert.ToInt64(Session["userId"])
                        );

                    if (responsePost.code == 0)
                    {
                        ProfileDto profileDto = new ProfileDto();
                        response = profileDto.SaveCover(
                            Convert.ToInt64(Session["userId"].ToString()),
                            responsePost.items.First().postId
                        );
                        response.item = new ProfileItem();
                        response.item.photoCover = String.Concat(CommonHelper.getHandlerPath(), returnUpload.files[0].newGuid.ToString().ToUpper(), ".", returnUpload.files[0].fileExtension);

                        if (response.code == 0)
                        {
                            Session["photoCover"] = response.item.photoCover;
                            Session["view_photoCover"] = response.item.photoCover;
                        }
                    }
                    else
                    {
                        response.code = responsePost.code;
                        response.message = responsePost.message;
                    }
                }
                else
                {
                    response.code = returnUpload.code;
                    response.message = returnUpload.message;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}