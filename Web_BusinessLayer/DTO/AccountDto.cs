using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.Enum;
using Web_BusinessLayer.Helpers;
using Web_Resource;

namespace Web_BusinessLayer.DTO
{
    public class AccountDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Validates the login.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public ResponseLogin ValidateLogin(string email, string password)
        {
            ResponseLogin response = new ResponseLogin();
            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_email == email && e.ent_enabled == true);

                if (objEnt != null)
                {
                    if (Commons.getHashSha256(password).Equals(objEnt.ent_password))
                    {
                        response = GetLoginData(objEnt);
                    }
                    else
                    {
                        response.code = 1;
                        response.message = Message.loginError1;
                    }
                }
                else
                {
                    response.code = 2;
                    response.message = Message.loginError2;
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Removes the account.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage RemoveAccount(long id)
        {
            ResponseLogin response = new ResponseLogin();
            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);

                if (objEnt != null)
                {
                    objEnt.ent_active = false;
                    objEnt.ent_enabled = false;
                    bdContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Validates the register login facebook.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="gender">The gender.</param>
        /// <returns></returns>
        public ResponseLogin ValidateLoginFacebook(string email, string id, string firstName, string lastName, string gender, string birthday, string location, bool register = true)
        {
            ResponseLogin response = new ResponseLogin();
            try
            {
                if (id != null && email != null && !id.Equals("") && !email.Equals(""))
                {
                    ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_email == email && e.ent_enabled==true);

                    if (objEnt != null)
                    {
                        if (objEnt.ent_idFacebook != null)
                        {
                            if (objEnt.ent_idFacebook == id || id.Equals("app"))
                            {
                                response = GetLoginData(objEnt);
                            }
                            else
                            {
                                response.code = 1;
                                response.message = Message.loginError1;
                            }
                        }
                        else
                        {
                            if (id.Equals("app"))
                            {
                                response = GetLoginData(objEnt);
                            }
                            else
                            {
                                ParamRegisterPerson paramFace = new ParamRegisterPerson();
                                paramFace.firstName = firstName;
                                paramFace.lastName = lastName;
                                paramFace.email = email;
                                paramFace.password = null;
                                paramFace.gender = gender;
                                paramFace.birthday = birthday;
                                paramFace.location = location;
                                paramFace.idFacebook = id;
                                response = RegisterPerson(paramFace);
                            }
                        }
                    }
                    else if (register)
                    {
                        ParamRegisterPerson paramFace = new ParamRegisterPerson();
                        paramFace.firstName = firstName;
                        paramFace.lastName = lastName;
                        paramFace.email = email;
                        paramFace.password = null;
                        paramFace.gender = gender;
                        paramFace.birthday = birthday;
                        paramFace.location = location;
                        paramFace.idFacebook = (id.Equals("app"))?null:id;
                        response = RegisterPerson(paramFace);
                    }
                    else
                    {
                        response.code = 1;
                        response.message = Message.loginError1;
                    }
                }
                else
                {
                    response.code = 1;
                    response.message = Message.loginError1;
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Registers the person.
        /// </summary>
        /// <param name="paramFace">The parameter face.</param>
        /// <returns></returns>
        public ResponseLogin RegisterPerson(ParamRegisterPerson paramFace)
        {
            Boolean isUpdate = true;
            ResponseLogin response = new ResponseLogin();
            try
            {
                bdContext.Configuration.ValidateOnSaveEnabled = false; //Disabling Automatic Validation

                ent_entity objEnt = (paramFace.idFacebook != null && paramFace.email == null) ?
                    bdContext.ent_entity.FirstOrDefault((e) => e.ent_idFacebook == paramFace.idFacebook && e.ent_enabled == true) : 
                    bdContext.ent_entity.FirstOrDefault((e) => e.ent_email == paramFace.email && e.ent_enabled==true);

                if (objEnt == null)
                {
                    isUpdate = false;
                    objEnt = new ent_entity();
                }
                cou_country objCou = null;
                lan_language objLan = null;
                if (paramFace.location != null && !paramFace.location.Equals(""))
                    objCou = bdContext.cou_country.FirstOrDefault((c) => paramFace.location.Contains(c.cou_countryName));
                if (paramFace.language != null && !paramFace.language.Equals(""))
                    objLan = bdContext.lan_language.FirstOrDefault((l) => paramFace.language.Contains(l.lan_languageCode));
                
                //Verify null
                if (objLan == null) objLan = bdContext.lan_language.FirstOrDefault((l) => Tables.language.English.ToString().Contains(l.lan_languageCode));

                objEnt.ent_firstName = paramFace.firstName;
                objEnt.ent_lastName = paramFace.lastName;
                if (paramFace.email != null) objEnt.ent_email = paramFace.email;
                if (paramFace.password != null) objEnt.ent_password = Commons.getHashSha256(paramFace.password);
                if (objCou != null) objEnt.cou_id = objCou.cou_id;
                if (objLan != null) objEnt.lan_id = objLan.lan_id;
                if (paramFace.gender != null) objEnt.ent_gender = (paramFace.gender == "male") ? (int?)Tables.gender.Male : (int?)Tables.gender.Female;
                else objEnt.ent_gender = (int?)Tables.gender.Undefined;
                objEnt.ent_typeEntity = (int?)Tables.entityType.Person;
                objEnt.ent_close = false;
                objEnt.ent_active = true;
                objEnt.ent_profileCode = Commons.getCodeProfile();
                if (paramFace.birthday != null && !paramFace.birthday.Equals("")) objEnt.ent_birthdate = Commons.convertDate(paramFace.birthday).ToUniversalTime();
                if (paramFace.idFacebook != null && !paramFace.idFacebook.Equals("app")) objEnt.ent_idFacebook = paramFace.idFacebook;
                objEnt.ent_enabled = true;
                if (!isUpdate)
                {
                    objEnt.ent_timestampCreated = DateTime.UtcNow;
                    bdContext.ent_entity.Add(objEnt);
                }
                else
                {
                    objEnt.ent_timestampModified = DateTime.UtcNow;
                }
                bdContext.SaveChanges();
                response.code = 0;
                if (response.code == 0) response = GetLoginData(objEnt);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Registers the company.
        /// </summary>
        /// <param name="companyName">Name of the company.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public ResponseLogin RegisterCompany(string companyName, string email, string password)
        {
            ResponseLogin response = new ResponseLogin();
            try
            {
                bdContext.Configuration.ValidateOnSaveEnabled = false; //Disabling Automatic Validation

                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_email == email);
                if (objEnt == null)
                {
                    CultureInfo ci = Thread.CurrentThread.CurrentUICulture;
                    cou_country objCou = bdContext.cou_country.FirstOrDefault((c) => c.cou_languages.Contains(ci.Name));
                    lan_language objLan = bdContext.lan_language.FirstOrDefault((l) => l.lan_languageCode.Contains(ci.TwoLetterISOLanguageName));

                    objEnt = new ent_entity();
                    objEnt.ent_firstName = companyName;
                    objEnt.ent_lastName = null;
                    objEnt.ent_email = email;
                    objEnt.ent_password = Commons.getHashSha256(password);
                    if (objCou != null) objEnt.cou_id = objCou.cou_id;
                    if (objLan != null) objEnt.lan_id = objLan.lan_id;
                    objEnt.ent_gender = (int?)Tables.gender.Undefined;
                    objEnt.ent_typeEntity = (int?)Tables.entityType.Company;
                    objEnt.ent_close = false;
                    objEnt.ent_active = true;
                    objEnt.ent_profileCode = Commons.getCodeProfile();
                    objEnt.ent_timestampCreated = DateTime.UtcNow;
                    objEnt.ent_enabled = true;
                    bdContext.ent_entity.Add(objEnt);
                    bdContext.SaveChanges();

                    long entId = objEnt.ent_id;
                    objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == entId);

                    response.photoProfile = String.Concat(CommonHelper.getHandlerPath(), String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), objEnt.ent_typeEntity));
                    response.locale = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString().Substring(0, 2);
                    response.userId = objEnt.ent_id;
                    response.userName = objEnt.ent_firstName;
                    response.userCode = objEnt.ent_profileCode;

                    response.code = 0;
                }
                else
                {
                    response.code = 3;
                    response.message = Message.loginError3;
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Passwords the reset.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public ResponseMail PasswordReset(string email)
        {
            ResponseMail response = new ResponseMail();
            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_email == email);
                if (objEnt != null)
                {
                    SP_MAIL_RESETPASS_Result res = (from p in bdContext.SP_MAIL_RESETPASS(objEnt.ent_id)
                        select p).FirstOrDefault();

                    response.parameters = res.GetType()
                        .GetProperties()
                        .Select(p =>
                        {
                            object value = p.GetValue(res, null);
                            return value == null ? null : value.ToString();
                        })
                        .ToArray();

                    response.code = 0;
                }
                else
                {
                    response.code = 2;
                    response.message = Message.loginError2;
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Sets the new password.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <param name="passOld">The pass old.</param>
        /// <param name="passNew">The pass new.</param>
        /// <returns></returns>
        public ResponseMail SetNewPassword(string hash, string passOld, string passNew)
        {
            ResponseMail response = new ResponseMail();
            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_hash == hash);
                if (objEnt != null)
                {
                    if (Commons.getHashSha256(passOld).Equals(objEnt.ent_password))
                    {
                        if (!passNew.Equals(passOld))
                        {
                            objEnt.ent_password = Commons.getHashSha256(passNew);
                            objEnt.ent_timestampModified = DateTime.UtcNow;
                            objEnt.ent_hash = null;
                            bdContext.SaveChanges();

                            response.code = 0;
                        }
                        else
                        {
                            response.code = 6;
                            response.message = Message.loginError6;
                        }
                    }
                    else
                    {
                        response.code = 5;
                        response.message = Message.loginError5;
                    }
                }
                else
                {
                    response.code = 4;
                    response.message = Message.loginError4;
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        public IEnumerable<EntityItem> GetListEntities(long userId)
        {
            IEnumerable<EntityItem> items = new List<EntityItem>();
            try
            {
                items = (from p in bdContext.SP_GET_LIST_ENTITIES(userId)
                            select new EntityItem
                            {
                                entityId = p.entityId,
                                description = p.description,
                                type = p.type
                            });
            }
            catch (Exception)
            {
            }
            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEnt"></param>
        /// <returns></returns>
        private ResponseLogin GetLoginData(ent_entity objEnt)
        {
            ResponseLogin response = new ResponseLogin();

            // Profile photo
            var objBlo = (from evn in bdContext.evn_event
                          from blo in bdContext.blo_blob
                          where evn.blo_idMaterial == blo.blo_id
                          && evn.evn_id == objEnt.evn_idProfile
                          select new
                          {
                              blo_guid = blo.blo_guid,
                              blo_extension = blo.blo_extension
                          }).FirstOrDefault();
            if (objBlo != null) response.photoProfile = String.Concat(CommonHelper.getHandlerPath(), objBlo.blo_guid.ToString().ToUpper(), ".", objBlo.blo_extension);
            else response.photoProfile = String.Concat(CommonHelper.getHandlerPath(), String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), objEnt.ent_typeEntity));
            // Cover photo
            objBlo = (from evn in bdContext.evn_event
                      from blo in bdContext.blo_blob
                      where evn.blo_idMaterial == blo.blo_id
                      && evn.evn_id == objEnt.evn_idCover
                      select new
                      {
                          blo_guid = blo.blo_guid,
                          blo_extension = blo.blo_extension
                      }).FirstOrDefault();
            if (objBlo != null) response.photoCover = String.Concat(CommonHelper.getHandlerPath(), objBlo.blo_guid.ToString().ToUpper(), ".", objBlo.blo_extension);
            else response.photoCover = String.Concat(CommonHelper.getHandlerPath(), String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Cover));

            if (objEnt.lan_language != null) response.locale = objEnt.lan_language.lan_languageCode;
            else response.locale = Thread.CurrentThread.CurrentUICulture.ToString().Substring(0, 2);
            response.userId = objEnt.ent_id;
            response.userName = String.Concat(objEnt.ent_firstName + " " + objEnt.ent_lastName);
            response.userCode = objEnt.ent_profileCode;
            response.typeEntity = objEnt.ent_typeEntity;

            response.code = 0;

            return response;
        }
    }
}
