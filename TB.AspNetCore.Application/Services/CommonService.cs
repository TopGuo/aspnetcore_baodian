using System;
using System.Collections.Generic;
using System.Linq;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Models.Web;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Extensions;
using TB.AspNetCore.Infrastructrue.Utils.Encryption;

namespace TB.AspNetCore.Application.Services
{
    public class CommonService : BaseService, ICommonService
    {
        SystemSettingService systemSettingService = new SystemSettingService();
        #region 广告
        public ResponsResult AdManagerList(AdManagerViewSearch model)
        {
            ResponsResult result = new ResponsResult();
            var query = base.Query<Advertising>();
            if (!string.IsNullOrEmpty(model.AdName))
            {
                query = query.Where(t => t.AdName == model.AdName);
            }
            if ((int)model.AdLocation > 0)
            {
                query = query.Where(t => t.AdLocation == (int)model.AdLocation);
            }
            List<AdManagerViewSearch> _list = new List<AdManagerViewSearch>();
            query.OrderByDescending(t => t.Id).Pages(model.PageIndex, model.PageSize, out int count).Select(t => new
            {
                t.Id,
                t.AdLocation,
                t.AdName,
                t.AdPic,
                t.BeginTime,
                t.EndTime,
                t.IsEnable,
                t.AdLink

            }).ToList().ForEach(t =>
            {
                _list.Add(
                    new AdManagerViewSearch
                    {
                        Id = t.Id,
                        AdName = t.AdName,
                        BeginTime = t.BeginTime,
                        EndTime = t.EndTime,
                        AdPic = t.AdPic,
                        AdLocationName = JsonExtensions.GetString((AdLocation)t.AdLocation),
                        IsEnable = (bool)t.IsEnable,//查询是几就是几
                    });
            }
            );
            result.Data = _list;
            result.RecordCount = count;
            return result;
        }

        public ResponsResult DelAdPic(AdManagerViewSearch model)
        {
            ResponsResult result = new ResponsResult();
            Advertising adPic = new Advertising();
            adPic.Id = model.Id;
            this.Delete(adPic, true);
            result.Message = "删除成功";
            return result;
        }
        public ResponsResult IsEnableAdPic(AdManagerViewSearch model)
        {
            ResponsResult result = new ResponsResult();
            var query = base.Where<Advertising>(s => s.Id == model.Id).FirstOrDefault();
            if (query == null)
            {
                result.Message = "操作失败！";
                return result;
            }
            query.IsEnable = model.IsEnable;//跟新赋值
            base.Update(query, true);
            result.Message = "操作成功！";
            return result;
        }
        public ResponsResult AdPicAdd_Updata(AdManagerViewSearch model)
        {
            ResponsResult result = new ResponsResult();
            Advertising adPic;
            if (!string.IsNullOrEmpty(model.Id))
            {
                adPic = this.First<Advertising>(t => t.Id != model.Id && t.AdName == model.AdName && t.AdLocation == (int)model.AdLocation);
            }
            else
            {
                adPic = this.First<Advertising>(t => t.AdName == model.AdName && t.AdLocation == (int)model.AdLocation);
            }

            if (adPic != null)
            {
                return result.SetStatus(ErrorCode.NotFound, "广告位已经存在！");
            }
            else
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    adPic = new Advertising();
                    adPic = model.GetJson().GetModel<Advertising>();
                    adPic.AdDesc = model.AdDesc;
                    adPic.IsEnable = true;//添加默认启用
                }
                else
                {
                    adPic = this.First<Advertising>(t => t.Id == model.Id);
                    adPic.AdLocation = (int)model.AdLocation;
                    adPic.AdName = model.AdName;
                    adPic.AdPic = model.AdPic;
                    adPic.BeginTime = model.BeginTime;
                    adPic.EndTime = model.EndTime;
                    adPic.AdDesc = model.AdDesc;
                    adPic.AdLink = model.AdLink;
                    adPic.IsEnable = model.IsEnable;

                }
            }

            if (!string.IsNullOrEmpty(model.Id))
            {
                this.Update(adPic, true);
            }
            else
            {
                adPic.Id = Guid.NewGuid().ToString("N");
                this.Add(adPic, true);
            }
            return result;
        }
        public string GetAdPicModel(string id)
        {
            Advertising adPic;
            if (string.IsNullOrEmpty(id))
            {
                adPic = new Advertising();
                adPic.AdPic = "";
                adPic.AdLocation = (int)AdLocation.Home;
            }
            else
            {
                adPic = this.First<Advertising>(t => t.Id == id);
            }
            return adPic.GetJson();
        }
        #endregion

        public ResponsResult GetSMSList(SmsSearchModel model)
        {
            ResponsResult result = new ResponsResult();
            var query = base.Query<SmsInfo>();
            if (!string.IsNullOrEmpty(model.IP))
            {
                query = query.Where(t => t.Ip.Contains(model.IP));
            }
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                query = query.Where(t => t.Mobile.Contains(model.Mobile));
            }
            if (model.StartDate.HasValue)
            {
                query = query.Where(t => t.CreateTime >= model.StartDate);
            }
            if (model.EndDate.HasValue)
            {
                query = query.Where(t => t.CreateTime <= model.EndDate);
            }
            result.Data = query.OrderByDescending(t => t.CreateTime).Pages(model.PageIndex, model.PageSize, out int count).ToList();
            result.RecordCount = count;
            return result;
        }

        public ResponsResult GetValidCode(string mobile)
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
            if (this.Exists<Account>(t => t.RealName == mobile))
            {
                return result.SetStatus(ErrorCode.Existed, "该手机号已注册! 如果该手机号为你所有,请登录!");
            }
            var message = CheckRequest(ServiceCollectionExtension.ClientIP, mobile);
            if (!string.IsNullOrEmpty(message))
            {
                return result.SetStatus(ErrorCode.BadRequest, message);
            }
            var codeList = Enumerable.Range(0, 9).OrderBy(t => Guid.NewGuid()).Skip(3).Take(6);
            var code = string.Join("", codeList);
            result.Message = $"验证码已发送到你的手机,有效时长{TbConstant.CodeValidMinutes}分钟";
            //MobileRequest request = new MobileRequest(mobile, $"{{code:'{code}',product:'{Constant.CompanyName}'}}");
            //var response = request.Excute();
            //if (!response.Success || response.ErrCode != 0)
            //{
            //    return result.SetError(response.Msg);
            //}
            SmsInfo sms = new SmsInfo
            {
                Code = code,
                Contents = "获取验证码",
                Ip = ServiceCollectionExtension.ClientIP,
                Mobile = mobile,
                CreateTime = DateTime.Now,
            };
            base.Add(sms, true);
            return result;
        }

        /// <summary>
        /// api获取手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="sign"></param>
        /// <param name="apiKey"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public ResponsResult ApiGetVerifyCode(string mobile, string sign, string apiKey, string accountId = "")
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
            if (string.IsNullOrEmpty(apiKey))
            {
                return result.SetStatus(ErrorCode.CannotEmpty, "输入不正确!");
            }
            if (!Security.ValidSign(sign, apiKey, mobile))
            {
                return result.SetStatus(ErrorCode.InvalidSign);
            }
            var message = CheckRequest(ServiceCollectionExtension.ClientIP, mobile);
            if (!string.IsNullOrEmpty(message))
            {
                return result.SetStatus(ErrorCode.BadRequest, message);
            }
            var codeList = Enumerable.Range(0, 9).OrderBy(t => Guid.NewGuid()).Skip(3).Take(6);
            var code = string.Join("", codeList);
            result.Message = $"验证码已发送到你的手机,有效时长{TbConstant.CodeValidMinutes}分钟";

            /**/
            SmsInfo sms = new SmsInfo
            {
                Code = code,
                Contents = "获取验证码",
                Ip = ServiceCollectionExtension.ClientIP,
                Mobile = mobile,
                CreateTime = DateTime.Now,
                AccountId = accountId
            };
            base.Add(sms, true);
            return result;
        }

        protected string CheckRequest(string ip, string mobile)
        {
            var now = DateTime.Now.AddSeconds(-TbConstant.CanRequestAfterSecond);
            var mobileCount = base.Where<SmsInfo>(t => t.Mobile == mobile && t.CreateTime > now).Count();
            if (mobileCount > 0)
            {
                if (mobileCount > 2)
                {
                    return "请求频繁,请稍后重试1!";
                }
                return "验证码已发送,请耐心等待.";
            }
            var ipCount = base.Where<SmsInfo>(t => t.Ip == ip && t.CreateTime > now).Count();
            if (ipCount > 15)
            {
                return "请求频繁,请稍后重试2!";
            }
            return string.Empty;
        }

        #region 保存系统配置
        public ResponsResult SaveSetting(SystemSettingModel model)
        {
            ResponsResult result = new ResponsResult();
            systemSettingService.SaveSettings(model);
            return result;
        }

        public ResponsResult SaveAndroid(AndroidVersion android)
        {
            ResponsResult result = new ResponsResult();
            systemSettingService.SaveSettings(android);
            return result;
        }

        public ResponsResult SaveIOS(IosVersion ios)
        {
            ResponsResult result = new ResponsResult();
            systemSettingService.SaveSettings(ios);
            return result;
        }

        #region 帮助管理模块
        public ResponsResult SaveHelp(InformationModel model)
        {
            ResponsResult result = new ResponsResult();

            if ((int)model.Type < 1)
            {
                return result.SetError("请选择类型");
            }
            if (model.Type != InformationType.UseGuide)
            {
                var info = this.First<Informations>(t => t.Type == (int)model.Type);
                if (info == null)
                {
                    info = new Informations { Id = this.GenNewGuid(), CreateTime = DateTime.Now, Type = (int)model.Type, Title = model.Type.GetString() };
                    this.Add(info);
                }
                else
                {
                    info.UpdateTime = DateTime.Now;
                    this.Update(info);
                }
                info.Description = model.Description;
                model.Id = info.Id;
                this.Save();
            }
            else
            {
                if (string.IsNullOrEmpty(model.Title))
                {
                    return result.SetError("请输入标题");
                }
                var info = this.Single<Informations>(t => t.Id == model.Id);
                if (info == null)
                {
                    info = new Informations { Id = this.GenNewGuid(), CreateTime = DateTime.Now, Type = (int)model.Type };
                    this.Add(info);
                }
                else
                {
                    info.UpdateTime = DateTime.Now;
                    this.Update(info);
                }
                info.Title = model.Title.Trim();
                info.Description = model.Description;
                model.Id = info.Id;
                this.Save();
            }
            result.Data = model.Id;
            return result;
        }

        public ResponsResult GetInformation(InformationType type, string id)
        {
            ResponsResult result = new ResponsResult();
            Informations info = null;
            if (type != InformationType.UseGuide)
            {
                info = this.First<Informations>(t => t.Type == (int)type);
            }
            else
            {
                info = this.Single<Informations>(t => t.Id == id);
            }
            if (info == null)
            {
                info = default(Informations);
            }
            result.Data = info;
            return result;
        }
        public ResponsResult ApiGetInformation(InformationType type)
        {
            ResponsResult result = new ResponsResult();
            if (type != InformationType.UseGuide)
            {
                result.Data = this.First<Informations>(t => t.Type == (int)type)?.Description ?? "";
            }
            else
            {
                result.Data = this.Where<Informations>(t => t.Type == (int)type).Select(t => new { t.Description, t.Id, t.Title, t.Type }).ToList();
            }
            return result;
        }
        #endregion
        #endregion
    }
}
