using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.DTO
{
    public class CommonDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Gets the badges.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <returns></returns>
        public ResponseBadges GetBadges(long entIdSession)
        {
            ResponseBadges response = new ResponseBadges();

            try
            {
                response = (from bud in bdContext.SP_GET_BADGES(entIdSession)
                            select new ResponseBadges()
                            {
                                all = bud.all,
                                friend = bud.friend,
                                @group = bud.@group,
                                member = bud.member,
                                follower = bud.follower
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
        /// Gets the alerts.
        /// </summary>
        /// <param name="entIdSession">The ent identifier session.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public ResponseAlert GetAlerts(
            long entIdSession,
            int lastId,
            int pageSize)
        {
            ResponseAlert response = new ResponseAlert();

            try
            {
                response.items = from ale in bdContext.SP_GET_ALERTS(
                                        entIdSession,
                                        lastId,
                                        pageSize)
                                 select new AlertItem()
                                 {
                                     id = ale.alertId,
                                     userId = ale.userId,
                                     profileGuid = (ale.profileGuid == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Profile) : ale.profileGuid,
                                     userProfileCode = ale.userProfileCode,
                                     date = ale.alertDate,
                                     description = Commons.getAlertDescription(ale.alertType, new string[] { ale.param1, ale.param2, ale.param3 }),
                                     view = ale.alertView,
                                     type = ale.alertType
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
        /// Puts the alert.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="typeAlert">The type alert.</param>
        /// <returns></returns>
        public ResponseMessage PutAlert(
                long entId,
                long targetId,
                int typeAlert
            )
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                ale_alert objAle = new ale_alert();
                objAle.ent_id = entId;
                objAle.ale_send = false;
                objAle.ale_idTarget = targetId;
                objAle.ale_typeTarget = typeAlert;
                objAle.ale_view = false;
                objAle.ale_timestampCreated = DateTime.UtcNow;
                objAle.ale_enabled = true;
                bdContext.ale_alert.Add(objAle);
                bdContext.SaveChanges();
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
        /// Views the alert.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ResponseMessage ViewAlert(long id)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                ale_alert objAle = bdContext.ale_alert.FirstOrDefault((a) => a.ale_id == id);
                if (objAle != null)
                {
                    objAle.ale_view = true;
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
        /// Views the alert.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        public ResponseMessage ViewAlert(long targetId, long targetType)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                var objAleList = bdContext.ale_alert.Where((a) => a.ale_idTarget == targetId && a.ale_typeTarget == targetType && a.ale_view == false && a.ale_enabled == true).GetEnumerator();
                while (objAleList.MoveNext())
                {
                    ale_alert objAle = objAleList.Current;
                    objAle.ale_view = true;
                }
                bdContext.SaveChanges();
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
        /// Removes the alert.
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        public ResponseMessage RemoveAlert(long targetId, long targetType)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                ale_alert objAle = bdContext.ale_alert.FirstOrDefault((a) => a.ale_idTarget == targetId && a.ale_typeTarget == targetType && a.ale_enabled == true);
                if (objAle != null)
                {
                    objAle.ale_enabled = false;
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
        /// Gets the list country.
        /// </summary>
        /// <returns></returns>
        public ResponseItemCombo GetListCountry()
        {
            ResponseItemCombo response = new ResponseItemCombo();

            try
            {
                response.items = from cou in bdContext.cou_country.AsEnumerable()
                                 where cou.cou_enabled == true
                                 orderby cou.cou_countryName ascending
                                 select new ComboItem()
                                 {
                                     id = cou.cou_id,
                                     label = cou.cou_countryName
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
        /// Inserts the BLOB.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="container">The container.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public long InsertBlob(Guid guid, String extension, int width, int height)
        {
            long idRes = 0;
            try
            {
                blo_blob objBlo = new blo_blob();
                objBlo.blo_guid = guid;
                objBlo.blo_extension = extension;
                objBlo.blo_width = width;
                objBlo.blo_height = height;
                objBlo.blo_timestampCreated = DateTime.UtcNow;
                bdContext.blo_blob.Add(objBlo);
                bdContext.SaveChanges();

                idRes = objBlo.blo_id;
            }
            catch (Exception)
            {
                idRes = -1;
            }
            return idRes;
        }
    }
}
