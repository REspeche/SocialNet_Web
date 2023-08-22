using Web_BusinessLayer.Classes;
using System.Linq;
using System;
using Web_BusinessLayer.Enum;
using System.Configuration;
using Web_BusinessLayer.Helpers;

namespace Web_BusinessLayer.DTO
{
    public class ProfileDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        /// <summary>
        /// Gets the profile data.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <returns></returns>
        public ResponseProfile GetProfileData(long entId)
        {
            ResponseProfile response = new ResponseProfile();

            try
            {
                response.item = (from pro in bdContext.SP_GET_PROFILE(entId)
                                 select new ProfileItem()
                                {
                                    email = pro.ent_email,
                                    firstname = pro.ent_firstName,
                                    lastname = pro.ent_lastName,
                                    gender = pro.ent_gender,
                                    genderLabel = Web_Resource.Enum.ResourceManager.GetString(((Tables.gender)pro.ent_gender).ToString()),
                                    birthdate = (pro.ent_birthdate==null)?null:((DateTime)pro.ent_birthdate).ToString("dd/MM/yyyy"),
                                    birthdateLabel = (pro.ent_birthdate == null) ? Web_Resource.Enum.ResourceManager.GetString(Tables.gender.Undefined.ToString()) : ((DateTime)pro.ent_birthdate).ToString("dd/MM/yyyy"),
                                    language = pro.lan_id,
                                    languageLabel = Web_Resource.Enum.ResourceManager.GetString(((Tables.language)pro.lan_id).ToString()),
                                    country = pro.cou_id,
                                    countryLabel = (pro.cou_id==null) ? Web_Resource.Enum.ResourceManager.GetString(Tables.gender.Undefined.ToString()) : pro.cou_countryName,
                                    photoProfile = String.Concat(CommonHelper.getHandlerPath(), (pro.photoProfile == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), pro.ent_typeEntity)  : pro.photoProfile),
                                    photoCover = String.Concat(CommonHelper.getHandlerPath(), (pro.photoCover == null) ? String.Format(ConfigurationManager.AppSettings["GuidEmpty"].ToString(), (int)Rules.typePicture.Cover) : pro.photoCover),
                                    canPost = pro.ent_canPost
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
        /// Saves the profile.
        /// </summary>
        /// <param name="entId">The ent identifier.</param>
        /// <param name="firstname">The firstname.</param>
        /// <param name="lastname">The lastname.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="birthdate">The birthdate.</param>
        /// <param name="language">The language.</param>
        /// <param name="country">The country.</param>
        /// <returns></returns>
        public ResponseProfile SaveProfile(
            long id,
            string firstname,
            string lastname,
            int genId,
            string birthdate,
            long lanId,
            long couId
        )
        {
            ResponseProfile response = new ResponseProfile();

            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);
                if (objEnt != null)
                {
                    objEnt.ent_firstName = firstname;
                    objEnt.ent_lastName = lastname;
                    objEnt.ent_gender = genId;
                    if (birthdate != null) objEnt.ent_birthdate = DateTime.Parse(birthdate);
                    objEnt.lan_id = lanId;
                    objEnt.cou_id = couId;
                    bdContext.SaveChanges();

                    cou_country objCou = bdContext.cou_country.FirstOrDefault((c) => c.cou_id == couId);
                    response.item = new ProfileItem();
                    response.item.firstname = firstname;
                    response.item.lastname = lastname;
                    response.item.gender = genId;
                    response.item.birthdate = birthdate;
                    response.item.language = lanId;
                    response.item.country = couId;
                    response.item.genderLabel = Web_Resource.Enum.ResourceManager.GetString(((Tables.gender)genId).ToString());
                    response.item.languageLabel = Web_Resource.Enum.ResourceManager.GetString(((Tables.language)lanId).ToString());
                    response.item.birthdateLabel = (birthdate == null) ? Web_Resource.Enum.ResourceManager.GetString(Tables.gender.Undefined.ToString()) : DateTime.Parse(birthdate).ToString("dd/MM/yyyy");
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
        /// Saves the photo.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="evnId">The evn identifier.</param>
        /// <returns></returns>
        public ResponseProfile SavePhoto(
            long id,
            long evnId
        )
        {
            ResponseProfile response = new ResponseProfile();

            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);
                if (objEnt != null)
                {
                    objEnt.evn_idProfile = evnId;
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

        /// <summary>
        /// Saves the cover.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="evnId">The evn identifier.</param>
        /// <returns></returns>
        public ResponseProfile SaveCover(
            long id,
            long evnId
        )
        {
            ResponseProfile response = new ResponseProfile();

            try
            {
                ent_entity objEnt = bdContext.ent_entity.FirstOrDefault((e) => e.ent_id == id);
                if (objEnt != null)
                {
                    objEnt.evn_idCover = evnId;
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
