using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.DTO;

namespace Web.Controllers
{
    [RoutePrefix("")]
    public class FriendController : BaseController
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
        /// Indexes the company.
        /// </summary>
        /// <returns></returns>
        [CompressFilter(Order = 1)]
        public ActionResult IndexCompany()
        {
            if (ViewBag.viewOtherUser == null) Session["view_userId"] = 0;
            string usercode = this.ControllerContext.RouteData.Values["usercode"].ToString();
            SwitchByName(usercode);
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
            ResponseFriendBadges response = new ResponseFriendBadges();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.GetCounts(
                                (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                                (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0
                            );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the friends.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetFriends(int lastId, int tab)
        {
            int pageSize = int.Parse((Request.Browser.IsMobileDevice) ?
                ConfigurationManager.AppSettings["FriendPageSize.mobile"].ToString() :
                ConfigurationManager.AppSettings["FriendPageSize"].ToString());

            ResponseFriend response = new ResponseFriend();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                switch (tab)
                {
                    case 1: //all friend
                        response = friendDto.GetFriends(
                            (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                    case 2: //send request of friend
                        response = friendDto.GetFriendsSend(
                            (Convert.ToInt64(Session["view_userId"]) > 0) ? Convert.ToInt64(Session["view_userId"]) : 0,
                            (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                            lastId,
                            pageSize
                        );
                        break;
                    case 3: //receive request of friend
                        response = friendDto.GetFriendsReceive(
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
        /// Follows the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> FollowFriend(string id)
        {
            ResponseMessage response = new ResponseMessage();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.FollowFriend(
                    Convert.ToInt64(id),
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(Session["view_userId"])
                    );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Blocks the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> BlockFriend(string id)
        {
            ResponseMessage response = new ResponseMessage();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.BlockFriend(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RemoveFriend(string id)
        {
            ResponseMessage response = new ResponseMessage();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.RemoveFriend(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Finds the person.
        /// </summary>
        /// <param name="searchstr">The searchstr.</param>
        /// <returns></returns>
        [HttpGet]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> FindPerson(string searchstr)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["FriendSearchSize"].ToString());

            ResponseMessage response = new ResponseMessage();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.FindFriends(
                    searchstr,
                    (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                    pageSize
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the friend.
        /// </summary>
        /// <param name="idO">The identifier origen.</param>
        /// <param name="idD">The identifier destin.</param>
        /// <param name="idR">The identifier recomend.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> AddFriend(string idD, string idR)
        {
            ResponseMessage response = new ResponseMessage();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.AddFriend(
                    Convert.ToInt64(Session["userId"]),
                    Convert.ToInt64(idD),
                    Convert.ToInt64(idR)
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Accepts the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> AcceptFriend(string id)
        {
            ResponseMessage response = new ResponseMessage();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.AcceptFriend(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Rejects the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RejectFriend(string id)
        {
            ResponseMessage response = new ResponseMessage();
            FriendDto friendDto = new FriendDto();
            await Task.Factory.StartNew(() =>
            {
                response = friendDto.RejectFriend(Convert.ToInt64(id));
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}