using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Error404s this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Error404()
        {
            return View();
        }
    }
}