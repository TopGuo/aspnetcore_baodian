using Microsoft.AspNetCore.Mvc;
using System;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Infrastructrue.Config;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Logs;
using TB.AspNetCore.Infrastructrue.Utils.Encryption;
using TB.AspNetCore.Infrastructrue.Utils.Handler;
using TB.AspNetCore.Infrastructrue.Utils.Http;
using TB.AspNetCore.Infrastructrue.Utils.Path;

namespace TB.AspNetCore.FileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : BaseController
    {
        /// <summary>
        /// 单张图片上传接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult Post([FromBody]FileUploadModel model)
        {
            ResponsResult result = new ResponsResult();
            try
            {
                var ext = System.IO.Path.GetExtension(model.FileName);
                if (string.IsNullOrEmpty(ext))
                {
                    ext = ".jpg";
                }
                string _fileName = $"{model.Type}_{DateTime.Now.ToString("yyyyMMddHHmmssfffffff")}{ext}";
                var _virtual = PathServerUtility.Combine(System.Enum.GetName(typeof(FileType), model.Type), model.Id.ToString(), _fileName);                                
                var webApiPath = ConfigLocator.Instance[TbConstant.WebSiteKey]+ "/api/UploadBase64File";
                var sign = Security.Sign(Domain.Config.TbConstant.UploadKey, _virtual);
                model.Picture = Convert.ToBase64String(ImageHandler.ShrinkImage(ImageHandler.Base64ToBytes(model.Picture)));
                string response;
                if (!string.IsNullOrEmpty(model.WaterMarks))
                {
                    response = HttpUtility.PostString(webApiPath, new { sign = sign, base64 = model.Picture, fileName = _virtual, watermarks = model.WaterMarks }.GetJson(), "application/json");
                }
                else
                {
                    response = HttpUtility.PostString(webApiPath, new { sign = sign, base64 = model.Picture, fileName = _virtual }.GetJson(), "application/json");
                }
                var uploadModel = (response.GetModel<ResponsResult>().Data as Newtonsoft.Json.Linq.JObject).ToObject<UploadModel>();
                result.Data = uploadModel;
                return result;
            }
            catch (Exception ex)
            {
                Log4Net.Error($"[上传图片异常]_:{ex}");
                return result.SetError("上传图片异常");
            }
        }
    }
}