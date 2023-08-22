using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.App_Start;
using Web_BusinessLayer.Enum;
using Web_BusinessLayer.Helpers;

namespace SocialNet_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleMobileConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            Session["isLogin"] = false;

            // User's session
            Session["userId"] = 0;
            Session["userName"] = string.Empty;
            Session["userCode"] = string.Empty;
            Session["photoCover"] = string.Empty;
            Session["photoProfile"] = String.Concat(String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile), CommonHelper.getImageWidth(Rules.imageSize.Profile));
            Session["typeEntity"] = 1; //person
            Session["locale"] = String.Empty;
            Session["view_userId"] = 0;
            Session["view_userName"] = string.Empty;
            Session["view_userCode"] = string.Empty;
            Session["view_photoCover"] = string.Empty;
            Session["view_photoProfile"] = String.Concat(String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile), CommonHelper.getImageWidth(Rules.imageSize.Profile));
            Session["view_typeEntity"] = 0;
            Session["view_targetId"] = 0;
            Session["view_isFriend"] = 0;
            Session["view_isMember"] = 0;
            Session["view_isFollow"] = 0;
            Session["view_canPost"] = 0;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            app.Response.Filter = null;
        }
    }
}
