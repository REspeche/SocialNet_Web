namespace Web.Controllers
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Helpers;

    internal class LocalisationAttribute : ActionFilterAttribute
    {
        public const string LangParam = "lang";
        public string CookieName = ConfigurationManager.AppSettings["CurrentUICulture"].ToString();

        // List of allowed languages in this app (to speed up check)
        private const string Cultures = "en es";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Try getting culture from URL first
            var culture = (string)filterContext.RouteData.Values[LangParam];

            // If not provided, or the culture does not match the list of known cultures, try cookie or browser setting
            if (string.IsNullOrEmpty(culture) || !CultureHelper.isCultureExist(culture))
            {
                // load the culture info from the cookie
                var cookie = filterContext.HttpContext.Request.Cookies[CookieName];
                if (cookie != null)
                {
                    // set the culture by the cookie content
                    culture = cookie.Value;
                }
                else
                {
                    // set the culture by the location if not specified
                    culture = filterContext.HttpContext.Request.UserLanguages[0];
                }
                // set the lang value into route data
                filterContext.RouteData.Values[LangParam] = culture;
            }

            // Keep the part up to the "-" as the primary language
            var language = culture.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0];
            filterContext.RouteData.Values[LangParam] = language;

            // Set the language - ignore specific culture for now
            var cultureInfo = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            // save the locale into cookie (full locale)
            HttpCookie _cookie = new HttpCookie(CookieName, culture);
            _cookie.Expires = DateTime.Now.AddYears(1);
            filterContext.HttpContext.Response.SetCookie(_cookie);

            if (filterContext.HttpContext.Session["locale"].ToString().Equals("")) filterContext.HttpContext.Session["locale"] = culture;

            // Pass on to normal controller processing
            base.OnActionExecuting(filterContext);
        }
    }
}