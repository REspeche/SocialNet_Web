using System;
using System.Collections.Generic;
using System.Linq;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.Enum;

namespace Web_BusinessLayer.DTO
{
    public class PhotoDto
    {
        private SocialNetEntities bdContext = new SocialNetEntities();

        public ResponseOnePost CreateGallery(
            string comment,
            long? idTarget,
            long idDo,
            long idDd,
            int visibility,
            string locationString,
            string locationCode,
            decimal? latitude,
            decimal? longitude,
            bool isSystem
        )
        {
            ResponseOnePost response = new ResponseOnePost();
            try
            {
                evn_event objEvn = bdContext.evn_event.FirstOrDefault((e) => e.ent_idDo == idDo && e.ent_idDd == idDo && e.evn_typeEvent == 4 && e.evn_system == true);
                if (objEvn == null)
                {
                    objEvn = new evn_event();
                    objEvn.evn_comment = comment;
                    objEvn.evn_idTarget = idTarget;
                    objEvn.ent_idDo = idDo;
                    objEvn.ent_idDd = idDd;
                    objEvn.evn_typeEvent = 4; //Gallery
                    objEvn.evn_visibility = visibility;
                    objEvn.evn_locationString = locationString;
                    objEvn.evn_locationCode = locationCode;
                    objEvn.evn_latitude = latitude;
                    objEvn.evn_longitude = longitude;
                    objEvn.evn_externalType = (int)Tables.externalType.None;
                    objEvn.evn_externalLink = null;
                    objEvn.blo_idMaterial = null;
                    objEvn.evn_system = isSystem;
                    objEvn.evn_timestampCreated = DateTime.UtcNow;
                    objEvn.evn_enabled = true;
                    bdContext.evn_event.Add(objEvn);
                }
                bdContext.SaveChanges();

                response.item = new PostItem(objEvn.evn_id);
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
