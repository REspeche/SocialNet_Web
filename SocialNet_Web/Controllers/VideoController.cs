using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.DTO;
using Web_BusinessLayer.Enum;

namespace Web.Controllers
{
    [RoutePrefix("")]
    public class VideoController : BaseController
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
        /// Gets the videos.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetVideos(
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
                    (int)Tables.eventType.Video
                );
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}