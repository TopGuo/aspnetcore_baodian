using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TB.AspNetCore.Domain.Config;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Domain.Enums;
using TB.AspNetCore.Domain.Models.Api;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Extensions;

namespace TB.AspNetCore.Application.Services
{
    public class RpwService : BaseService, IRpwService
    {
        private readonly IMapper _mapper;
        public RpwService(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponsResult AddMsgInfo(string accountId, AddRpwModel model)
        {
            ResponsResult result = new ResponsResult();
            if (string.IsNullOrEmpty(model.Content))
            {
                return result.SetStatus(ErrorCode.RpwMsgEmpty);
            }
            if (model.Pics.Count <= 0)
            {
                return result.SetStatus(ErrorCode.PicsEmpty);
            }
            MsgContent msgContent = new MsgContent()
            {
                AccountId = accountId,
                AreaType = (int)model.Type,
                ContextType = (int)MsgContextType.hot,
                Status = (int)MsgStatus.NoReviewed,
                Content = model.Content,
                Pics = model.Pics.GetJson(),
                CreateTime = DateTime.Now,
                Longtude = model.Longtude,
                Latiude = model.Latitude
            };
            base.Add(msgContent, true);
            return result;
        }

        /// <summary>
        /// 添加红包信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponsResult AddRpwInfo(string accountId, AddRpwModel model)
        {
            ResponsResult result = new ResponsResult();
            if (string.IsNullOrEmpty(model.Content))
            {
                return result.SetStatus(ErrorCode.RpwMsgEmpty);
            }
            if (model.Pics.Count < 0)
            {
                return result.SetStatus(ErrorCode.PicsEmpty);
            }
            if (!model.RpwCounts.HasValue)
            {
                return result.SetStatus(ErrorCode.TotalAmount);
            }
            if (!model.RpwTotalAmount.HasValue)
            {
                return result.SetStatus(ErrorCode.AmountEmpty);
            }
            if (model.RpwCounts <= 0 || model.RpwTotalAmount <= 0)
            {
                return result.SetStatus(ErrorCode.AmountPub);
            }

            MsgContent msgContent = new MsgContent()
            {
                AccountId = accountId,
                AreaType = (int)model.Type,
                ContextType = (int)MsgContextType.Rpw,
                Status = (int)MsgStatus.NoReviewed,
                Content = model.Content,
                Pics = model.Pics.GetJson(),
                CreateTime = DateTime.Now,
                Longtude = model.Longtude,
                Latiude = model.Latitude,
                RemainCounts = model.RpwCounts,
                TotalCounts = model.RpwCounts,
                TotalPrice = model.RpwTotalAmount

            };
            if (model.IsHasOutLink && !string.IsNullOrEmpty(model.OutLinkStr))
            {
                msgContent.OutLink = model.OutLinkStr;
            }
            base.Add(msgContent);
            //写入订单记录
            Orders orders = new Orders()
            {
                Id = this.GenNewGuid(),
                AccountId = accountId,
                SourceId = msgContent.Id.ToString(),
                OrderType = (int)OrderType.RpwRecharge,
                Amount = model.RpwTotalAmount ?? 0,
                PayType = (int)PayType.None,
                OrderStatus = (int)OrderStatus.Pending,
                ActualAmount = model.RpwTotalAmount ?? 0,
                CreateTime = DateTime.Now,
            };
            base.Add(orders, true);
            return result;
        }

        /// <summary>
        /// 获取类广告信息
        /// </summary>
        /// <param name="type">入参</param>
        /// <returns></returns>
        public ResponsResult GetAds(AdLocation type)
        {
            ResponsResult result = new ResponsResult();
            var list = base.Where<Advertising>(t => t.AdLocation.Equals((int)type) && t.IsEnable == true).Select(t => new AdvertisingModel
            {
                AdLocation = (AdLocation)t.AdLocation,
                AdName = t.AdName,
                AdPic = t.AdPic,
                Id = t.Id,
                BeginTime = t.BeginTime,
                AdDesc = t.AdDesc
            }).ToList();
            list.ForEach(t =>
            {
                t.AdPic = t.AdPic.GetPWFullPath(SystemSettingService.SystemSetting.WebSite);
            });
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 获取热点消息
        /// </summary>
        /// <param name="longtude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public ResponsResult GetMsgInfo(decimal longtude, decimal latitude)
        {
            ResponsResult result = new ResponsResult();
            var info = base.Where<MsgContent>(t => t.AreaType == (int)AllOrLocal.All && t.ContextType == (int)MsgContextType.hot).Select(m => new MsgContentModel
            {
                TotalCounts = m.TotalCounts,
                TotalPrice = m.TotalPrice,
                RemainCounts = m.RemainCounts,
                Content = m.Content,
                Pics = m.Pics.GetModelList<string>("").GetPwFullPath(SystemSettingService.SystemSetting.ApiSite+"/"),
            }).ToList();
            result.Data = info;
            return result;
        }
        

        /// <summary>
        /// 获取红包墙消息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="longtude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public ResponsResult GetRpwInfo(string accountId, decimal longtude, decimal latitude)
        {
            ResponsResult result = new ResponsResult();
            var info = base.Where<MsgContent>(t => t.AreaType == (int)AllOrLocal.All && t.ContextType == (int)MsgContextType.Rpw).Select(m => new MsgContentModel
            {
                TotalCounts = m.TotalCounts,
                TotalPrice = m.TotalPrice,
                RemainCounts = m.RemainCounts,
                Content = m.Content,
                Pics = m.Pics.GetModelList<string>("").GetPwFullPath(SystemSettingService.SystemSetting.ApiSite + "/"),
            }).ToList();
            result.Data = info;
            return result;
        }
    }
}
