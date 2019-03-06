using System;
using System.Collections.Generic;
using System.Text;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Models.Web;

namespace TB.AspNetCore.Domain.Repositorys
{
    /// <summary>
    /// 公共服务/接码...
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 保存帮助管理内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult SaveHelp(InformationModel model);

        /// <summary>
        /// 获取帮助管理内容
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponsResult GetInformation(InformationType type, string id);

        /// <summary>
        /// 获取帮助管理内容2
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ResponsResult ApiGetInformation(InformationType type);
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        ResponsResult GetValidCode(string mobile);
        /// <summary>
        /// api获取手机验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="sign"></param>
        /// <param name="apiKey"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        ResponsResult ApiGetVerifyCode(string mobile, string sign, string apiKey, string accountId = "");

        /// <summary>
        /// 获取短信
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult GetSMSList(SmsSearchModel model);
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult AdManagerList(AdManagerViewSearch model);
        /// <summary>
        /// 获取单条广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetAdPicModel(string id);

        /// <summary>
        /// 删除广告位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult DelAdPic(AdManagerViewSearch model);

        /// <summary>
        /// 禁用启用广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult IsEnableAdPic(AdManagerViewSearch model);
        /// <summary>
        /// 添加广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult AdPicAdd_Updata(AdManagerViewSearch model);

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult SaveSetting(SystemSettingModel model);

        /// <summary>
        /// 保存android配置
        /// </summary>
        /// <param name="android"></param>
        /// <returns></returns>
        ResponsResult SaveAndroid(AndroidVersion android);

        /// <summary>
        /// 保存ios配置
        /// </summary>
        /// <param name="ios"></param>
        /// <returns></returns>
        ResponsResult SaveIOS(IosVersion ios);
    }
}
