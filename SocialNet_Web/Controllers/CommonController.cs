using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.DTO;

namespace Web.Controllers
{
    [Localisation]
    public class CommonController : Controller
    {
        /// <summary>
        /// Gets the badges.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetBadges()
        {
            ResponseBadges response = new ResponseBadges();
            CommonDto commonDto = new CommonDto();
            await Task.Factory.StartNew(() =>
            {
                response = commonDto.GetBadges((Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0);
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the alerts.
        /// </summary>
        /// <param name="lastId">The last identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetAlerts(int lastId)
        {
            int pageSize = int.Parse(ConfigurationManager.AppSettings["AlertPageSize"].ToString());

            ResponseAlert response = new ResponseAlert();
            CommonDto commonDto = new CommonDto();
            await Task.Factory.StartNew(() =>
            {
                response = commonDto.GetAlerts(
                    (Convert.ToInt64(Session["userId"]) > 0) ? Convert.ToInt64(Session["userId"]) : 0,
                    lastId,
                    pageSize);
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sets the view alert.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> ViewAlert(int id)
        {
            ResponseMessage response = new ResponseMessage();
            CommonDto commonDto = new CommonDto();
            await Task.Factory.StartNew(() =>
            {
                response = commonDto.ViewAlert(id);
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the list country.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        [CacheFilter(Duration = 5, Order = 2)]
        public async Task<ActionResult> GetListCountry()
        {
            ResponseItemCombo response = new ResponseItemCombo();
            CommonDto commonDto = new CommonDto();
            await Task.Factory.StartNew(() =>
            {
                response = commonDto.GetListCountry();
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}