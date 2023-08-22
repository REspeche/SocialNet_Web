namespace Web_BusinessLayer.DTO
{
    using System;
    using System.Linq;
    using Web_BusinessLayer;
    using Enum;
    using Classes;
    using System.Configuration;
    using Helpers;

    public class BaseDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Gets the other user information.
        /// </summary>
        /// <param name="usercode">The usercode.</param>
        /// <returns></returns>
        public ResponseLogin GetOtherUserInfo(string usercode, long userIdSession)
        {
            ResponseLogin response = new ResponseLogin();
            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_profileCode == usercode);
                if (objEnt != null)
                {
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

                    if (objBlo != null) response.view_photoProfile = String.Concat(CommonHelper.getHandlerPath(), objBlo.blo_guid.ToString().ToUpper(), ".", objBlo.blo_extension);
                    else response.view_photoProfile = String.Concat(CommonHelper.getHandlerPath(), String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), objEnt.ent_typeEntity));
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
                    if (objBlo != null) response.view_photoCover = String.Concat(CommonHelper.getHandlerPath(), objBlo.blo_guid.ToString().ToUpper(), ".", objBlo.blo_extension);
                    else response.view_photoCover = String.Concat(CommonHelper.getHandlerPath(), String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Cover));

                    response.view_userId = objEnt.ent_id;
                    response.view_userName = String.Concat(objEnt.ent_firstName + " " + objEnt.ent_lastName);
                    response.view_userCode = objEnt.ent_profileCode;
                    response.view_typeEntity = objEnt.ent_typeEntity;
                    response.view_targetId = (objEnt.ent_idTarget==null)?0: objEnt.ent_idTarget;
                    response.view_canPost = (objEnt.ent_canPost) ? 1 : 0;

                    if (userIdSession > 0)
                    {
                        rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => ((r.ent_idDo == userIdSession && r.ent_idDd == objEnt.ent_id) || (r.ent_idDo == objEnt.ent_id && r.ent_idDd == userIdSession)) && r.rel_accepted == true && r.rel_enabled == true);
                        if (objRel != null)
                        {
                            if (objEnt.ent_typeEntity == (int)Tables.entityType.Person) response.view_isFriend = 1;
                            if (objEnt.ent_typeEntity == (int)Tables.entityType.Group) response.view_isMember = 1;
                            response.view_isFollow = (objRel.rel_following == true) ? 1 : 0;
                        }
                    }

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
    }
}
