using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.DTO;

namespace Web.Controllers
{
    [Localisation]
    public class BaseController : Controller
    {
        /// <summary>
        /// Changes the culture.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void ChangeCulture(string id)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(id);

            HttpCookie _cookie = new HttpCookie(ConfigurationManager.AppSettings["CurrentUICulture"].ToString(), id);
            _cookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Response.SetCookie(_cookie);
        }

        /// <summary>
        /// Removes the cookie culture.
        /// </summary>
        public void RemoveCookieCulture()
        {
            var expiredCookie = new HttpCookie(ConfigurationManager.AppSettings["CurrentUICulture"].ToString()) { Expires = DateTime.Now.AddDays(-1) };
            HttpContext.Response.Cookies.Add(expiredCookie);
        }

        /// <summary>
        /// Switches the name of the new name.
        /// </summary>
        /// <param name="usercode">The usercode.</param>
        /// <returns></returns>
        [CompressFilter(Order = 1)]
        public ActionResult SwitchByName(string userCode)
        {
            ViewBag.viewOtherUser = true;

            if ((Convert.ToInt64(Session["view_userId"]) == 0 || !Session["view_userCode"].ToString().Equals(userCode)) && Convert.ToInt64(Session["userId"]) != 0)
            {
                BaseDto baseDto = new BaseDto();
                ResponseLogin response = baseDto.GetOtherUserInfo(
                    userCode,
                    Convert.ToInt64(Session["userId"])
                );

                Session["view_userId"] = response.view_userId;
                Session["view_userName"] = response.view_userName;
                Session["view_userCode"] = response.view_userCode;
                Session["view_photoProfile"] = response.view_photoProfile;
                Session["view_photoCover"] = response.view_photoCover;
                Session["view_typeEntity"] = response.view_typeEntity;
                Session["view_targetId"] = response.view_targetId;
                Session["view_isFriend"] = response.view_isFriend;
                Session["view_isMember"] = response.view_isMember;
                Session["view_isFollow"] = response.view_isFollow;
                Session["view_canPost"] = response.view_canPost;
            }
            return View("Index");
        }
    }
}