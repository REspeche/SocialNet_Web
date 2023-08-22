using System;
using System.Configuration;
using System.Linq;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.DTO
{
    public class CompanyDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Gets the counts.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <returns></returns>
        public ResponseCompanyBadges GetCounts(
           long entId)
        {
            ResponseCompanyBadges response = new ResponseCompanyBadges();

            try
            {
                response = (from bud in bdContext.SP_GET_COUNT_COMPANY(
                                        entId)
                            select new ResponseCompanyBadges()
                            {
                                countAll = bud.Value
                            }).FirstOrDefault();
                response.code = 0;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseCompany GetCompanies(
           long entIdCompany,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseCompany response = new ResponseCompany();

            try
            {
                response.items = from gro in bdContext.SP_GET_COMPANIES(
                                        entIdCompany,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 where gro.rel_accepted == true
                                 select new CompanyItem()
                                 {
                                     joinId = gro.joinId,
                                     companyId = gro.companyId,
                                     companyName = gro.companyName,
                                     profileGuid = (gro.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Company) : gro.profileGuid,
                                     profileCode = gro.companyProfileCode,
                                     seggested = gro.rel_suggested,
                                     following = gro.rel_following,
                                     countMember = gro.countMember,
                                     isMe = (gro.isMe == 1) ? true : false
                                 };
                response.code = 0;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Gets the followers.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseFollower GetFollowers(
           long entId,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseFollower response = new ResponseFollower();

            try
            {
                response.items = from fol in bdContext.SP_GET_FOLLOWERS(
                                        entId,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 where fol.typeAction.Equals("all")
                                 select new FollowerItem()
                                 {
                                     friendId = fol.friendId,
                                     userId = fol.userId,
                                     userName = fol.userName,
                                     profileGuid = (fol.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : fol.profileGuid,
                                     userProfileCode = fol.userProfileCode,
                                     seggested = fol.rel_suggested,
                                     offered = fol.rel_offered,
                                     following = fol.rel_following,
                                     blocked = fol.rel_blocked,
                                     isFriend = fol.isFriend
                                 };
                response.code = 0;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Follows the company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="viewUserId">The view user identifier.</param>
        /// <returns></returns>
        public ResponseMessage FollowCompany(long id, long userId, long viewUserId)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) =>
                                                                                (r.rel_id == id && id > 0 && (r.ent_idDo == userId || r.ent_idDd == userId)) ||
                                                                                (id == 0 && r.ent_idDo == userId && r.ent_idDd == viewUserId) ||
                                                                                (id == 0 && r.ent_idDo == viewUserId && r.ent_idDd == userId) && r.rel_enabled == true);
                if (objRel != null)
                {
                    objRel.rel_following = !objRel.rel_following;
                    bdContext.SaveChanges();
                    response.code = 0;
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
        /// Saves the profile.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="companyName">Name of the company.</param>
        /// <param name="couId">The cou identifier.</param>
        /// <param name="lanId">The lan identifier.</param>
        /// <returns></returns>
        public ResponseProfile SaveProfile(
            long id,
            string companyName,
            long couId,
            long lanId
        )
        {
            ResponseProfile response = new ResponseProfile();

            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);
                if (objEnt != null)
                {
                    objEnt.ent_firstName = companyName;
                    objEnt.cou_id = couId;
                    objEnt.lan_id = lanId;
                    bdContext.SaveChanges();

                    cou_country objCou = bdContext.cou_country.FirstOrDefault((c) => c.cou_id == couId);
                    response.item = new ProfileItem();
                    response.item.firstname = companyName;
                    response.item.country = couId;
                    response.item.language = couId;
                    if (objCou != null) response.item.countryLabel = objCou.cou_countryName;
                    response.item.languageLabel = Web_Resource.Enum.ResourceManager.GetString(((Tables.language)lanId).ToString());
                }
                response.code = 0;
                response.message = Web_Resource.Message.notifyOK;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Adds the follower.
        /// </summary>
        /// <param name="idO">The identifier o.</param>
        /// <param name="idD">The identifier d.</param>
        /// <param name="idR">The identifier r.</param>
        /// <returns></returns>
        public ResponseMessage AddFollower(long idO, long idD, long idR)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                CommonDto commonDto = new CommonDto();
                long idEntAlert = 0;

                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.ent_idDo == idD && r.ent_idDd == idR);
                if (objRel != null)
                {
                    if (objRel.rel_accepted == false)
                    {
                        objRel.rel_offered = false;
                        objRel.rel_accepted = false;
                        objRel.rel_rejected = false;
                        objRel.rel_blocked = false;
                        objRel.rel_following = false;
                        objRel.rel_suggested = true;
                        objRel.rel_timestampModified = DateTime.UtcNow;
                        objRel.rel_timestampResponse = null;
                        objRel.rel_enabled = true;

                        idEntAlert = (long)objRel.ent_idDo;
                        bdContext.SaveChanges();
                    }
                }
                else
                {
                    objRel = new rel_relationship();
                    if (idR > 0) //Recomend
                    {
                        idEntAlert = idD;
                        objRel.ent_idDo = idD;
                        objRel.ent_idDd = idR;
                        objRel.ent_idDr = idO;
                        objRel.rel_timestampSuggested = DateTime.UtcNow;
                        objRel.rel_suggested = true;
                    }
                    else
                    { //Suggest
                        idEntAlert = idD;
                        objRel.ent_idDo = idO;
                        objRel.ent_idDd = idD;
                    }
                    objRel.rel_offered = false;
                    objRel.rel_accepted = false;
                    objRel.rel_rejected = false;
                    objRel.rel_blocked = false;
                    objRel.rel_following = false;
                    objRel.rel_timestampCreated = DateTime.UtcNow;
                    objRel.rel_enabled = true;
                    bdContext.rel_relationship.Add(objRel);
                    bdContext.SaveChanges();
                }

                //Save alert
                commonDto.PutAlert(
                        idEntAlert,
                        objRel.rel_id,
                        (int)Tables.alertTarget.Group
                    );

                response.code = 0;
                response.message = Web_Resource.Message.notifyOK;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Uns the link company.
        /// </summary>
        /// <param name="idSession">The identifier session.</param>
        /// <param name="idGroup">The identifier group.</param>
        /// <returns></returns>
        public ResponseMessage UnLinkCompany(long idSession, long idGroup)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.ent_idDo == idSession && r.ent_idDd == idGroup && r.rel_enabled == true);
                if (objRel != null)
                {
                    objRel.rel_enabled = false;
                    bdContext.SaveChanges();
                    response.code = 0;
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
        /// Finds the companies.
        /// </summary>
        /// <param name="searchstr">The searchstr.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseCompanySearch FindCompanies(
           string searchstr,
           long entIdSession,
           int pageSize)
        {
            ResponseCompanySearch response = new ResponseCompanySearch();

            try
            {
                response.items = from gro in bdContext.SP_FIND_COMPANIES(
                                    searchstr,
                                    entIdSession,
                                    pageSize)
                                 select new CompanySearch()
                                 {
                                     companyId = gro.companyId,
                                     companyName = gro.companyName,
                                     description = "",
                                     profileGuid = (gro.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Company) : gro.profileGuid,
                                     isJoin = Convert.ToBoolean(gro.isJoin)
                                 };
                response.code = 0;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Joins the company.
        /// </summary>
        /// <param name="idFollower">The identifier follower.</param>
        /// <param name="idCompany">The identifier company.</param>
        /// <returns></returns>
        public ResponseCompany JoinCompany(long idFollower, long idCompany)
        {
            ResponseCompany response = new ResponseCompany();
            try
            {
                CommonDto commonDto = new CommonDto();

                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.ent_idDo == idFollower && r.ent_idDd == idCompany && r.rel_timestampResponse == null);
                if (objRel != null)
                {
                    if (objRel.rel_timestampResponse == null)
                    {
                        objRel.rel_offered = false;
                        objRel.rel_accepted = true;
                        objRel.rel_rejected = false;
                        objRel.rel_blocked = false;
                        objRel.rel_following = true;
                        objRel.rel_suggested = false;
                        objRel.rel_timestampModified = DateTime.UtcNow;
                        objRel.rel_timestampResponse = DateTime.UtcNow;
                        objRel.rel_enabled = true;
                        bdContext.SaveChanges();
                    }
                }
                else
                {
                    objRel = new rel_relationship();
                    //Suggest
                    objRel.ent_idDo = idFollower; //Seguidor de la empresa
                    objRel.ent_idDd = idCompany; //Empresa que quiere seguir

                    objRel.rel_offered = false;
                    objRel.rel_accepted = true;
                    objRel.rel_rejected = false;
                    objRel.rel_blocked = false;
                    objRel.rel_following = true;
                    objRel.rel_suggested = false;
                    objRel.rel_timestampCreated = DateTime.UtcNow;
                    objRel.rel_timestampResponse = DateTime.UtcNow;
                    objRel.rel_enabled = true;
                    bdContext.rel_relationship.Add(objRel);
                    bdContext.SaveChanges();
                }

                //Save alert what a person want to participe to group
                commonDto.PutAlert(
                        idCompany,
                        objRel.rel_id,
                        (int)Tables.alertTarget.Follower
                    );

                response.items = GetCompanies(idCompany, 0, 0, 1).items;
                response.code = 0;
                response.message = Web_Resource.Message.notifyOK;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Saves the switch.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public ResponseMessage SaveSwitch(
            long id,
            string name,
            bool value
        )
        {
            ResponseMessage response = new ResponseMessage();

            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);
                if (objEnt != null)
                {
                    switch (name)
                    {
                        case "canPost":
                            objEnt.ent_canPost = value;
                            break;
                    }
                    objEnt.ent_timestampModified = DateTime.UtcNow;
                    bdContext.SaveChanges();
                }
                response.code = 0;
                response.message = Web_Resource.Message.notifyOK;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
