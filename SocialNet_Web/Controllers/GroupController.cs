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
    public class GroupController : BaseController
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
            ResponseGroupBadges response = new ResponseGroupBadges();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.GetCounts(
                                (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                                (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0
                            );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="tab">The tab.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetGroups(int lastId, int tab)
        {
            int pageSize = int.Parse((Request.Browser.IsMobileDevice) ?
                ConfigurationManager.AppSettings["GroupPageSize.mobile"].ToString() :
                ConfigurationManager.AppSettings["GroupPageSize"].ToString());

            ResponseGroup response = new ResponseGroup();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                switch (tab)
                {
                    case 1: //all groups
                        response = groupDto.GetGroups(
                            0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                    case 2: //groups suggested
                        response = groupDto.GetGroupsSuggested(
                            0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                    case 3: //groups from friends
                        response = groupDto.GetFriendGroups(
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
        /// Gets the members.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="tab">The tab.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetMembers(int lastId, int tab)
        {
            int pageSize = int.Parse((Request.Browser.IsMobileDevice) ?
                ConfigurationManager.AppSettings["FriendPageSize.mobile"].ToString() :
                ConfigurationManager.AppSettings["FriendPageSize"].ToString());

            ResponseMember response = new ResponseMember();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                switch (tab)
                {
                    case 1: //all member
                        response = groupDto.GetMembers(
                            (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                    case 2: //send request to be member
                        response = groupDto.GetMembersSend(
                            (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                    case 3: //receive request to be member
                        response = groupDto.GetMembersReceive(
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
        /// Follows the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> FollowGroup(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.FollowGroup(
                    Convert.ToInt64(id),
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(Session["view_userId"]));
                if (response.code == 0) Session["view_isFollow"] = ((int)Session["view_isFollow"]==1)?0:1;
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RemoveGroup(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.RemoveGroup(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes the group from profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RemoveGroupFromProfile(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.RemoveGroup(Convert.ToInt64(id));
                if (response.code==0) response.action = Rules.actionMessage.RedirectToPublicWall;
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Uns the link group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> UnLinkGroup(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.UnLinkGroup(
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(id)
                    );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Finds the group.
        /// </summary>
        /// <param name="searchstr">The searchstr.</param>
        /// <returns></returns>
        [HttpGet]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> FindGroup(string searchstr)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["FriendSearchSize"].ToString());

            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.FindGroups(
                    searchstr,
                    (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                    pageSize
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Joins the group.
        /// </summary>
        /// <param name="idGroup">The identifier group.</param>
        /// <param name="idMember">The identifier member.</param>
        /// <param name="send">The send.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> JoinGroup(
            string idGroup,
            string idMember,
            int send
        )
        {
            ResponseGroup response = new ResponseGroup();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.JoinGroup(
                    (idMember.Equals("0")) ? Convert.ToInt64(Session["userId"]) : Convert.ToInt64(idMember),
                    (idGroup.Equals("0")) ? Convert.ToInt64(Session["view_userId"]) : Convert.ToInt64(idGroup),
                    Convert.ToInt64(Session["userId"]),
                    (send==1)?true:false
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Cancels the join.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> CancelJoin(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.CancelJoin(
                    Convert.ToInt64(id)
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the group.
        /// </summary>
        /// <param name="idD">The identifier d.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> AddGroup(string name)
        {
            ResponseGroup response = new ResponseGroup();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.AddGroup(
                    Convert.ToInt64(Session["userId"]),
                    name
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Accepts the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> AcceptGroup(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();          
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.AcceptGroup(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Rejects the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RejectGroup(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.RejectGroup(Convert.ToInt64(id));
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
            string groupname,
            string createdate,
            long country
        )
        {
            ResponseProfile response = new ResponseProfile();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.SaveProfile(
                    Convert.ToInt64(Session["view_userId"]),
                    groupname,
                    createdate,
                    Convert.ToInt64(country)
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
                        response.item.photoProfile = String.Concat(CommonHelper.getHandlerPath(), returnUpload.files[0].newGuid.ToString().ToUpper(), ".", returnUpload.files[0].fileExtension);

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
                        response.item.photoCover = String.Concat(CommonHelper.getHandlerPath(), returnUpload.files[0].newGuid.ToString().ToUpper(), ".", returnUpload.files[0].fileExtension);

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
        /// Adds the member.
        /// </summary>
        /// <param name="idD">The identifier d.</param>
        /// <param name="idR">The identifier r.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> AddMember(string idD, string idR)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.AddMember(
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(idD),
                    Convert.ToInt64(idR)
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Accepts the member.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> AcceptMember(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.AcceptMember(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Rejects the member.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RejectMember(string id)
        {
            ResponseMessage response = new ResponseMessage();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.RejectMember(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the counts members.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetCountsMembers()
        {
            ResponseFriendBadges response = new ResponseFriendBadges();
            GroupDto groupDto = new GroupDto();
            await Task.Factory.StartNew(() =>
            {
                response = groupDto.GetCountsMembers(
                                (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                                (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0
                            );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}