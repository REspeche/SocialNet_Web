using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Web.Helpers;
using Web_BusinessLayer.Classes;
using Web_BusinessLayer.DTO;
using Web_BusinessLayer.Enum;

namespace Web.Controllers
{
    [RoutePrefix("api")]
    public class ApiDefaultController : ApiController
    {
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetStatus()
        {
            ResponseApi responseApi = new ResponseApi();
            responseApi.result = Api.Result.Ok;

            return Ok(responseApi);
        }

        /// <summary>
        /// Posts the verify identity.
        /// </summary>
        /// <param name="paramObj">The parameter object.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("verifyIdentity")]
        [ResponseType(typeof(ResponseApi))]
        public async Task<IHttpActionResult> PostVerifyIdentity(ParamApi paramObj)
        {
            ResponseApi responseApi = new ResponseApi();
            await Task.Factory.StartNew(() =>
            {
                if (VerifyLogin(paramObj, true).code == 0) responseApi.result = Api.Result.Ok; //Login o Register
            });
            return Ok(responseApi);
        }

        /// <summary>
        /// Posts the create post.
        /// </summary>
        /// <param name="paramObj">The parameter object.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("createPost")]
        [ResponseType(typeof(ResponseApi))]
        public async Task<IHttpActionResult> PostCreatePost(ParamApiPost paramObj)
        {
            ResponseApi responseApi = new ResponseApi();
            await Task.Factory.StartNew(() =>
            {
                ResponseLogin responseLogin = VerifyLogin(paramObj);
                if (responseLogin.code == 0) //Login
                {
                    CommonDto commonDto = new CommonDto();

                    string idBlobMaterial = null;
                    if (paramObj.picture != null && !paramObj.picture.Equals(""))
                    {
                        Image imageUpload = paramObj.picture.Base64ToImage();
                        Guid _newGuid = Commons.getNewGuid();
                        string _fileExtension = imageUpload.GetImageFormat();
                        int _fileWidth = imageUpload.Size.Width, _fileHeight = imageUpload.Size.Height;
                        int codeImage = UploadBlobHelper.UploadFileToBlobStorage(
                                            String.Concat(_newGuid.ToString().ToUpper(), ".", _fileExtension),
                                            imageUpload.ToStream(imageUpload.RawFormat));

                        if (codeImage == 0)
                        {
                            idBlobMaterial = commonDto.InsertBlob(
                                                _newGuid,
                                                _fileExtension,
                                                _fileWidth,
                                                _fileHeight
                                                ).ToString();
                        }
                    }

                    string comment = paramObj.text;
                    string id = null;
                    string idTarget = null;
                    string idDo = responseLogin.userId.ToString();
                    string idDd = (paramObj.entityId.Equals(0))? responseLogin.userId.ToString() : paramObj.entityId.ToString();
                    string typeEvent = (paramObj.picture.Equals(""))? ((int)Tables.eventType.Post).ToString(): ((int)Tables.eventType.Photo).ToString();
                    string visibility = (paramObj.anonymous==1)? ((int)Tables.visibility.Anonymous).ToString() : ((int)Tables.visibility.Public).ToString();
                    string locationString = null;
                    string locationCode = null;
                    string latitude = null;
                    string longitude = null;
                    string externalType = null;
                    string externalLink = null;

                    long _id = (id != null && !id.Equals("")) ? Convert.ToInt64(id) : 0;
                    long? _idTarget = (idTarget != null && !idTarget.Equals("")) ? Convert.ToInt64(idTarget) : (long?)null;
                    string _locationString = (locationString != null && !locationString.Equals("")) ? locationString : null;
                    string _locationCode = (locationCode != null && !locationCode.Equals("")) ? locationCode : null;
                    decimal? _latitude = (latitude != null && !latitude.Equals("")) ? Convert.ToDecimal(latitude) : (decimal?)null;
                    decimal? _longitude = (longitude != null && !longitude.Equals("")) ? Convert.ToDecimal(longitude) : (decimal?)null;
                    string _externalLink = (externalLink != null && !externalLink.Equals("")) ? externalLink : null;
                    long? _idBlobMaterial = (idBlobMaterial != null && !idBlobMaterial.Equals("")) ? Convert.ToInt64(idBlobMaterial) : (long?)null;

                    WallDto wallDto = new WallDto();
                    ResponsePost response = wallDto.WritePosts(
                        comment,
                        _id,
                        _idTarget,
                        Convert.ToInt64(idDo),
                        Convert.ToInt64(idDd),
                        Convert.ToInt32(typeEvent),
                        Convert.ToInt32(visibility),
                        _locationString,
                        _locationCode,
                        _latitude,
                        _longitude,
                        Convert.ToInt32(externalType),
                        _externalLink,
                        _idBlobMaterial,
                        false,
                        responseLogin.userId
                        );

                    if (response.code==0) responseApi.result = Api.Result.Ok;
                }
            });
            return Ok(responseApi);
        }

        /// <summary>
        /// Posts the list entities.
        /// </summary>
        /// <param name="paramObj">The parameter object.</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("listEntities")]
        [ResponseType(typeof(ResponseApiListEntities))]
        public async Task<IHttpActionResult> PostListEntities(ParamApi paramObj)
        {
            ResponseApiListEntities responseApi = new ResponseApiListEntities();
            await Task.Factory.StartNew(() =>
            {
                ResponseLogin responseLogin = VerifyLogin(paramObj);
                if (responseLogin.code == 0) //Login
                {
                    AccountDto accountDto = new AccountDto();
                    responseApi.items = accountDto.GetListEntities(responseLogin.userId);
                    responseApi.result = Api.Result.Ok;
                }
            });
            return Ok(responseApi);
        }

        // Private functions ----------------------------------------------------

        /// <summary>
        /// Verifies the login.
        /// </summary>
        /// <param name="paramObj">The parameter object.</param>
        /// <returns></returns>
        private ResponseLogin VerifyLogin(ParamApi paramObj, bool register = false)
        {
            var accountDto = new AccountDto();
            ResponseLogin response = new ResponseLogin();
            response.code = -1; //fail
            if (paramObj.idFB != null && !paramObj.idFB.Equals("")) {
                if (paramObj.user != null && paramObj.idFB != null)
                {
                    response = accountDto.ValidateLoginFacebook(paramObj.user, paramObj.idFB, null, null, null, null, null, register);
                }
            }
            else {
                if (paramObj.user != null && paramObj.password != null)
                {
                    response = accountDto.ValidateLogin(paramObj.user, paramObj.password);
                }
            }
            return response;
        }
    }
}