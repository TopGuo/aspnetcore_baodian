using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;

namespace TB.AspNetCore.Domain.Repositorys
{
    public interface IRpwService
    {
        //
        /// <summary>
        /// 获取红包墙信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="longtude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        ResponsResult GetRpwInfo(string accountId, decimal longtude, decimal latitude);

        //
        /// <summary>
        /// 获取广告消息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ResponsResult GetAds(AdLocation type);

        //
        /// <summary>
        /// 获取普通消息
        /// </summary>
        /// <param name="longtude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        ResponsResult GetMsgInfo(decimal longtude, decimal latitude);

        //
        /// <summary>
        /// 新增红包墙消息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult AddRpwInfo(string accountId,AddRpwModel model);
        //
        /// <summary>
        /// 新增普通消息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResponsResult AddMsgInfo(string accountId,AddRpwModel model);



    }
}
