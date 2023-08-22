using System;
using System.Configuration;
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
    public class CompanyController : BaseController
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
        /// Gets the counts.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetCounts()
        {
            ResponseCompanyBadges response = new ResponseCompanyBadges();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.GetCounts(
                                (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0
                            );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="tab">The tab.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetCompanies(int lastId, int tab)
        {
            int pageSize = int.Parse((Request.Browser.IsMobileDevice) ?
                ConfigurationManager.AppSettings["GroupPageSize.mobile"].ToString() :
                ConfigurationManager.AppSettings["GroupPageSize"].ToString());

            ResponseCompany response = new ResponseCompany();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                switch (tab)
                {
                    case 1: //all companies
                        response = companyDto.GetCompanies(
                            0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the followers.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="tab">The tab.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetFollowers(int lastId, int tab)
        {
            int pageSize = int.Parse((Request.Browser.IsMobileDevice) ?
                ConfigurationManager.AppSettings["FriendPageSize.mobile"].ToString() :
                ConfigurationManager.AppSettings["FriendPageSize"].ToString());

            ResponseFollower response = new ResponseFollower();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                switch (tab)
                {
                    case 1: //all followers
                        response = companyDto.GetFollowers(
                            (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Follows the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> FollowCompany(string id)
        {
            ResponseMessage response = new ResponseMessage();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.FollowCompany(
                    Convert.ToInt64(id),
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(Session["view_userId"]));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the profile.
        /// </summary>
        /// <param name="groupname">The groupname.</param>
        /// <param name="createdate">The createdate.</param>
        /// <param name="country">The country.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> SaveProfile(
            string companyName,
            long country,
            long language
        )
        {
            ResponseProfile response = new ResponseProfile();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.SaveProfile(
                    Convert.ToInt64(Session["view_userId"]),
                    companyName,
                    Convert.ToInt64(country),
                    Convert.ToInt64(language)
                    );

                if (response.code == 0)
                {
                    Session["view_userName"] = response.item.firstname;
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
                        Convert.ToInt64(Session["view_userId"].ToString()),
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

                    if (responsePost.code == 0)
                    {
                        ProfileDto profileDto = new ProfileDto();
                        response = profileDto.SavePhoto(
                            Convert.ToInt64(Session["view_userId"].ToString()),
                            responsePost.items.First().postId
                        );
                        response.item = new ProfileItem();
                        response.item.photoProfile = String.Concat(CommonHelper.getHandlerPath(), String.Concat(returnUpload.files[0].newGuid.ToString().ToUpper(), ".", returnUpload.files[0].fileExtension));

                        if (response.code == 0)
                        {
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
                if (returnUpload.code == 0 && returnUpload.files.Count == 1)
                {
                    CommonDto commonDto = new CommonDto();

                    WallDto wallDto = new WallDto();
                    ResponsePost responsePost = wallDto.WritePosts(
                        "",
                        0,
                        null,
                        Convert.ToInt64(Session["userId"].ToString()),
                        Convert.ToInt64(Session["view_userId"].ToString()),
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
                            Convert.ToInt64(Session["view_userId"].ToString()),
                            responsePost.items.First().postId
                        );
                        response.item = new ProfileItem();
                        response.item.photoCover = String.Concat(CommonHelper.getHandlerPath(), String.Concat(returnUpload.files[0].newGuid.ToString().ToUpper(), ".", returnUpload.files[0].fileExtension));

                        if (response.code == 0)
                        {
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

        /// <summary>
        /// Adds the follower.
        /// </summary>
        /// <param name="idD">The identifier d.</param>
        /// <param name="idR">The identifier r.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> AddFollower(string idD, string idR)
        {
            ResponseMessage response = new ResponseMessage();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.AddFollower(
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(idD),
                    Convert.ToInt64(idR)
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Uns the link company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> UnLinkCompany(string id)
        {
            ResponseMessage response = new ResponseMessage();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.UnLinkCompany(
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(id)
                    );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Finds the company.
        /// </summary>
        /// <param name="searchstr">The searchstr.</param>
        /// <returns></returns>
        [HttpGet]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> FindCompany(string searchstr)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["FriendSearchSize"].ToString());

            ResponseMessage response = new ResponseMessage();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.FindCompanies(
                    searchstr,
                    (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                    pageSize
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Joins the company.
        /// </summary>
        /// <param name="idCompany">The identifier company.</param>
        /// <param name="idFollower">The identifier follower.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> JoinCompany(
            string idCompany,
            string idFollower
        )
        {
            ResponseCompany response = new ResponseCompany();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.JoinCompany(
                    (idFollower.Equals("0")) ? Convert.ToInt64(Session["userId"]) : Convert.ToInt64(idFollower),
                    (idCompany.Equals("0")) ? Convert.ToInt64(Session["view_userId"]) : Convert.ToInt64(idCompany)
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the switch.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> SaveSwitch(
            string name,
            string value
        )
        {
            ResponseMessage response = new ResponseMessage();
            CompanyDto companyDto = new CompanyDto();
            await Task.Factory.StartNew(() =>
            {
                response = companyDto.SaveSwitch(
                    Convert.ToInt64(Session["view_userId"]),
                    name,
                    Convert.ToBoolean(value)
                    );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}