using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Logs;
using TB.AspNetCore.Infrastructrue.Utils.Cookie;

namespace TB.AspNetCore.Api.Controllers
{
    public class ApiBaseController : BaseController
    {
        SortedDictionary<string, string> RequestParams = new SortedDictionary<string, string>();

        protected const string TOKEN_Name = "token";
        protected SourceType SourceType { get; private set; }
        protected string ApiKey { get; private set; }
        protected TokenModel TokenModel { get; private set; } = new TokenModel();

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                #region
                var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
                if (userAgent.Contains("MicroMessenger"))
                {
                    SourceType = SourceType.WeChatApp;
                    ApiKey = TbConstant.WeChatApp_ApiKey;
                }
                else if (userAgent.Contains("iPhone") || userAgent.Contains("iPod") || userAgent.Contains("iPad"))
                {
                    SourceType = SourceType.IOS;
                    ApiKey = TbConstant.iOS_ApiKey;
                }
                else if (userAgent.Contains("Android"))
                {
                    SourceType = SourceType.Android;
                    ApiKey = TbConstant.Android_ApiKey;
                }
                foreach (var kv in context.HttpContext.Request.Query)
                {
                    RequestParams[kv.Key] = kv.Value.ToString();
                }
                if (context.HttpContext.Request.HasFormContentType)
                {
                    foreach (var kv in context.HttpContext.Request.Form)
                    {
                        RequestParams[kv.Key] = kv.Value.ToString();
                    }
                }
                var values = context.HttpContext.GetValues();
                foreach (var kv in values)
                {
                    RequestParams[kv.Key] = kv.Value.ToString();
                }
                var dict = new Dictionary<string, string>();
                foreach (var kv in RequestParams)
                {
                    if (kv.Value.Length > 500)
                    {
                        dict[kv.Key] = kv.Value.Substring(0, 500);
                    }
                    else
                    {
                        dict[kv.Key] = kv.Value;
                    }
                }
                if (SourceType != SourceType.Unknown)
                {
                    //context.Result = new ObjectResult(new Result().SetStatus(ErrorCode.Unauthorized, "请设置User-Agent请求头: 如:iPhone 或者 Android"));
                }
                else
                {
                    var token = string.Empty;
                    if (RequestParams.ContainsKey(TOKEN_Name))
                    {
                        token = RequestParams[TOKEN_Name];
                    }
                    if (string.IsNullOrEmpty(token))
                    {
                        token = context.HttpContext.Request.Cookies["token"];
                        //token = CookieUtility.GetCookie("token");
                    }
                    if (!context.ActionDescriptor.FilterDescriptors.Any(t => t.Filter is AllowAnonymousFilter))
                    {
                        if (string.IsNullOrEmpty(token))
                        {
                            context.Result = new ObjectResult(new ResponsResult(ErrorCode.Unauthorized, "The request is not contains parameter 'token'"));
                        }
                        else
                        {
                            var model = CheckToken(token);
                            if (model.Success)
                            {

                                if (string.IsNullOrEmpty(this.TokenModel.Id))
                                {
                                    model.SetStatus(ErrorCode.Relogin, "请登录");
                                }
                                else
                                {
                                    //检查用户的token是否匹配
                                    //var _db = ContextHelper.New<DataContext>();
                                    //if (!_db.Account.Any(t => t.Id == Token.Id && t.Token == Token.UserToken))
                                    //{
                                    //    model.SetError("你的账户已在其他设备登录,请重新登录!", ResponseStatus.Relogin);
                                    //}
                                }
                            }
                            if (!model.Success)
                            {
                                context.Result = new ObjectResult(model);
                            }
                        }
                    }
                    else
                    {
                        string json = ServiceCollectionExtension.Decrypt(token);
                        if (string.IsNullOrEmpty(json))
                        {
                            this.TokenModel = new TokenModel();
                        }
                        else
                        {
                            this.TokenModel = json.GetModel<TokenModel>();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Log4Net.Debug(ex);
            }
            return base.OnActionExecutionAsync(context, next);
        }

        /// <summary>
        /// 检查返回的token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected ResponsResult CheckToken(string token)
        {
            ResponsResult rModel = new ResponsResult();
            try
            {
                string json = ServiceCollectionExtension.Decrypt(token);
                if (string.IsNullOrEmpty(json))
                {
                    return rModel.SetStatus(ErrorCode.Relogin, "token不正确.请重新登录!");
                }
                this.TokenModel = json.GetModel<TokenModel>();
                if (this.TokenModel == null)
                {
                    return rModel.SetStatus(ErrorCode.InvalidToken, "非法请求.");
                }
                if (string.IsNullOrEmpty(this.TokenModel.Id))
                {
                    return rModel.SetStatus(ErrorCode.InvalidToken, "无效token.");
                }
                //缓存+token对比时间做单点登录
                //检查token里的token是否在系统注册
                //Bicycle.Service.RegistActions
                //if (!Bicycle.Data.Entity.SystemToken.Tokens.Exists(t => this.Token.Token == t.Token))
                //{
                //    return rModel.SetError("不匹配,非法请求.", ResponseStatus.NotAcceptable);
                //}
            }
            catch (Exception ex)
            {
                return rModel.SetStatus(ErrorCode.SystemError, $"请求失败.{ex.Message}");
            }

            return rModel;
        }
    }
}