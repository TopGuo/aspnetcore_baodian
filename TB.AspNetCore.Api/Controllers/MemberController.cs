using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Services;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Utils.Encryption;

namespace TB.AspNetCore.Api.Controllers
{
    /// <summary>
    /// 会员模块
    /// </summary>
    [Route("api/Member/[action]")]
    [ApiController]
    public class MemberController : ApiBaseController
    {
        ICommonService _commonService { get; set; }
        IMemberService _memberService { get; set; }
        //
        /// <summary>
        /// 邀请好友-web页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponsResult SharedLink()
        {
            var settings = SystemSettingService.SystemSetting;
            this.TokenModel.Id = "xx90yyx";
            return new ResponsResult
            {
                Data = new ApiSharedModel
                {
                    Link = $"Register/{this.TokenModel.Id}".GetPWFullPath(settings.WebSite),
                    Code = this.TokenModel.Id,
                    InvitedAmount = settings.InvitedPackage,
                    InviteeAmount = settings.InviteePackage,
                    Content = "分享内容",
                    Picture = "/Images/baodian28.png".GetPWFullPath(settings.WebSite)
                }
            };
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile">手机号(包括166)</param>
        /// <param name="sign">加密参数[手机号,apiKey]</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult GetCode(string mobile, string sign)
        {
            //TODO:this.ApiKey
            string Android_ApiKey = "506DDE1D-4642-4859-9226-E26B098FC02B";
            //this.ApiKey = Android_ApiKey;
            return _commonService.ApiGetVerifyCode(mobile, sign, Android_ApiKey);
        }

        //
        /// <summary>
        /// 生成签名(仿客户端)
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>
        /// <param name="type">1:WeChatApp_ApiKey 2:iOS_ApiKey 3:Android_ApiKey</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult GenSSign(string mobile, string code, int type)
        {
            var apikey = string.Empty;
            string WeChatApp_ApiKey = "23ACA89E-0169-44A1-8D08-64AD3EC4A551";
            string iOS_ApiKey = "972F5D56-296B-4BD7-8CB6-49617A441A99";
            string Android_ApiKey = "506DDE1D-4642-4859-9226-E26B098FC02B";

            if (type == 1)
            {
                apikey = WeChatApp_ApiKey;
            }
            else if (type == 2)
            {
                apikey = iOS_ApiKey;
            }
            else if (type == 3)
            {
                apikey = Android_ApiKey;
            }
            ResponsResult result = new ResponsResult
            {
                Data = Security.Sign(mobile, code, apikey)
            };
            return result;
        }

        //
        /// <summary>
        /// 生成签名(仿客户端)
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="type">1:WeChatApp_ApiKey 2:iOS_ApiKey 3:Android_ApiKey</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult GenSSign1(string mobile, int type)
        {
            var apikey = string.Empty;
            string WeChatApp_ApiKey = "23ACA89E-0169-44A1-8D08-64AD3EC4A551";
            string iOS_ApiKey = "972F5D56-296B-4BD7-8CB6-49617A441A99";
            string Android_ApiKey = "506DDE1D-4642-4859-9226-E26B098FC02B";

            if (type == 1)
            {
                apikey = WeChatApp_ApiKey;
            }
            else if (type == 2)
            {
                apikey = iOS_ApiKey;
            }
            else if (type == 3)
            {
                apikey = Android_ApiKey;
            }
            ResponsResult result = new ResponsResult
            {
                Data = Security.Sign(mobile, apikey)
            };
            return result;
        }
        //登录-注册微信授权

        //
        /// <summary>
        /// 登录-注册//手机接码/邮箱接码
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="sign">签名</param>
        /// <returns>
        /// <see cref="ApiAccountModel"/>
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult LoginWithCode(string mobile, string code, string sign)
        {
            //TODO:this.ApiKey, this.SourceType
            string Android_ApiKey = "506DDE1D-4642-4859-9226-E26B098FC02B";
            SourceType sourceType = SourceType.Android;
            return _memberService.Login(mobile, code, sign, Android_ApiKey, sourceType);
        }
        //浏览记录/分享记录
    }

}