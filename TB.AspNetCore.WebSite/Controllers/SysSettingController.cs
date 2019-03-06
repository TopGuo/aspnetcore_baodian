using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TB.AspNetCore.Application.Action;
using TB.AspNetCore.Domain.Repositorys;

namespace TB.AspNetCore.WebSite.Controllers
{
    public class SysSettingController : WebBase
    {
        public ICommonService _commonService { get; set; }

        #region 帮助管理
        [Action("帮助管理", ActionType.ContentManager, 1)]
        [Route("SysSetting/Helps")]
        public ViewResult Helps()
        {
            return View();
        }
        #endregion

        [Route("SysSetting/Settings")]
        [Action("系统设置", ActionType.SystemManager, 4)]
        public ViewResult Settings()
        {
            return View();
        }
        [Route("SysSetting/SmsGo")]
        [Action("短信明细", ActionType.TongJi, 1)]
        public ViewResult SmsGo()
        {
            return View();
        }

        [Action("广告管理", ActionType.SystemManager, 3)]
        public ViewResult AdManager()
        {
            return View();
        }
        /// <summary>
        /// 添加广告管理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("SysSetting/AdPicAdd_Updata")]
        public ViewResult AdPicAdd_Updata(string id)
        {
            string title = "添加广告";
            if (id != null && !string.IsNullOrEmpty(id))
            {
                title = "修改广告";
            }
            else
            {
                id = string.Empty;
            }
            ViewBag.AdModel = _commonService.GetAdPicModel(id);
            ViewBag.title = title;

            return View();
        }
    }
}