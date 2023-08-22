namespace Web_BusinessLayer.DTO
{
    using System;
    using System.Linq;
    using Web_BusinessLayer;
    using Enum;
    using Classes;
    using System.Configuration;

    public class WallDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Gets the posts.
        /// </summary>
        /// <param name="evnId">The evn identifier.</param>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponsePost GetPosts(
            long evnId,
            long entId,
            long entIdSession,
            int lastId, 
            int pageSize,
            int typeEvent)
        {
            ResponsePost response = new ResponsePost();

            try {
                response.items = from pos in bdContext.SP_GET_POSTS(
                    evnId,
                    entId,
                    entIdSession,
                    lastId, 
                    pageSize,
                    typeEvent)
                        select new PostItem()
                        {
                            postId = pos.postId,
                            postText = Commons.getGalleryTitle(pos.postText),
                            postGuid = pos.postGuid,
                            userId = pos.userId,
                            userName = (pos.visibility == 7) ? Web_Resource.Site.lblAnonymous : pos.userNameO,
                            userNameDes = pos.userNameD,
                            profileGuid = (pos.profileGuid == null || pos.visibility == 7) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), pos.typeEntity) : pos.profileGuid,
                            postCreated = pos.postCreated,
                            width = pos.width,
                            height = pos.height,
                            typeEvent = (pos.typeEvent == null) ? (int)Tables.eventType.Post : (int)pos.typeEvent,
                            userProfileCode = (pos.visibility == 7) ? "" : pos.userProfileCodeO,
                            userProfileCodeDes = pos.userProfileCodeD,
                            postExtType = pos.postExtType,
                            postExtLink = pos.postExtLink,
                            countComments = pos.countComments,
                            countLikes = pos.countLikes,
                            isLike = pos.isLike,
                            lastComments = (pos.lastComments==null)? new CommentItem[] { } : Commons.deserialize<CommentItems>(pos.lastComments).comments,
                            lastPhotos = (pos.lastPhotos == null) ? new PhotoItem[] { } : Commons.deserialize<PhotoItems>(pos.lastPhotos).photos,
                            isSystem = pos.isSystem
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
        /// Gets the comments.
        /// </summary>
        /// <param name="evnIdTarget">The evn identifier target.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseComment GetComments(
            long evnId,
            long evnIdTarget,
            long entIdSession,
            int firstId,
            int pageSize)
        {
            ResponseComment response = new ResponseComment();

            try
            {
                response.items = from pos in bdContext.SP_GET_COMMENTS(
                    evnId,
                    evnIdTarget,
                    entIdSession,
                    firstId,
                    pageSize)
                                 select new CommentItem()
                                 {
                                     postId = pos.postId,
                                     postText = pos.postText,
                                     userId = pos.userId,
                                     userName = (pos.visibility == 7) ? Web_Resource.Site.lblAnonymous : pos.userName,
                                     profileGuid = (pos.profileGuid == null || pos.visibility == 7) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), pos.typeEntity) : pos.profileGuid,
                                     postCreated = pos.postCreated,
                                     userProfileCode = (pos.visibility == 7) ? "" : pos.userProfileCode,
                                     countLikes = pos.countLikes,
                                     isLike = pos.isLike,
                                     typeEntity = pos.typeEntity
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
        /// Writes the posts.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="idTarget">The identifier target.</param>
        /// <param name="idDo">The identifier do.</param>
        /// <param name="idDd">The identifier dd.</param>
        /// <param name="typeEvent">The type event.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="locationString">The location string.</param>
        /// <param name="locationCode">The location code.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="externalType">Type of the external.</param>
        /// <param name="externalLink">The external link.</param>
        /// <param name="idBlobMaterial">The identifier BLOB material.</param>
        /// <returns></returns>
        public ResponsePost WritePosts(
            string comment,
            long id,
            long? idTarget,
            long idDo,
            long idDd,
            int typeEvent,
            int visibility,
            string locationString,
            string locationCode,
            decimal? latitude,
            decimal? longitude,
            int externalType,
            string externalLink,
            long? idBlobMaterial,
            bool isSystem,
            long userIdSession
        )
        {
            ResponsePost response = new ResponsePost();

            try
            {
                evn_event objEvn = new evn_event();
                if (id == 0) //Add new post
                {
                    if (typeEvent == 5 && idDo == idDd)
                    {
                        evn_event objEvnAlbum = bdContext.evn_event.FirstOrDefault((e) => e.ent_idDo == idDo && e.ent_idDd == idDo && e.evn_typeEvent == 4 && e.evn_system == true);
                        if (objEvnAlbum == null)
                        {
                            // Create Album for first time
                            idTarget = new PhotoDto().CreateGallery(
                                "{valGalleryWall}",
                                null,
                                idDo,
                                idDd,
                                visibility,
                                locationString,
                                locationCode,
                                latitude,
                                longitude,
                                true //isSystems
                            ).item.postId;
                        }
                        else
                        {
                            idTarget = objEvnAlbum.evn_id;
                        }
                    }
                    objEvn.evn_comment = comment;
                    objEvn.evn_idTarget = idTarget;
                    objEvn.ent_idDo = idDo;
                    objEvn.ent_idDd = idDd;
                    objEvn.evn_typeEvent = typeEvent;
                    objEvn.evn_visibility = visibility;
                    objEvn.evn_locationString = locationString;
                    objEvn.evn_locationCode = locationCode;
                    objEvn.evn_latitude = latitude;
                    objEvn.evn_longitude = longitude;
                    objEvn.evn_externalType = externalType;
                    objEvn.evn_externalLink = externalLink;
                    objEvn.blo_idMaterial = idBlobMaterial;
                    objEvn.evn_system = isSystem;
                    objEvn.evn_timestampCreated = DateTime.UtcNow;
                    objEvn.evn_enabled = true;
                    bdContext.evn_event.Add(objEvn);
                }
                else //Edit post
                {
                    objEvn = bdContext.evn_event.FirstOrDefault((e) => e.evn_id == id);
                    if (objEvn!=null)
                    {
                        objEvn.evn_comment = comment;
                        objEvn.evn_timestampModified = DateTime.UtcNow;
                    }
                }
                bdContext.SaveChanges();

                //Save alert
                if (idDo != idDd && idDd != userIdSession)
                {
                    ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == idDd);
                    if (objEnt != null)
                    {
                        CommonDto commonDto = new CommonDto();
                        commonDto.PutAlert(
                                (objEnt.ent_typeEntity==(int)Tables.entityType.Group)? (long)objEnt.ent_idTarget:objEnt.ent_id,
                                objEvn.evn_id,
                                (int)Tables.alertTarget.Post
                            );
                    }
                }

                response.items = GetPosts(objEvn.evn_id, 0, 0, 0, 1, 0).items;
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
        /// Writes the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <param name="idTarget">The identifier target.</param>
        /// <param name="idDo">The identifier do.</param>
        /// <param name="idDd">The identifier dd.</param>
        /// <param name="typeEvent">The type event.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="locationString">The location string.</param>
        /// <param name="locationCode">The location code.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="externalType">Type of the external.</param>
        /// <param name="externalLink">The external link.</param>
        /// <param name="idBlobMaterial">The identifier BLOB material.</param>
        /// <returns></returns>
        public ResponseComment WriteComment(
            string comment,
            long? idTarget,
            long idDo,
            long idDd,
            string visibility,
            string locationString,
            string locationCode,
            decimal? latitude,
            decimal? longitude,
            int externalType,
            string externalLink,
            long? idBlobMaterial,
            long userIdSession
        )
        {
            ResponseComment response = new ResponseComment();

            try
            {
                evn_event objEvn = new evn_event();
                objEvn.evn_comment = comment;
                objEvn.evn_idTarget = idTarget;
                objEvn.ent_idDo = idDo;
                objEvn.ent_idDd = idDd;
                objEvn.evn_typeEvent = (int)Tables.eventType.Comment;
                objEvn.evn_visibility = Convert.ToInt32(visibility);
                objEvn.evn_locationString = locationString;
                objEvn.evn_locationCode = locationCode;
                objEvn.evn_latitude = latitude;
                objEvn.evn_longitude = longitude;
                objEvn.evn_externalType = externalType;
                objEvn.evn_externalLink = externalLink;
                objEvn.blo_idMaterial = idBlobMaterial;
                objEvn.evn_system = false;
                objEvn.evn_timestampCreated = DateTime.UtcNow;
                objEvn.evn_enabled = true;
                bdContext.evn_event.Add(objEvn);
                bdContext.SaveChanges();

                //Save alert
                if (idDo != idDd && idDd!= userIdSession)
                {
                    CommonDto commonDto = new CommonDto();
                    commonDto.PutAlert(
                            idDd,
                            objEvn.evn_id,
                            (int)Tables.alertTarget.Comment
                        );
                }

                response.items = GetComments(objEvn.evn_id, 0, 0, 0, 1).items;
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
        /// Removes the posts.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage RemovePosts(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                evn_event objEvn = bdContext.evn_event.FirstOrDefault((e) => e.evn_id == id);
                if (objEvn != null)
                {
                    objEvn.evn_enabled = false;
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
        /// Likes the posts.
        /// </summary>
        /// <param name="idEvn">The identifier evn.</param>
        /// <param name="idEnt">The identifier ent.</param>
        /// <returns></returns>
        public ResponseMessage LikePosts(long idEvn, long idEnt)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                lik_like objLik = bdContext.lik_like.FirstOrDefault((l) => l.evn_id == idEvn && l.ent_id == idEnt);
                if (objLik != null)
                {
                    objLik.lik_enabled = (objLik.lik_enabled)? false :true;
                    bdContext.SaveChanges();
                }
                else
                {
                    objLik = new lik_like();
                    objLik.ent_id = idEnt;
                    objLik.evn_id = idEvn;
                    objLik.lik_typeLike = (int)Tables.likeType.Like;
                    objLik.lik_timestampCreated = DateTime.UtcNow;
                    objLik.lik_enabled = true;
                    bdContext.lik_like.Add(objLik);
                    bdContext.SaveChanges();
                }
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
