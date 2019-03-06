using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Services;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Repositorys;

namespace TB.AspNetCore.Api.Controllers
{
    /// <summary>
    /// 红包墙模块
    /// </summary>
    [Route("api/rpw/[action]")]
    [ApiController]
    public class RpwController : ApiBaseController
    {
        //
        /// <summary>
        /// 获取红包墙消息
        /// </summary>
        IRpwService RpwService { get; set; }

        //
        /// <summary>
        /// 获取热点消息内容
        /// </summary>
        /// <param name="longtude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <returns>
        /// <see cref="MsgContentModel" langword="true"/>
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult GetHotInfo(decimal longtude, decimal latitude)
        {
            return RpwService.GetMsgInfo(longtude, latitude);
        }

        /// <summary>
        /// 添加热点信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult AddHotInfo([FromBody]AddRpwModel model)
        {
            return RpwService.AddMsgInfo(this.TokenModel.Id, model);
        }

        //
        /// <summary>
        /// 发布红包墙消息-
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ResponsResult AddRpwInfo([FromBody]AddRpwModel model)
        {
            return RpwService.AddRpwInfo(this.TokenModel.Id,model);
        }
        //
        /// <summary>
        /// 获取红包墙信息
        /// </summary>
        /// <param name="longtude"></param>
        /// <param name="latitude"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult GetRpwInfo(decimal longtude, decimal latitude,int pageIndex,int pageSize)
        {
            return RpwService.GetRpwInfo(this.TokenModel.Id,longtude, latitude);
        }
        //
        /// <summary>
        /// AndroidApp版本检查
        /// </summary>
        /// <returns>
        /// s<see cref="AndroidVersion"/>
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult GetAppAndroidVersion()
        {
            ResponsResult result = new ResponsResult();
            result.Data = SystemSettingService.AndroidVersion;
            return result;
        }
        /// <summary>
        /// IOSApp版本检查
        /// </summary>
        /// <returns>
        /// s<see cref="IosVersion"/>
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        public ResponsResult GetAppIosVersion()
        {
            ResponsResult result = new ResponsResult();
            result.Data = SystemSettingService.IosVersion;
            return result;
        }
        //
        /// <summary>
        /// 获取广告/banner/公告
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// <see cref="AdvertisingModel" langword="true"/>
        /// </returns>
        [HttpGet]
        [AllowAnonymous]
        public ResponsResult GetAd(AdLocation type)
        {
            return RpwService.GetAds(type);

        }
    }
}