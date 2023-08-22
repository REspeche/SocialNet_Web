using System;
using System.Configuration;
using System.Linq;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.DTO
{
    public class FriendDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Gets the counts.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <returns></returns>
        public ResponseFriendBadges GetCounts(
           long entId,
           long entIdSession)
        {
            ResponseFriendBadges response = new ResponseFriendBadges();

            try
            {
                response = (from bud in bdContext.SP_GET_COUNT_FRIEND (
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

        /// <summary>
        /// Gets the friends.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseFriend GetFriends(
           long entId,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseFriend response = new ResponseFriend();

            try
            {
                response.items = from fri in bdContext.SP_GET_FRIENDS(
                                        entId,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                    where fri.rel_accepted==true || fri.rel_blocked==true
                                    select new FriendItem()
                                    {
                                        friendId = fri.friendId,
                                        userId = fri.userId,
                                        userName = fri.userName,
                                        profileGuid = (fri.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : fri.profileGuid,
                                        userProfileCode = fri.userProfileCode,
                                        seggested = fri.rel_suggested,
                                        offered = fri.rel_offered,
                                        following = fri.rel_following,
                                        blocked = fri.rel_blocked
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
        /// Gets the friends send.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseFriend GetFriendsSend(
           long entId,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseFriend response = new ResponseFriend();

            try
            {
                response.items = from fri in bdContext.SP_GET_FRIENDS(
                                        entId,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 where fri.rel_accepted == false
                                            && fri.rel_blocked == false
                                            && fri.rel_rejected == false
                                            && fri.typeAction.Equals("send")
                                 select new FriendItem()
                                 {
                                     friendId = fri.friendId,
                                     userId = fri.userId,
                                     userName = fri.userName,
                                     profileGuid = (fri.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : fri.profileGuid,
                                     userProfileCode = fri.userProfileCode,
                                     seggested = fri.rel_suggested,
                                     offered = fri.rel_offered,
                                     following = fri.rel_following,
                                     blocked = fri.rel_blocked
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
        /// Gets the friends receive.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseFriend GetFriendsReceive(
           long entId,
           long entIdSession,
           int lastId,
           int pageSize)
        {
            ResponseFriend response = new ResponseFriend();

            try
            {
                response.items = from fri in bdContext.SP_GET_FRIENDS(
                                        entId,
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 where fri.rel_accepted == false
                                            && fri.rel_blocked == false
                                            && fri.rel_rejected == false
                                            && fri.typeAction.Equals("receive")
                                 select new FriendItem()
                                 {
                                     friendId = fri.friendId,
                                     userId = fri.userId,
                                     userName = fri.userName,
                                     profileGuid = (fri.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : fri.profileGuid,
                                     userProfileCode = fri.userProfileCode,
                                     seggested = fri.rel_suggested,
                                     offered = fri.rel_offered,
                                     following = fri.rel_following,
                                     blocked = fri.rel_blocked
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
        /// Follows the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage FollowFriend(long id, long userId, long viewUserId)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => 
                                                                                (r.rel_id == id && id>0 && (r.ent_idDo == userId || r.ent_idDd == userId)) || 
                                                                                (id == 0 && r.ent_idDo == userId && r.ent_idDd == viewUserId) || 
                                                                                (id == 0 && r.ent_idDo == viewUserId && r.ent_idDd == userId) && r.rel_enabled==true);
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
        /// Blocks the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage BlockFriend(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.rel_id == id);
                if (objRel != null)
                {
                    objRel.rel_blocked = !objRel.rel_blocked;
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
        /// Removes the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage RemoveFriend(long id)
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
                    commonDto.RemoveAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Friend); //Alert type friend

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
        /// Finds the friends.
        /// </summary>
        /// <param name="searchstr">The searchstr.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseFriendSearch FindFriends(
           string searchstr,
           long entIdSession,
           int pageSize)
        {
            ResponseFriendSearch response = new ResponseFriendSearch();

            try
            {
                response.items = from fri in bdContext.SP_FIND_FRIENDS(
                    searchstr,
                    entIdSession,
                    pageSize)
                                 select new FriendSearch()
                                 {
                                     userId = fri.userId,
                                     userName = fri.userName,
                                     description = fri.country,
                                     profileGuid = (fri.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : fri.profileGuid,
                                     userProfileCode = fri.userProfileCode,
                                     isFriend = Convert.ToBoolean(fri.isFriend)
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
        /// Adds the friend.
        /// </summary>
        /// <param name="idO">The identifier origen.</param>
        /// <param name="idD">The identifier destine.</param>
        /// <param name="idR">The identifier recomend.</param>
        /// <returns></returns>
        public ResponseMessage AddFriend(long idO, long idD, long idR)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                long idEntAlert = 0;

                rel_relationship objRel = bdContext.rel_relationship.FirstOrDefault((r) => r.ent_idDo == idO && r.ent_idDd == idD && r.rel_enabled == true);
                if (objRel != null)
                {
                    if (objRel.rel_accepted == false && objRel.rel_blocked == false) objRel.rel_timestampModified = DateTime.UtcNow;
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
                        idEntAlert = idR;
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
                    CommonDto commonDto = new CommonDto();
                    commonDto.PutAlert(
                            idEntAlert,
                            objRel.rel_id,
                            (int)Tables.alertTarget.Friend
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

        /// <summary>
        /// Accepts the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage AcceptFriend(long id)
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
                    commonDto.ViewAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Friend); //Alert type friend

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
        /// Rejects the friend.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage RejectFriend(long id)
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
                    commonDto.ViewAlert(Convert.ToInt64(id), (int)Tables.alertTarget.Friend); //Alert type friend

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
    }
}
