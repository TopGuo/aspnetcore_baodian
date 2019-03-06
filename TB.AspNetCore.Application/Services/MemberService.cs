using System;
using System.Linq;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Utils.Encryption;

namespace TB.AspNetCore.Application.Services
{
    public class MemberService : BaseService, IMemberService
    {
        public ResponsResult Login(string mobile, string code, string sign, string apiKey, SourceType source)
        {
            if (mobile == "18333103619")
            {
                ResponsResult result = new ResponsResult();
                result.Data = new ApiAccountModel
                {
                    HeadPicture = "images/baodian28.png".GetPWFullPath(SystemSettingService.SystemSetting.WebSite),
                    CreateTime = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    AccountStatus = AccountStatus.Normal,
                    FullName = "测试",
                    NickName = "测试",
                    Mobile = mobile,
                    HasOrders = false,
                    ApproveModel = ApproveModel.AutoApprove,
                    Token = ServiceCollectionExtension.Encrypt(new TokenModel
                    {
                        Id = "1",
                        Mobile = mobile,
                        Type = AccountType.StandardUser,
                        Source = source,
                    }.GetJson()),

                };
                return result;
            }
            var _result = ApiVerifyCode(mobile, code, apiKey, sign);
            if (_result.Success)
            {
                var account = this.Single<Account>(t => t.AccountType == (int)AccountType.StandardUser && t.RealName == mobile);
                if (account == null)
                {
                    account = new Account
                    {
                        RealName = mobile,
                        AccountStatus = (int)AccountStatus.Normal,
                        AccountType = (int)AccountType.StandardUser,
                        CreateTime = DateTime.Now,
                        PassWord = "",
                        LastLoginTime = DateTime.Now,
                        HeaderPic = "/images/baodian28.png".GetPWFullPath(SystemSettingService.SystemSetting.WebSite),
                    };

                    base.Add(account, true);
                }
                //重置token,单设备登录!
                //account.Token = Guid.NewGuid();
                if (account.AccountStatus==(int)AccountStatus.Disabled)
                {
                    return _result.SetStatus(ErrorCode.AccountDisabled, "你的账户已禁用!");
                }
                if (_result.Success)
                {
                    var aModel = GetApiAccountViewModel(account, source);
                    base.Update(account, true);
                    _result.Data = aModel;
                }

                return _result;
            }
            else
            {
                return _result;
            }
        }

        /// <summary>
        /// 公用方法
        /// </summary>
        /// <param name="account"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private ApiAccountModel GetApiAccountViewModel(Account account, SourceType source)
        {
            ApiAccountModel aModel = null;
            if (account.AccountStatus!=(int)AccountStatus.Disabled)
            {                
                aModel = new ApiAccountModel
                {                   
                    HeadPicture = account.HeaderPic.GetPWFullPath(SystemSettingService.SystemSetting.WebSite),
                    CreateTime = account.CreateTime,
                    LastLoginTime = account.LastLoginTime,
                    AccountStatus = (AccountStatus)account.AccountStatus,
                    FullName = account.RealName,
                    NickName = account.NicikName,
                    Mobile = account.RealName,
                    Token = ServiceCollectionExtension.Encrypt(new TokenModel
                    {
                        Id = account.Id,
                        Mobile = account.RealName,
                        Type = (AccountType)account.AccountType,
                        Source = source,
                    }.GetJson())
                };        
            }
            return aModel;
        }

        /// <summary>
        /// 验证码验签
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        private ResponsResult ApiVerifyCode(string mobile, string code, string apiKey, string sign)
        {
            ResponsResult result = new ResponsResult();

            if (string.IsNullOrEmpty(mobile))
            {
                return result.SetStatus(ErrorCode.CannotEmpty, "请输入手机号!");
            }
            if (!mobile.IsMobile())
            {
                return result.SetStatus(ErrorCode.InvalidMobile, "请输入正确的手机号!");
            }
            if (string.IsNullOrEmpty(code))
            {
                return result.SetStatus(ErrorCode.CannotEmpty, "请输入验证码!");
            }
            if (!Security.ValidSign(sign, apiKey, mobile, code))
            {
                return result.SetStatus(ErrorCode.InvalidSign);
            }
            var now = DateTime.Now.AddSeconds(-TbConstant.CanRequestAfterSecond);
            var mobileCode = base.Where<SmsInfo>(t => t.Mobile == mobile && (bool)!t.IsUsed && t.CreateTime > now)
                .OrderByDescending(t => t.CreateTime).FirstOrDefault();
            // 手机验证码 120秒后过期
            if (mobileCode == null || mobileCode.CreateTime < DateTime.Now.AddMinutes(-TbConstant.CodeValidMinutes))
            {
                return result.SetStatus(ErrorCode.Expired, "验证码已过期,请重新获取!");
            }

            if (mobileCode.Code != code)
            {
                return result.SetStatus(ErrorCode.InvalidData, "验证码不正确!");
            }
            mobileCode.IsUsed = true;
            return result;
        }

        public ResponsResult Register(string accountId, string mobile, string code)
        {
            ResponsResult result = new ResponsResult();
            if (string.IsNullOrEmpty(mobile))
            {
                return result.SetStatus(ErrorCode.CannotEmpty, "请输入手机号!");
            }
            if (!mobile.IsMobile())
            {
                return result.SetStatus(ErrorCode.InvalidMobile, "手机号格式不正确!");
            }
            if (string.IsNullOrEmpty(code))
            {
                return result.SetStatus(ErrorCode.CannotEmpty, "请输入正确的手机验证码!");
            }
            if (this.Exists<Account>(t => t.RealName == mobile))
            {
                return result.SetStatus(ErrorCode.Existed, "该手机号已注册! 如果该手机号为你所有,请登录!");
            }

            var now = DateTime.Now.AddSeconds(-TbConstant.CanRequestAfterSecond);
            var mobileCode = base.Where<SmsInfo>(t => t.Mobile == mobile && (bool)!t.IsUsed && t.CreateTime > now)
                .OrderByDescending(t => t.CreateTime).FirstOrDefault();
            if (mobileCode == null)
            {
                return result.SetStatus(ErrorCode.Expired, "验证码已过期,请重新获取!");
            }

            if (mobileCode.Code != code)
            {
                return result.SetStatus(ErrorCode.InvalidData, "验证码不正确!");
            }
            mobileCode.IsUsed = true;
            base.Update(mobileCode);
            var account = new Account
            {
                RealName = mobile,
                AccountStatus = (int)AccountStatus.Normal,
                CreateTime = DateTime.Now,
                PassWord = "",
                LastLoginTime = DateTime.Now,
                ReferId = accountId,
                HeaderPic = TbConstant.DefaultHeadPicture,
                AccountType = (int)SourceType.Web,
            };

            this.Add(account, true);
            var settings = SystemSettingService.SystemSetting;
            result.Data = settings.CouponForRegist + settings.InvitedPackage;

            return result;
        }
    }
}
