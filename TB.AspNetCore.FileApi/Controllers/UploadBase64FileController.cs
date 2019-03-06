using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.IO;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Infrastructrue.Config;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Utils.Encryption;
using TB.AspNetCore.Infrastructrue.Utils.Handler;
using TB.AspNetCore.Infrastructrue.Utils.Path;

namespace TB.AspNetCore.FileApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadBase64FileController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="fileName"></param>
        /// <param name="sign"></param>
        /// <param name="watermarks"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult UploadBase64File([FromBody]ReqModel model)
        {
            ResponsResult result = new ResponsResult();

            if (string.IsNullOrWhiteSpace(model.Base64) || string.IsNullOrEmpty(model.FileName))
            {
                var di = PathServerUtility.MapPath(TbConstant.DefaultHeadPicture);
                FileInfo fi = new FileInfo(di);
                UploadModel uploadModel = new UploadModel
                {
                    FileName = fi.Name,
                    FullName = fi.FullName,
                    Extension = fi.Extension,
                    Length = fi.Length,
                    VirtualPath = model.FileName ?? TbConstant.DefaultHeadPicture,
                    FullVirtualPath = PathServerUtility.CombineWithRoot(model.FileName ?? TbConstant.DefaultHeadPicture),
                };
                result.Data = uploadModel;
            }
            else
            {
                if (!Security.ValidSign(model.Sign, model.FileName, TbConstant.UploadKey))
                {
                    return result.SetStatus(ErrorCode.InvalidSign);
                }
                var filePath = PathServerUtility.MapPath(model.FileName);
                FileInfo fi = new FileInfo(filePath);
                var bytes = ImageHandler.Base64ToBytes(model.Base64);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                    if (!string.IsNullOrEmpty(model.Watermarks))
                    {
                        MemoryStream ms = new MemoryStream(bytes);
                        using (Image image = Image.FromStream(ms))
                        {
                            using (Bitmap bitmap = new Bitmap(image.Width, image.Height))
                            {
                                using (Graphics graphics = Graphics.FromImage(bitmap))
                                {
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                                    graphics.Clear(Color.Transparent);

                                    graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0,
                                        image.Width, image.Height, GraphicsUnit.Pixel);
                                    StringFormat strFormat = new StringFormat();
                                    strFormat.Alignment = StringAlignment.Center;
                                    graphics.DrawString(model.Watermarks, new Font("华文彩云", 16, FontStyle.Italic | FontStyle.Bold)
                                        , Brushes.Sienna,
                                        new PointF(image.Width / 2, image.Height - image.Height / 8), strFormat);
                                    bitmap.Save(filePath, ImageFormat.Png);
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (var fs = fi.Create())
                    {
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Flush(true);
                        fi.Refresh();
                    }
                }
                var webSite = ConfigLocator.Instance[TbConstant.WebSiteKey];
                UploadModel uploadModel = new UploadModel
                {
                    FileName = fi.Name,
                    FullName = fi.FullName,
                    Extension = fi.Extension,
                    Length = fi.Length,
                    VirtualPath = model.FileName,
                    FullVirtualPath = PathServerUtility.CombineWithRoot(webSite, model.FileName),
                };
                result.Data = uploadModel;
            }

            return result;
        }
    }

    public class ReqModel
    {

        /// <summary>
        /// 图片base64
        /// </summary>
        public string Base64 { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 水印
        /// </summary>
        public string Watermarks { get; set; }
    }
}