using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Models.Web;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Utils.Handler;
using TB.AspNetCore.Infrastructrue.Utils.Http;
using TB.AspNetCore.Infrastructrue.Utils.Path;

namespace TB.AspNetCore.WebSite.Controllers
{

    [Produces("application/json")]
    [Route("webapi/[action]")]
    public class WebApiController : WebBase
    {
        public IAccountService accountService { get; set; }

        public IPermissionService _permissionService { get; set; }

        public ITaskService _taskService { get; set; }

        public ICommonService _commonService { get; set; }

        public IMemberService _memberService { get; set; }

        #region 帮助管理
        [HttpPost]
        public ResponsResult SaveHelp([FromBody]InformationModel model)
        {
            return _commonService.SaveHelp(model);
        }

        public ResponsResult GetInformationByType(InformationType type)
        {
            return _commonService.ApiGetInformation(type);
        }
        public ResponsResult GetInformation(InformationType type, string id)
        {
            return _commonService.GetInformation(type, id);
        }
        #endregion


        #region 保存系统配置

        /// <summary>
        /// 保存系统设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult SaveSystemSetting([FromBody]SystemSettingModel model)
        {
            return _commonService.SaveSetting(model);
        }
        [HttpPost]
        public ResponsResult SaveAndroid([FromBody]AndroidVersion android)
        {
            return _commonService.SaveAndroid(android);
        }
        [HttpPost]
        public ResponsResult SaveIOS([FromBody]IosVersion ios)
        {
            return _commonService.SaveIOS(ios);
        }
        #endregion

        #region 邀请好友注册
        /// <summary>
        /// 邀请好友注册
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ResponsResult Register([FromBody]RegisterModel model)
        {
            return _memberService.Register(model.Id, model.Mobile, model.Code);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ResponsResult GetValidCode([FromBody]RegisterModel model)
        {
            return _commonService.GetValidCode(model.Mobile);
        }

        /// <summary>
        /// 获取短信信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult GetSMSList([FromBody]SmsSearchModel model)
        {
            return _commonService.GetSMSList(model);
        }
        #endregion

        #region 广告
        [HttpPost]
        public ResponsResult AdManagerList([FromBody]AdManagerViewSearch model)
        {
            return _commonService.AdManagerList(model);
        }
        [HttpPost]
        public ResponsResult DelAdPic([FromBody]AdManagerViewSearch model)
        {
            return _commonService.DelAdPic(model);
        }

        /// <summary>
        /// 禁用启用
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult IsEnableAdPic([FromBody]AdManagerViewSearch model)
        {
            return _commonService.IsEnableAdPic(model);
        }
        [HttpPost]
        public ResponsResult AdPicAddUpdata([FromBody]AdManagerViewSearch model)
        {
            if (model==null)
            {
                return new ResponsResult().SetStatus(ErrorCode.CannotEmpty);
            }
            if (!string.IsNullOrEmpty(model.AdPic) && model.AdPic.Length > 1000)
            {
                DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var fileName = (long)((DateTime.Now.AddHours(-8) - Jan1st1970).TotalMilliseconds);

                model.AdPic = ImageHandler.SaveBase64Image(model.AdPic, $"{fileName}.png", TB.AspNetCore.Application.Services.SystemSettingService.SystemSetting.AdImagePath);
            }
            return _commonService.AdPicAdd_Updata(model);
        }
        #endregion
        #region 登录模块
        [AllowAnonymous]
        [HttpPost]
        public ResponsResult Login([FromBody]BackstageUserAdd model)
        {
            return accountService.Login(model);
        }

        [HttpGet]
        public ResponsResult Logout()
        {
            ResponsResult result = new ResponsResult();
            return accountService.LogoutUser();
        }
        #endregion

        #region 添加后台管理员
        [HttpPost]
        public ResponsResult AddMemberAdd_Update([FromBody]BackstageUserAdd model)
        {
            if (!string.IsNullOrEmpty(model.Id))
            {
                return accountService.UpdateAccount(model);
            }
            return accountService.AddAccount(model);
        }
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult MemberPwd_Update([FromBody]BackstageUserAdd model)
        {
            return accountService.UpdatePwd(model);
        }
        [HttpGet]
        public ResponsResult GetBackstageUser(string id)
        {
            return accountService.GetBackstageUser(id);
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult SetMemberState([FromBody]BackstageUserAdd model)
        {
            if (model.AccountStatus == AccountStatus.Normal)
            {
                model.AccountStatus = AccountStatus.Disabled;
            }
            else
            {
                model.AccountStatus = AccountStatus.Normal;
            }
            return accountService.UpdateAccount(model);
        }
        #endregion

        #region 后台用户列表
        public ResponsResult BackstageUser([FromBody]AccountSearchModel model)
        {
            return accountService.GetBackstageUserList(model);
        }
        #endregion

        #region 角色模块
        [HttpPost]
        public ResponsResult GetRoles()
        {
            return _permissionService.GetRoles();
        }
        [HttpPost]
        public ResponsResult SaveRoles([FromBody]RoleModel model)
        {
            return _permissionService.SaveRoles(model);
        }
        [HttpPost]
        public ResponsResult DeleteRoles([FromBody]RoleModel model)
        {
            return _permissionService.DeleteRoles(model);
        }
        #endregion

        #region 计划任务模块
        [HttpPost]
        public ResponsResult GetTaskList()
        {
            return _taskService.GetTaskList();
        }
        [HttpPost]
        public async Task<ResponsResult> AddTaskAsync([FromBody]TaskScheduleModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.Id) && !model.Id.Equals("-1"))
            {
                return await _taskService.ModifyTaskAsync(model);
            }
            return await _taskService.AddTaskAsync(model);
        }

        public async Task<ResponsResult> DelTaskAsync([FromBody]TaskScheduleModel model)
        {
            return await _taskService.DelTaskAsync(model);
        }
        #endregion
        #region Editor
        [AllowAnonymous]
        public object Upload()
        {
            object returnValue = null;
            string path = this.Params("path");
            if (string.IsNullOrEmpty(path))
            {
                path = TbConstant.UPLOAD_TEMP_PATH;// Request.QueryString["path"];
            }
            switch (Request.Query["action"])
            {
                case "Local":
                    returnValue = uploadLocal(path);
                    break;
                case "Remote":
                    returnValue = new { list = uploadRemote(path) };
                    break;
                case "config":
                    returnValue = new { url = "url", title = "title", original = "original", state = "SUCCESS" };
                    break;
            }

            //向浏览器返回数据json数据
            return returnValue;
        }
        //文件允许格式
        private readonly string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };
        //文件大小限制，单位KB
        private const int size = 1024 * 2;
        object uploadLocal(string path)
        {

            //文件上传状态,初始默认成功，可选参数{"SUCCESS","ERROR","SIZE","TYPE"}
            string state = "SUCCESS";

            string title = string.Empty;
            string oriName = string.Empty;
            string filename = string.Empty;
            string url = string.Empty;
            string currentType = string.Empty;

            //保存路径
            string uploadpath = PathServerUtility.MapPath(path);

            try
            {
                IFormFile uploadFile = Request.Form.Files[0];
                title = uploadFile.FileName.ToLower();

                //目录验证
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }

                //格式验证
                string[] temp = uploadFile.FileName.Split('.');
                currentType = "." + temp[temp.Length - 1].ToLower();
                if (Array.IndexOf(filetype, currentType) == -1)
                {
                    state = "文件类型不正确";
                }
                else               //大小验证
                if (uploadFile.Length / 1024 > size)
                {
                    state = $"文件不能超过{size / 1024}M";
                }
                //获取图片描述
                if (Request.Query["pictitle"].Count > 0)
                {
                    if (!String.IsNullOrEmpty(Request.Query["pictitle"]))
                    {
                        title = Request.Query["pictitle"];
                    }
                }
                //获取原始文件名
                if (Request.Query["fileName"].Count > 0)
                {
                    if (!String.IsNullOrEmpty(Request.Query["fileName"]))
                    {
                        oriName = Request.Query["fileName"].ToString().Split(',')[1];
                    }
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    if (System.IO.File.Exists(Path.Combine(uploadpath, title)))
                    {
                        state = "ERROR";
                    }
                    else
                    {
                        filename = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + currentType;
                        FileInfo fi = new FileInfo(Path.Combine(uploadpath, filename));
                        using (var fs = fi.OpenWrite())
                        {
                            uploadFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    url = PathServerUtility.CombineWithRoot(path, filename);
                }
            }
            catch (Exception)
            {
                state = "ERROR";
            }


            return new { url, title, original = oriName, state };
        }
        class upload
        {
            public string source { get; set; }
            public string state { get; set; }
            public string url { get; set; }
        }
        List<upload> uploadRemote(string path)
        {
            List<upload> list = new List<upload>();

            string savePath = PathServerUtility.MapPath(path);       //保存文件地址
                                                                     //目录验证
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string uri = System.Net.WebUtility.UrlDecode(this.Params("upfile"));
            string[] imgUrls = null;
            if (string.IsNullOrEmpty(uri))
            {
                uri = System.Net.WebUtility.UrlDecode(this.Params("upfile[]"));
                imgUrls = Regex.Split(uri, ",", RegexOptions.IgnoreCase);
            }
            else
            {
                uri = uri.Replace("&amp;", "&");
                imgUrls = Regex.Split(uri, "ue_separate_ue", RegexOptions.IgnoreCase);
            }
            for (int i = 0, len = imgUrls.Length; i < len; i++)
            {


                try
                {
                    var imgUrl = imgUrls[i];
                    var upload = new upload { source = imgUrl, state = "SUCCESS" };
                    if (!imgUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                        && !imgUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    //格式验证
                    int temp = imgUrl.LastIndexOf('.');
                    var ext = imgUrl.Substring(temp).ToLower();
                    if (Array.IndexOf(filetype, ext) == -1)
                    {
                        upload.state = "ERROR";
                        continue;
                    }
                    var stream = HttpUtility.DownBytes(imgUrl);
                    if (stream == null || stream.Length == 0)
                    {
                        continue;
                    }
                    var tmpName = $"{DateTime.Now.ToString("yyyyMMddHHmmssffffff")}_{i}{ext}";
                    //写入文件
                    using (FileStream fs = new FileStream(Path.Combine(savePath, tmpName), FileMode.CreateNew))
                    {
                        //stream.CopyTo(fs);
                        fs.Write(stream, 0, stream.Length);
                        fs.Flush();
                    }

                    upload.url = PathServerUtility.CombineWithRoot(path, tmpName);
                    list.Add(upload);
                }
                catch
                {

                }
            }
            return list;
        }

        #endregion
    }
}