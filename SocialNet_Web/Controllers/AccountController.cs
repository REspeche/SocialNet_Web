using System.Web.Mvc;
using Web_BusinessLayer.DTO;
using Web_BusinessLayer.Classes;
using Web.Helpers;
using System;
using Web_BusinessLayer.Enum;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [RoutePrefix("")]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Returns the session.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public ActionResult returnSession()
        {
            ResponseLogin response = new ResponseLogin();

            response.code = 0;
            response.userId = Convert.ToInt64(Session["userId"]);
            response.view_userId = Convert.ToInt64(Session["view_userId"]);
            response.userName = (Session["userName"] == null) ? "" : Session["userName"].ToString();
            response.userCode = (Session["userCode"]==null) ? "" : Session["userCode"].ToString();
            response.photoProfile = (Session["photoProfile"] == null) ? "" : Session["photoProfile"].ToString();
            response.locale = (Session["photoProfile"] == null) ? "es" : Session["locale"].ToString();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public ActionResult closeSession()
        {
            ResponseMessage response = new ResponseMessage();

            response.code = 0;
            response.action = (Convert.ToInt64(Session["view_userId"]) > 0) ? Rules.actionMessage.RedirectToLogin : Rules.actionMessage.None; //redirect to login if the user is viewing another wall
            RemoveCookieCulture();
            Session.Abandon();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Removes the account.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> removeAccount()
        {
            ResponseMessage response = new ResponseMessage();

            var accountDto = new AccountDto();
            await Task.Factory.StartNew(() =>
            {
                response = accountDto.RemoveAccount(Convert.ToInt64(Session["userId"]));
                if (response.code == 0)
                {
                    response.action = Rules.actionMessage.RedirectToLogin;
                    RemoveCookieCulture();
                    Session.Abandon();
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Logins the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> Login(string email, string password)
        {
            ResponseLogin response = new ResponseLogin();
            var accountDto = new AccountDto();
            await Task.Factory.StartNew(() =>
            {
                response = accountDto.ValidateLogin(email, password);
                if (response.code == 0)
                {
                    SetSessionVars(response);
                    response.action = Rules.actionMessage.RedirectToWall;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Logins the facebook.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> LoginFacebook(
            string email, 
            string id, 
            string firstName,
            string lastName,
            string gender,
            string birthday,
            string location)
        {
            ResponseLogin response = new ResponseLogin();
            var accountDto = new AccountDto();
            await Task.Factory.StartNew(() =>
            {
                response = accountDto.ValidateLoginFacebook(email, id, firstName, lastName, gender, birthday, location);
                if (response.code == 0)
                {
                    SetSessionVars(response);
                    response.action = Rules.actionMessage.RedirectToWall;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Registers the person.
        /// </summary>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RegisterPerson(string firstname, string lastname, string email, string password)
        {
            ResponseLogin response = new ResponseLogin();
            var accountDto = new AccountDto();
            String[] dataCountry = Commons.getClientCountry();
            await Task.Factory.StartNew(() =>
            {
                ParamRegisterPerson paramPerson = new ParamRegisterPerson();
                paramPerson.firstName = firstname;
                paramPerson.lastName = lastname;
                paramPerson.email = email;
                paramPerson.password = password;
                paramPerson.idFacebook = null;
                paramPerson.location = dataCountry[0];
                paramPerson.language = dataCountry[1];
                response = accountDto.RegisterPerson(paramPerson);
                if (response.code == 0)
                {
                    Session["isLogin"] = true;

                    // User's session
                    Session["userId"] = response.userId;
                    Session["userName"] = response.userName;
                    Session["userCode"] = response.userCode;
                    Session["photoProfile"] = response.photoProfile;
                    Session["locale"] = response.locale;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Registers the company.
        /// </summary>
        /// <param name="companyname">The companyname.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> RegisterCompany(string companyname, string email, string password)
        {
            ResponseLogin response = new ResponseLogin();
            var accountDto = new AccountDto();
            await Task.Factory.StartNew(() =>
            {
                response = accountDto.RegisterCompany(companyname, email, password);
                if (response.code == 0)
                {
                    Session["isLogin"] = true;

                    // User's session
                    Session["userId"] = response.userId;
                    Session["userName"] = response.userName;
                    Session["userCode"] = response.userCode;
                    Session["photoProfile"] = response.photoProfile;
                    Session["locale"] = response.locale;
                }
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Passwords the reset.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> PasswordReset(string email)
        {
            var accountDto = new AccountDto();
            ResponseMessage response = new ResponseMessage();
            ResponseMail resMail = accountDto.PasswordReset(email);
            if (resMail.code==0)
            {
                await Task.Factory.StartNew(() =>
                {
                    response = MailHelper.SendMail(Web_Resource.Mail_Template.ResetPass.template,
                                               resMail.parameters
                                               );
                });
            }
            else
            {
                response.code = resMail.code;
                response.message = resMail.message;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public ActionResult ChangePassword(string c)
        {
            return View();
        }

        /// <summary>
        /// Sets the new password.
        /// </summary>
        /// <param name="passOld">The pass old.</param>
        /// <param name="passNew">The pass new.</param>
        /// <param name="hash">The hash.</param>
        /// <returns></returns>
        [HttpPost]
        [CompressFilter(Order = 1)]
        public async Task<ActionResult> SetNewPassword(string passOld, string passNew, string hash)
        {
            var accountDto = new AccountDto();
            ResponseMessage response = new ResponseMessage();
            try {
                await Task.Factory.StartNew(() =>
                {
                    response = accountDto.SetNewPassword(hash, passOld, passNew);
                });
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sets the session vars.
        /// </summary>
        /// <param name="response">The response.</param>
        private void SetSessionVars(ResponseLogin response)
        {
            Session["isLogin"] = true;

            // User's session
            Session["userId"] = response.userId;
            Session["userName"] = response.userName;
            Session["userCode"] = response.userCode;
            Session["photoProfile"] = response.photoProfile;
            Session["photoCover"] = response.photoCover;
            Session["typeEntity"] = response.typeEntity;
            if (!Session["locale"].ToString().Equals(response.locale))
            {
                Session["locale"] = response.locale;
                ChangeCulture(response.locale);
            }
            response.view_userId = Convert.ToInt64(Session["view_userId"]);
            response.view_userName = (Session["view_userName"] == null) ? "" : Session["view_userName"].ToString();
            response.view_userCode = (Session["view_userCode"] == null) ? "" : Session["view_userCode"].ToString();
            response.view_photoProfile = (Session["view_photoProfile"] == null) ? "" : Session["view_photoProfile"].ToString();
            response.view_photoCover = (Session["view_photoCover"] == null) ? "" : Session["view_photoCover"].ToString();
            response.view_typeEntity = Convert.ToInt32(Session["view_typeEntity"]);
        }
    }
}