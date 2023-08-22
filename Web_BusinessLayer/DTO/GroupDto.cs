using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.DTO
{
    public class GroupDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Gets the counts.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <returns></returns>
        public ResponseGroupBadges GetCounts(
           long entIdGroup, //group id
           long entId)
        {
            ResponseGroupBadges response = new ResponseGroupBadges();

            try
            {
                response = (from bud in bdContext.SP_GET_COUNT_GROUP(
                                        entIdGroup,
                                        entId)
                            select new ResponseGroupBadges()
                            {
                                countAll = bud.countAll,
                                countSend = bud.countSend,
                                countReceive = bud.countReceive
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
        /// Gets the groups.
        /// </summary>
        /// <param name="entIdGroup">The ent identifier group.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseGroup GetGroups(
           long entIdGroup,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseGroup response = new ResponseGroup();

            try
            {
                response.items = from gro in bdContext.SP_GET_GROUPS(
                                        entIdGroup,
                                        entIdSession,
                                        lastId,
                                        pageSize,
                                        true,
                                        null,
                                        null,
                                        null)
                                 select new GroupItem()
                                 {
                                     joinId = gro.joinId,
                                     groupId = gro.groupId,
                                     groupName = gro.groupName,
                                     profileGuid = (gro.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Group) : gro.profileGuid,
                                     profileCode = gro.groupProfileCode,
                                     seggested = gro.rel_suggested,
                                     following = gro.rel_following,
                                     countMember = gro.countMember,
                                     isMe = (gro.isMe==1)? true : false
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
        /// Gets the members.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseMember GetMembers(
           long entId,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseMember response = new ResponseMember();

            try
            {
                response.items = from mem in bdContext.SP_GET_MEMBERS(
                                        entId,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 where mem.typeAction.Equals("all")
                                 select new MemberItem()
                                 {
                                     friendId = mem.memberId,
                                     userId = mem.userId,
                                     userName = mem.userName,
                                     profileGuid = (mem.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : mem.profileGuid,
                                     userProfileCode = mem.userProfileCode,
                                     seggested = mem.rel_suggested,
                                     offered = mem.rel_offered,
                                     following = mem.rel_following,
                                     blocked = mem.rel_blocked,
                                     isFriend = mem.isFriend
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
        /// Gets the members send.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseMember GetMembersSend(
           long entId,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseMember response = new ResponseMember();

            try
            {
                response.items = from mem in bdContext.SP_GET_MEMBERS(
                                        entId,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 where mem.typeAction.Equals("send")
                                 select new MemberItem()
                                 {
                                     friendId = mem.memberId,
                                     userId = mem.userId,
                                     userName = mem.userName,
                                     profileGuid = (mem.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : mem.profileGuid,
                                     userProfileCode = mem.userProfileCode,
                                     seggested = mem.rel_suggested,
                                     offered = mem.rel_offered,
                                     following = mem.rel_following,
                                     blocked = mem.rel_blocked,
                                     isFriend = mem.isFriend
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
        /// Gets the members receive.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseMember GetMembersReceive(
           long entId,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseMember response = new ResponseMember();

            try
            {
                response.items = from mem in bdContext.SP_GET_MEMBERS(
                                        entId,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 where mem.typeAction.Equals("receive")
                                 select new MemberItem()
                                 {
                                     friendId = mem.memberId,
                                     userId = mem.userId,
                                     userName = mem.userName,
                                     profileGuid = (mem.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : mem.profileGuid,
                                     userProfileCode = mem.userProfileCode,
                                     seggested = mem.rel_suggested,
                                     offered = mem.rel_offered,
                                     following = mem.rel_following,
                                     blocked = mem.rel_blocked,
                                     isFriend = mem.isFriend
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
        /// Gets the groups suggested.
        /// </summary>
        /// <param name="entIdGroup">The ent identifier group.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseGroup GetGroupsSuggested(
           long entIdGroup,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseGroup response = new ResponseGroup();

            try
            {
                response.items = from gro in bdContext.SP_GET_GROUPS(
                                        entIdGroup,
                                        entIdSession,
                                        lastId,
                                        pageSize,
                                        false,
                                        false,
                                        true,
                                        null)
                                 where gro.rel_accepted == false
                                            && gro.rel_rejected == false
                                            && gro.rel_suggested == true
                                 select new GroupItem()
                                 {
                                     joinId = gro.joinId,
                                     groupId = gro.groupId,
                                     groupName = gro.groupName,
                                     profileGuid = (gro.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Group) : gro.profileGuid,
                                     profileCode = gro.groupProfileCode,
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
        /// Gets the groups invited.
        /// </summary>
        /// <param name="entIdGroup">The ent identifier group.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseGroup GetGroupsInvited(
           long entIdGroup,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseGroup response = new ResponseGroup();

            try
            {
                response.items = from gro in bdContext.SP_GET_GROUPS(
                                        entIdGroup,
                                        entIdSession,
                                        lastId,
                                        pageSize,
                                        false,
                                        false,
                                        null,
                                        1)
                                 where gro.rel_accepted == false
                                            && gro.rel_rejected == false
                                            && gro.typeAction.Equals("invite")
                                 select new GroupItem()
                                 {
                                     joinId = gro.joinId,
                                     groupId = gro.groupId,
                                     groupName = gro.groupName,
                                     profileGuid = (gro.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Group) : gro.profileGuid,
                                     profileCode = gro.groupProfileCode,
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
        /// Gets the friend groups.
        /// </summary>
        /// <param name="entIdGroup">The ent identifier group.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseGroup GetFriendGroups(
           long entIdGroup,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseGroup response = new ResponseGroup();

            try
            {
                response.items = from gro in bdContext.SP_GET_GROUPSFRIEND(
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 select new GroupItem()
                                 {
                                     joinId = gro.joinId,
                                     groupId = gro.groupId,
                                     groupName = gro.groupName,
                                     profileGuid = (gro.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Group) : gro.profileGuid,
                                     profileCode = gro.groupProfileCode,
                                     countMember = gro.countMember
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
        /// Follows the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage FollowGroup(long id, long userId, long viewUserId)
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
        /// Removes the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage RemoveGroup(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                ent_entity objGro = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);
                if (objGro != null)
                {
                    objGro.ent_enabled = false;
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
        /// Uns the link group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage UnLinkGroup(long idSession, long idGroup)
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
        /// Finds the groups.
        /// </summary>
        /// <param name="searchstr">The searchstr.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseGroupSearch FindGroups(
           string searchstr,
           long entIdSession,
           int pageSize)
        {
            ResponseGroupSearch response = new ResponseGroupSearch();

            try
            {
                response.items = from gro in bdContext.SP_FIND_GROUPS(
                                    searchstr,
                                    entIdSession,
                                    pageSize)
                                 select new GroupSearch()
                                 {
                                     groupId = gro.groupId,
                                     groupName = gro.groupName,
                                     description = "",
                                     profileGuid = (gro.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Group) : gro.profileGuid,
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
        /// Joins the group.
        /// </summary>
        /// <param name="idMember">The identifier member.</param>
        /// <param name="idGroup">The identifier group.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="send">if set to <c>true</c> [send].</param>
        /// <returns></returns>
        public ResponseGroup JoinGroup(
            long idMember, 
            long idGroup,
            long entIdSession,
            bool send)
        {
            ResponseGroup response = new ResponseGroup();
            try
            {
                long idAdministrator = 0;
                CommonDto commonDto = new CommonDto();

                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.ent_idDo == idMember && r.ent_idDd == idGroup && r.rel_timestampResponse == null);
                if (objRel != null)
                {
                    if (objRel.rel_timestampResponse == null)
                    {
                        objRel.rel_offered = false;
                        objRel.rel_accepted = false;
                        objRel.rel_rejected = false;
                        objRel.rel_blocked = false;
                        objRel.rel_following = false;
                        objRel.rel_suggested = send;
                        objRel.rel_timestampModified = DateTime.UtcNow;
                        objRel.rel_timestampResponse = null;
                        objRel.rel_enabled = true;

                        idAdministrator = (long)objRel.ent_entity1.ent_idTarget;
                        bdContext.SaveChanges();
                    }
                }
                else
                {
                    objRel = new rel_relationship();
                    //Suggest
                    objRel.ent_idDo = idMember; //Participante del grupo
                    objRel.ent_idDd = idGroup; //Grupo al que quiere participar
                    if (idMember != entIdSession)
                    {
                        objRel.ent_idDr = entIdSession;
                        objRel.rel_suggested = send;
                    }

                    objRel.rel_offered = false;
                    objRel.rel_accepted = false;
                    objRel.rel_rejected = false;
                    objRel.rel_blocked = false;
                    objRel.rel_following = false;                   
                    objRel.rel_timestampCreated = DateTime.UtcNow;
                    objRel.rel_timestampResponse = null;
                    objRel.rel_enabled = true;
                    bdContext.rel_relationship.Add(objRel);

                    idAdministrator = (
                                        from ent in bdContext.ent_entity.AsEnumerable()
                                        where ent.ent_typeEntity == 3 && ent.ent_id == idGroup
                                        select (ent.ent_idTarget == null) ? 0 : (long)ent.ent_idTarget
                                    ).First();
                    bdContext.SaveChanges();
                }

                //Save alert what a person want to participe to group
                commonDto.PutAlert(
                        idAdministrator,
                        objRel.rel_id,
                        (int)Tables.alertTarget.Member
                    );

                GroupItem groupItem = new GroupItem();
                groupItem.joinId = objRel.rel_id;
                List<GroupItem> listItem = new List<GroupItem>();
                listItem.Add(groupItem);

                response.items = listItem;
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
        /// Adds the group.
        /// </summary>
        /// <param name="idTarget">The identifier target.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public ResponseGroup AddGroup(
            long idTarget,
            string name
        )
        {
            ResponseGroup response = new ResponseGroup();
            try
            {
                ent_entity objEntTarget = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == idTarget);

                //add entity like a group
                ent_entity objEnt = new ent_entity();
                objEnt.ent_idTarget = idTarget;
                objEnt.ent_firstName = name;
                objEnt.ent_lastName = String.Empty;
                objEnt.ent_email = null;
                objEnt.ent_password = null;
                if (objEntTarget != null)
                {
                    objEnt.cou_id = objEntTarget.cou_id;
                    objEnt.lan_id = objEntTarget.lan_id;
                }
                objEnt.ent_gender = (int?)Tables.gender.Undefined;
                objEnt.ent_typeEntity = (int?)Tables.entityType.Group;
                objEnt.ent_close = false;
                objEnt.ent_active = true;
                objEnt.ent_profileCode = Commons.getCodeProfile();
                objEnt.ent_timestampCreated = DateTime.UtcNow;
                objEnt.ent_enabled = true;
                bdContext.ent_entity.Add(objEnt);

                // add relationship
                rel_relationship objRel = new rel_relationship();
                objRel.ent_idDo = idTarget;
                objRel.ent_idDd = objEnt.ent_id;
                objRel.rel_offered = false;
                objRel.rel_accepted = true;
                objRel.rel_rejected = false;
                objRel.rel_blocked = false;
                objRel.rel_following = true;
                objRel.rel_suggested = false;
                objRel.rel_timestampCreated = DateTime.UtcNow;
                objRel.rel_enabled = true;
                bdContext.rel_relationship.Add(objRel);
                bdContext.SaveChanges();

                response.code = 0;
                response.items = GetGroups(objEnt.ent_id, 0, 0, 1).items;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Accepts the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage AcceptGroup(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.rel_id == id);
                if (objRel != null)
                {
                    objRel.rel_accepted = true;
                    objRel.rel_rejected = false;
                    objRel.rel_timestampResponse = DateTime.UtcNow;
                    objRel.rel_following = true;
                    bdContext.SaveChanges();

                    CommonDto commonDto = new CommonDto();
                    commonDto.ViewAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Group); //Alert type group

                    response.code = 0;
                    response.message = Web_Resource.Message.notifyOK;
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
        /// Rejects the group.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage RejectGroup(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.rel_id == id);
                if (objRel != null)
                {
                    objRel.rel_accepted = false;
                    objRel.rel_rejected = true;
                    objRel.rel_timestampResponse = DateTime.UtcNow;
                    objRel.rel_following = false;
                    bdContext.SaveChanges();

                    CommonDto commonDto = new CommonDto();
                    commonDto.ViewAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Group); //Alert type group

                    response.code = 0;
                    response.message = Web_Resource.Message.notifyOK;
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
        /// <param name="groupname">The groupname.</param>
        /// <param name="createdate">The createdate.</param>
        /// <param name="couId">The cou identifier.</param>
        /// <returns></returns>
        public ResponseProfile SaveProfile(
            long id,
            string groupname,
            string createdate,
            long couId
        )
        {
            ResponseProfile response = new ResponseProfile();

            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);
                if (objEnt != null)
                {
                    objEnt.ent_firstName = groupname;
                    objEnt.ent_birthdate = DateTime.Parse(createdate);
                    objEnt.cou_id = couId;
                    bdContext.SaveChanges();

                    cou_country objCou = bdContext.cou_country.FirstOrDefault((c) => c.cou_id == couId);
                    response.item = new ProfileItem();
                    response.item.firstname = groupname;
                    response.item.birthdate = createdate;
                    response.item.country = couId;
                    response.item.birthdateLabel = (createdate == null) ? Web_Resource.Enum.ResourceManager.GetString(Tables.gender.Undefined.ToString()) : DateTime.Parse(createdate).ToString("dd/MM/yyyy");
                    if (objCou != null) response.item.countryLabel = objCou.cou_countryName;
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
        /// Adds the member.
        /// </summary>
        /// <param name="idO">The identifier o.</param>
        /// <param name="idD">The identifier d.</param>
        /// <param name="idR">The identifier r.</param>
        /// <returns></returns>
        public ResponseMessage AddMember(long idO, long idD, long idR)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                CommonDto commonDto = new CommonDto();
                long idEntAlert = 0;

                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.ent_idDo == idD && r.ent_idDd == idR);
                if (objRel != null)
                {
                    if (objRel.rel_accepted == false) objRel.rel_timestampModified = DateTime.UtcNow;
                    else objRel = null;
                }
                else
                {
                    objRel = new rel_relationship();
                    objRel.rel_timestampCreated = DateTime.UtcNow;
                    bdContext.rel_relationship.Add(objRel);
                }

                if (objRel != null)
                {
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
                    objRel.rel_timestampResponse = null;
                    objRel.rel_enabled = true;
                    bdContext.SaveChanges();

                    //Save alert
                    commonDto.PutAlert(
                            idEntAlert,
                            objRel.rel_id,
                            (int)Tables.alertTarget.Group
                        );
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

        public ResponseMessage CancelJoin(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.rel_id == id);
                if (objRel != null)
                {
                    objRel.rel_enabled = false;
                    bdContext.SaveChanges();

                    CommonDto commonDto = new CommonDto();
                    commonDto.RemoveAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Member); //Alert type group

                    response.code = 0;
                    response.message = Web_Resource.Message.notifyOK;
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
        /// Accepts the member.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage AcceptMember(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.rel_id == id);
                if (objRel != null)
                {
                    objRel.rel_accepted = true;
                    objRel.rel_rejected = false;
                    objRel.rel_timestampResponse = DateTime.UtcNow;
                    objRel.rel_following = true;
                    bdContext.SaveChanges();

                    CommonDto commonDto = new CommonDto();
                    commonDto.ViewAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Member); //Alert type member

                    response.code = 0;
                    response.message = Web_Resource.Message.notifyOK;
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
        /// Rejects the member.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage RejectMember(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.rel_id == id);
                if (objRel != null)
                {
                    objRel.rel_accepted = false;
                    objRel.rel_rejected = true;
                    objRel.rel_timestampResponse = DateTime.UtcNow;
                    objRel.rel_following = false;
                    bdContext.SaveChanges();

                    CommonDto commonDto = new CommonDto();
                    commonDto.ViewAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Member); //Alert type member

                    response.code = 0;
                    response.message = Web_Resource.Message.notifyOK;
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
        /// Gets the counts members.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <returns></returns>
        public ResponseFriendBadges GetCountsMembers(
           long entId,
           long entIdSession)
        {
            ResponseFriendBadges response = new ResponseFriendBadges();

            try
            {
                response = (from bud in bdContext.SP_GET_COUNT_MEMBER(
                                        entId,
                                        entIdSession)
                            select new ResponseFriendBadges()
                            {
                                countAll = bud.countAll,
                                countSend = bud.countSend,
                                countReceive = bud.countReceive
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
    }
}
